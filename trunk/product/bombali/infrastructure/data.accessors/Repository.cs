namespace bombali.infrastructure.data.accessors
{
    using System;
    using System.Collections.Generic;
    using logging;
    using NHibernate;
    using NHibernate.Criterion;
    using NHibernate.Transform;

    public sealed class Repository : IRepository
    {

        public Repository(ISessionFactory session_factory)
        {
            this.session_factory = session_factory;
            if (session_factory == null)
            {
                throw new ApplicationException("Repository cannot do any with a null session factory. Please provide a session factory.");
            }
        }

        [Lock]
        public IList<T> get_all<T>()
        {
            IList<T> list;
            Type persistentClass = typeof(T);

            using (ISession session = session_factory.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(persistentClass);
                list = criteria.List<T>();
                session.Close();
            }

            Log.bound_to(this).Info("Repository found {0} records of type {1}.", list.Count, typeof(T).Name);

            return list;
        }

        [Lock]
        public IList<T> get_with_criteria<T>(DetachedCriteria detachedCriteria)
        {
            if (detachedCriteria == null)
            {
                Log.bound_to(this).Warn(
                    "Please ensure you send in a criteria when you want to limit records. Otherwise please consider using GetAll(). Returning empty list.");
                return null;
            }

            IList<T> list;
            using (ISession session = session_factory.OpenSession())
            {
                ICriteria criteria = detachedCriteria.GetExecutableCriteria(session);
                list = criteria.List<T>();
                session.Close();
            }

            Log.bound_to(this).Info("Repository found {0} records of type {1} with criteria {2}.", list.Count, typeof(T).Name, detachedCriteria.ToString());

            return list;
        }

        [Lock]
        public IList<T> get_transformation_with_criteria<T>(DetachedCriteria detachedCriteria)
        {
            if (detachedCriteria == null)
            {
                Log.bound_to(this).Warn(
                    "Please ensure you send in a criteria when you want to get transformed records. Otherwise please consider using GetAll(). Returning empty list.");
                return null;
            }

            IList<T> list;
            using (ISession session = session_factory.OpenSession())
            {
                ICriteria criteria = detachedCriteria.GetExecutableCriteria(session);
                list = criteria
                    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(T)))
                    .List<T>();
                session.Close();
            }

            Log.bound_to(this).Info("Repository found {0} records of type {1} with criteria {2}.", list.Count, typeof(T).Name, detachedCriteria.ToString());

            return list;
        }

        [Lock]
        public void save_or_update<T>(IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                Log.bound_to(this).Warn(
                    "Please ensure you send a non null list of records to save.");
                return;
            }
            Log.bound_to(this).Info("Received {0} records of type {1} marked for save/update.", list.Count, typeof(T).Name);
            using (ISession session = session_factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (T item in list)
                    {
                        session.SaveOrUpdate(item);
                        //session.Flush();
                    }
                    transaction.Commit();
                }
                session.Close();
            }

            Log.bound_to(this).Info("Saved {0} records of type {1} successfully.", list.Count, typeof(T).Name);
        }

        [Lock]
        public void save_or_update<T>(T item)
        {
            if (item == null)
            {
                Log.bound_to(this).Warn(
                    "Please ensure you send a non null record to save.");
                return;
            }

            using (ISession session = session_factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(item);
                    //session.Flush();
                    transaction.Commit();
                }
                session.Close();
            }

            Log.bound_to(this).Info("Saved item of type {0} successfully.", typeof(T).Name);
        }

        [Lock]
        public void delete<T>(IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                Log.bound_to(this).Warn(
                    "Please ensure you send a non null list of records to delete.");
                return;
            }

            Log.bound_to(this).Info("Received {0} records of type {1} marked for deletion.", list.Count, typeof(T).Name);
            using (ISession session = session_factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (T item in list)
                    {
                        session.Delete(item);
                        //session.Flush();
                    }
                    transaction.Commit();
                }
                session.Close();
            }

            Log.bound_to(this).Info("Removed {0} records of type {1} successfully.", list.Count, typeof(T).Name);
        }

        public ISessionFactory session_factory { get; private set; }

        //public string connection_string
        //{
        //    get { return session_factory. }
        //}
    }
}