namespace bombali.infrastructure.data.accessors
{
    using System.Reflection;
    using Castle.Core.Interceptor;
    using NHibernate;
    using IInterceptor = Castle.Core.Interceptor.IInterceptor;

    public class TransactionInterceptor : IInterceptor
    {
        private readonly ISessionFactory session_factory;

        public TransactionInterceptor(ISessionFactory session_factory)
        {
            this.session_factory = session_factory;
        }

        private static bool is_transaction(MethodInfo methodInfo)
        {
            object[] attributes = methodInfo.GetCustomAttributes(false);
            foreach (object attribute in attributes)
            {
                if (attribute is TransactionAttribute)
                {
                    return true;
                }
            }

            return false;
        }

        public void Intercept(IInvocation invocation)
        {
            if (is_transaction(invocation.Method))
            {
                using (ISession session = session_factory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        invocation.Proceed();

                        transaction.Commit();
                    }
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}