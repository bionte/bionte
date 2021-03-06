﻿using System;
using System.Windows;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace YAPOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            base.OnStartup(e);

            #if (DEBUG)
                RunInDebugMode();
            #else
                RunInReleaseMode();
            #endif

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            
        }

        private static void RunInDebugMode()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private static void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            try
            {
                Bootstrapper bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            ExceptionPolicy.HandleException(ex, "Default Policy");
            MessageBox.Show(YAPOS.Properties.Resources.UnhandledException);
            Environment.Exit(1);
        }
    }
}
