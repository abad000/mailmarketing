﻿using System;

namespace MailBC.DataStore
{
    public class DbContextInitializer
    {
        private static readonly object SyncLock = new object();
        private static DbContextInitializer _instance;
        private bool _isInitialized = false;

        protected DbContextInitializer() { }

        public static DbContextInitializer Instance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DbContextInitializer();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// This is the method which should be given the call to intialize the DbContext; e.g.,
        /// DbContextInitializer.Instance().InitializeDbContextOnce(() => InitializeDbContext());
        /// where InitializeDbContext() is a method which calls DbContextManager.Init()
        /// </summary>
        /// <param name="initMethod"></param>
        public void InitializeDbContextOnce(Action initMethod)
        {
            lock (SyncLock)
            {
                if (!_isInitialized)
                {
                    initMethod();
                    _isInitialized = true;
                }
            }
        }
    }
}