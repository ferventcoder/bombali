namespace bombali.infrastructure.app.monitorchecks
{
    using System;
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using System.Text;
    using logging;

    public class ServerCheck : ICheck
    {
        IList<IPStatus> okay_responses;
        double failure_count = 0d;

        public ServerCheck()
        {
            build_list_of_okay_responses();
        }

        public void build_list_of_okay_responses()
        {
            okay_responses = new List<IPStatus>
                                 {
                                     IPStatus.Success
                                 };
        }

        public string last_response { get; private set; }

        public bool run_check(string what_to_check)
        {
            bool successful_check = true;

            IPStatus response_code = IPStatus.Unknown;

            //todo:make the number of loops configurable
            for (int i = 1; i <= 4; i++)
            {
                response_code = ping_server(what_to_check);
                if (okay_responses.Contains(response_code))
                {
                    break;
                }
            }

            last_response = response_code.ToString();

            if (okay_responses.Contains(response_code))
            {
                failure_count = 0;
                Log.bound_to(this).Info("{0} was able to successfully reach {1}. Response code was {2}.", ApplicationParameters.name,
                                        what_to_check, response_code);
            }
            else
            {
                successful_check = false;
                failure_count += 1;
                Log.bound_to(this).Warn(
                    "{0} warning! {1} is unreachable. Response code was {2}. This has happened {3} times.",
                    ApplicationParameters.name, what_to_check, response_code, failure_count);
            }

            return successful_check;
        }

        IPStatus ping_server(string server)
        {
            Log.bound_to(this).Debug("{0} is checking {1}.", ApplicationParameters.name, server);

            const int time_to_live = 3000;
            const bool dont_fragment = false;
            int timeout = (int)TimeSpan.FromSeconds(18).TotalMilliseconds;
            byte[] buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            PingOptions options = new PingOptions(time_to_live, dont_fragment);

            Ping ping = new Ping();

            try
            {
                PingReply response = ping.Send(server, timeout, buffer, options);
                return response.Status;
            }
            catch (Exception)
            {
                return IPStatus.DestinationUnreachable;
            }
            finally
            {
                ping.Dispose();
            }
        }
    }
}