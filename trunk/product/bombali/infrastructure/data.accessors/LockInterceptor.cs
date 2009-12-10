namespace bombali.infrastructure.data.accessors
{
    using System;
    using System.Reflection;
    using System.Threading;
    using Castle.Core.Interceptor;

    public class LockInterceptor : IInterceptor
    {
        private static object database_lock = new object();
        private const int lock_timeout = 360000;

        private static void acquire_lock()
        {
            Monitor.TryEnter(database_lock, lock_timeout);
        }

        private static void release_lock()
        {
            Monitor.Exit(database_lock);
        }

        private static bool is_lockable(MethodInfo methodInfo)
        {
            object[] attributes = methodInfo.GetCustomAttributes(false);
            foreach (object attribute in attributes)
            {
                if (attribute is LockAttribute)
                {
                    return true;
                }
            }

            return false;
        }

        public void Intercept(IInvocation invocation)
        {
            if (is_lockable(invocation.Method))
            {
                acquire_lock();
                try
                {
                    invocation.Proceed();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    release_lock();
                }
            }
            else
            {
                invocation.Proceed();
            }
        }

    }
}