using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Zhaoxi.HostComputer.Base;
using Zhaoxi.HostComputer.Views;

namespace Zhaoxi.HostComputer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalMonitor.Start();

            if (new LoginWindow().ShowDialog() == true)
            {
                new MainWindow().ShowDialog();
            }
            Application.Current.Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalMonitor.Stop();
            base.OnExit(e);
        }
    }
}
