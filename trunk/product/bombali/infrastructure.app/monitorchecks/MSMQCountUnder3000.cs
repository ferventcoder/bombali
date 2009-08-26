namespace bombali.infrastructure.app.monitorchecks
{
    using System;
    using System.ComponentModel;
    using System.Messaging;
    using System.Runtime.InteropServices;
    using System.Transactions;
    using logging;

    public class MSMQCountUnder3000 : ICheck
    {
        private double failure_count = 0d;

        private const int message_count_threshhold = 3000;

        public string last_response { get; private set; }

        public bool run_check(string what_to_check)
        {
            bool successful_check = true;
            int message_count = get_message_count_for_queue_at(what_to_check);

            last_response = message_count.ToString();

            if (message_count > message_count_threshhold)
            {
                successful_check = false;
                failure_count += 1;
                Log.bound_to(this).Warn(
                    "{0} warning! Queue {1} is over the threshold of {2} with a count of {3} messages. This has happened {3} times.",
                    ApplicationParameters.name, what_to_check, last_response, failure_count);

            }
            else
            {
                failure_count = 0;
                Log.bound_to(this).Info("{0} found queue {1} with a count of {2} messages.", ApplicationParameters.name,
                                                what_to_check, last_response);
            }

            return successful_check;
        }

        private static int get_message_count_for_queue_at(string address_to_queue)
        {
            QueueAddress queue_address = new QueueAddress(address_to_queue);

            MessageQueue queue = new MessageQueue(queue_address.FormatName, false, false);
            //if (queue_address.IsLocal)
            //{
            //    queue = new MessageQueue(queue_address.LocalName,false,false);    
            //}

            return queue.GetCount();
        }

    }

    public static class NativeMethods
    {
        public const int MQ_MOVE_ACCESS = 4;
        public const int MQ_DENY_NONE = 0;

        [DllImport("mqrt.dll", CharSet = CharSet.Unicode)]
        public static extern int MQOpenQueue(string formatName, int access, int shareMode, ref IntPtr hQueue);

        [DllImport("mqrt.dll")]
        public static extern int MQCloseQueue(IntPtr queue);

        [DllImport("mqrt.dll")]
        public static extern int MQMoveMessage(IntPtr sourceQueue, IntPtr targetQueue, long lookupID, IDtcTransaction transaction);

        [DllImport("mqrt.dll")]
        public static extern int MQMgmtGetInfo([MarshalAs(UnmanagedType.BStr)]string computerName, [MarshalAs(UnmanagedType.BStr)]string objectName, ref MQMGMTPROPS mgmtProps);

        public const byte VT_NULL = 1;
        public const byte VT_UI4 = 19;
        public const int PROPID_MGMT_QUEUE_MESSAGE_COUNT = 7;

        //size must be 16
        [StructLayout(LayoutKind.Sequential)]
        public struct MQPROPVariant
        {
            public byte vt;       //0
            public byte spacer;   //1
            public short spacer2; //2
            public int spacer3;   //4
            public uint ulVal;    //8
            public int spacer4;   //12
        }

        //size must be 16 in x86 and 28 in x64
        [StructLayout(LayoutKind.Sequential)]
        public struct MQMGMTPROPS
        {
            public uint cProp;
            public IntPtr aPropID;
            public IntPtr aPropVar;
            public IntPtr status;

        }
    }

    /// <summary>
    /// Deals with the various versions of queue addresses required when dealing with MSMQ
    /// </summary>
    public class QueueAddress
    {
        private const string _localhost = "localhost";

        static QueueAddress()
        {
            LocalMachineName = Environment.MachineName.ToLowerInvariant();
        }

        public QueueAddress(Uri uri)
        {
            PublicQueuesNotAllowed(uri);

            IsLocal = IsUriHostLocal(uri);

            ActualUri = IsLocal ? SetUriHostToLocalMachineName(uri) : uri;

            FormatName = BuildQueueFormatName(uri);

            if (IsLocal)
                LocalName = @".\private$\" + uri.AbsolutePath.Substring(1);
        }

        public QueueAddress(string address)
            : this(new Uri(address))
        {
        }

        /// <summary>
        /// The local machine name used when publishing local queues
        /// </summary>
        public static string LocalMachineName { get; private set; }

        /// <summary>
        /// True if the queue is local to this machine
        /// </summary>
        public bool IsLocal { get; private set; }

        /// <summary>
        /// The Uri specified by the caller, unmodified
        /// </summary>
        public Uri ActualUri { get; private set; }

        /// <summary>
        /// The format name used to talk to MSMQ
        /// </summary>
        public string FormatName { get; private set; }

        /// <summary>
        /// The name of the queue in local format (.\private$\name)
        /// </summary>
        public string LocalName { get; private set; }

        private static string BuildQueueFormatName(Uri uri)
        {
            string hostName = uri.Host;
            return string.Format(@"FormatName:DIRECT=OS:{0}\private$\{1}", hostName, uri.AbsolutePath.Substring(1));
        }

        private static Uri SetUriHostToLocalMachineName(Uri uri)
        {
            UriBuilder builder = new UriBuilder(uri.Scheme, LocalMachineName, uri.Port, uri.PathAndQuery);

            return builder.Uri;
        }

        private static bool IsUriHostLocal(Uri uri)
        {
            string hostName = uri.Host;
            return string.Compare(hostName, ".") == 0 ||
                string.Compare(hostName, _localhost, true) == 0 ||
                    string.Compare(uri.Host, LocalMachineName, true) == 0;
        }

        private static void PublicQueuesNotAllowed(Uri uri)
        {
            if (!uri.AbsolutePath.Substring(1).Contains("/"))
                return;

            if (uri.AbsolutePath.Substring(1).ToLowerInvariant().Contains("public"))
                throw new NotSupportedException(string.Format("Public queues are not supported (please submit a patch): {0}", uri));

            //throw new UriParseException(
            //    "MSMQ endpoints do not allow child folders unless it is 'public' (not supported yet, please submit patch). " +
            //        "Good: 'msmq://machinename/queue_name' or 'msmq://machinename/public/queue_name' - " +
            //            "Bad: msmq://machinename/round_file/queue_name");
        }
    }

    public static class MsmqExtensions
    {
        /// <summary>
        /// Gets the count.
        /// http://blog.codebeside.org/archive/2008/08/27/counting-the-number-of-messages-in-a-message-queue-in.aspx
        /// </summary>
        /// <param name="queue">The self.</param>
        /// <returns></returns>
        public static int GetCount(this MessageQueue queue)
        {
            //if (!MessageQueue.Exists(queue.MachineName + @"\" + queue.QueueName))
            //{
            //    return 0;
            //}

            var props = new NativeMethods.MQMGMTPROPS { cProp = 1 };
            try
            {
                props.aPropID = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(props.aPropID, NativeMethods.PROPID_MGMT_QUEUE_MESSAGE_COUNT);

                props.aPropVar = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.MQPROPVariant)));
                Marshal.StructureToPtr(new NativeMethods.MQPROPVariant { vt = NativeMethods.VT_NULL }, props.aPropVar, false);

                props.status = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(props.status, 0);

                int result = NativeMethods.MQMgmtGetInfo(null, "queue=" + queue.FormatName, ref props);
                if (result != 0) throw new Win32Exception(result);

                if (Marshal.ReadInt32(props.status) != 0)
                {
                    return 0;
                }

                var propVar = (NativeMethods.MQPROPVariant)Marshal.PtrToStructure(props.aPropVar, typeof(NativeMethods.MQPROPVariant));
                if (propVar.vt != NativeMethods.VT_UI4)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(propVar.ulVal);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(props.aPropID);
                Marshal.FreeHGlobal(props.aPropVar);
                Marshal.FreeHGlobal(props.status);
            }
        }

    }
}