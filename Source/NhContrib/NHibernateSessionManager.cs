using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate;
using NHibernate.Cache;
using System.Data;
using Nuaguil.Utils.DesignByContract;

namespace Nuaguil.NhContrib
{
    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public sealed class NHibernateSessionManager
    {
        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static NHibernateSessionManager Instance {
            get {
                return Nested.NHibernateSessionManager;
            }
        }

        /// <summary>
        /// Private constructor to enforce singleton
        /// </summary>
        private NHibernateSessionManager() { }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly NHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManager();
        }

        #endregion

        public ISessionFactory GetSessionFactoryFor()
        {
           return GetSessionFactoryFor(Utils.GetDefaultDbCfg());
        }
 
       /// <summary>
        /// This method attempts to find a session factory stored in <see cref="_sessionFactories" />
        /// via its name; if it can't be found it creates a new one and adds it the hashtable.
        /// </summary>
        /// <param name="sessionFactoryConfigPath">Path location of the factory config</param>
        public ISessionFactory GetSessionFactoryFor(string sessionFactoryConfigPath) {
            Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            //  Attempt to retrieve a stored SessionFactory from the hashtable.
            ISessionFactory sessionFactory = (ISessionFactory) _sessionFactories[sessionFactoryConfigPath];

            //  Failed to find a matching SessionFactory so make a new one.
            if (sessionFactory == null) {
                Check.Require(File.Exists(sessionFactoryConfigPath),
                    "The config file at '" + sessionFactoryConfigPath + "' could not be found");

                NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
                cfg.Configure(sessionFactoryConfigPath);

                //  Now that we have our Configuration object, create a new SessionFactory
                sessionFactory = cfg.BuildSessionFactory();

                if (sessionFactory == null) {
                    throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
                }

                _sessionFactories.Add(sessionFactoryConfigPath, sessionFactory);
            }

            return sessionFactory;
        }

        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptorOn(string sessionFactoryConfigPath, IInterceptor interceptor) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen) {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSessionFrom(sessionFactoryConfigPath, interceptor);
        }

        public ISession GetSessionFrom(string sessionFactoryConfigPath) {
            return GetSessionFrom(sessionFactoryConfigPath, null);
        }

        public ISession GetSessionFrom()
        {
            return GetSessionFrom(Utils.GetDefaultDbCfg(), null);
        }


        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSessionFrom(string sessionFactoryConfigPath, IInterceptor interceptor) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session == null) {
                if (interceptor != null) {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession(interceptor);
                }
                else {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
                }

                ContextSessions[sessionFactoryConfigPath] = session;
            }

            Check.Ensure(session != null, "session was null");

            return session;
        }

        /// <summary>
        /// Gets a StatelessSession 
        /// it gets invoked from other public methods.
        /// </summary>
        public IStatelessSession GetStatelessSessionFrom(string sessionFactoryConfigPath)
        {
            IStatelessSession session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenStatelessSession();
            Check.Ensure(session != null, "session was null");
            return session;
        }

        public IStatelessSession GetStatelessSessionFrom()
        {
            return GetStatelessSessionFrom(Utils.GetDefaultDbCfg());
        }

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        public void CloseSessionOn(string sessionFactoryConfigPath) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen) {
                //session.Flush();
                session.Close();
                session.Dispose();
            }

            ContextSessions.Remove(sessionFactoryConfigPath);
        }

        public void CloseSessionOn()
        {
            CloseSessionOn(Utils.GetDefaultDbCfg());
        }

        public ITransaction BeginTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            if (transaction == null) {
                transaction = GetSessionFrom(sessionFactoryConfigPath).BeginTransaction();
                ContextTransactions.Add(sessionFactoryConfigPath, transaction);
            }

            return transaction;
        }

        /// <summary>
        /// Iniciar una transacción en la base de datos definida por omisión
        /// </summary>
        /// <returns></returns>
        public ITransaction BeginTransactionOn()
        {
            return BeginTransactionOn(Utils.GetDefaultDbCfg());
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return GetSessionFrom(Utils.GetDefaultDbCfg()).BeginTransaction(isolationLevel);
        }
        
        public ITransaction BeginTransaction(IsolationLevel isolationLevel,string sessionFactoryConfigPath)
        {
            return GetSessionFrom(sessionFactoryConfigPath).BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Método para iniciar  una transaccion en la base de datos
        /// definida en el archivo de configuración sessionFactoryConfigPath
        /// </summary>
        /// <param name="sessionFactoryConfigPath">
        /// Nombre del archivo de configuración de la sesión
        /// </param>
        /// <returns></returns>
        public ITransaction BeginTransaction(string sessionFactoryConfigPath)
        {
            return GetSessionFrom(sessionFactoryConfigPath).BeginTransaction();
        }

        /// <summary>
        /// Método para iniciar  una transaccion en la base de datos
        /// definida en el factory por omisión
        /// </summary>
        /// <returns></returns>
        public ITransaction BeginTransaction()
        {
            return GetSessionFrom(Utils.GetDefaultDbCfg()).BeginTransaction();
        }


        public void CommitTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try {
                if (HasOpenTransactionOn(sessionFactoryConfigPath)) {
                    transaction.Commit();
                    ContextTransactions.Remove(sessionFactoryConfigPath);
                }
            }
            catch (HibernateException exc) {
                RollbackTransactionOn(sessionFactoryConfigPath);
                throw new PersistenceException("Error al hacer el commit",exc);
            }
        }

        /// <summary>
        /// Método para enviar una transacción en la sessión por emision
        /// definida en el app/web.config
        /// </summary>
        public void CommitTransactionOn()
        {
            CommitTransactionOn(Utils.GetDefaultDbCfg());
        }

        public bool HasOpenTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public bool HasOpenTransactionOn()
        {
            return HasOpenTransactionOn(Utils.GetDefaultDbCfg());
        }

        public void RollbackTransactionOn(string sessionFactoryConfigPath) {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try
            {
                if (HasOpenTransactionOn(sessionFactoryConfigPath))
                {
                    transaction.Rollback();
                }

                ContextTransactions.Remove(sessionFactoryConfigPath);
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al hacer el commit", exc);
            }
            finally {
                CloseSessionOn(sessionFactoryConfigPath);
            }
        }

        public void RollbackTransactionOn()
        {
            RollbackTransactionOn(Utils.GetDefaultDbCfg());
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one transaction per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
        private Hashtable ContextTransactions {
            get {
                if (IsInWebContext()) {
                    if (HttpContext.Current.Items[TransactionKey] == null)
                        HttpContext.Current.Items[TransactionKey] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[TransactionKey];
                }
                if (CallContext.GetData(TransactionKey) == null)
                    CallContext.SetData(TransactionKey, new Hashtable());

                return (Hashtable)CallContext.GetData(TransactionKey);
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one session per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
        private Hashtable ContextSessions {
            get {
                if (IsInWebContext()) {
                    if (HttpContext.Current.Items[SessionKey] == null)
                        HttpContext.Current.Items[SessionKey] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[SessionKey];
                }
                if (CallContext.GetData(SessionKey) == null)
                    CallContext.SetData(SessionKey, new Hashtable());

                return (Hashtable)CallContext.GetData(SessionKey);
            }
        }

        private bool IsInWebContext() {
            return HttpContext.Current != null;
        }

        private readonly Hashtable _sessionFactories = new Hashtable();
        private const string TransactionKey = "CONTEXT_TRANSACTIONS";
        private const string SessionKey = "CONTEXT_SESSIONS";
    }
}
