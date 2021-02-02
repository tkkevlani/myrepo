using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BhajanPlay
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //new Service1().OnStart();

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new Service1().play;
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
