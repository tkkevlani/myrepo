using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;

namespace BhajanPlay
{
    public partial class Service1 : ServiceBase
    {
        public static Process p = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += play;
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
        }

        protected override void OnStop()
        {
            if (p != null)
                p.Kill();
        }

        internal void play(object sender, DoWorkEventArgs er)
        {

            DateTime morningTime;
            DateTime eveningTime;

            try
            {
                do
                {
                    try
                    {
                        Console.WriteLine("Start Bhajan Play");
                        string morningURL = ConfigurationManager.AppSettings["morningURL"];
                        string eveningURL = ConfigurationManager.AppSettings["eveningURL"];
                        string morningTimeConfig = ConfigurationManager.AppSettings["morningTime"];
                        string eveningTimeConfig = ConfigurationManager.AppSettings["eveningTime"];

                        //***********************************************************************
                        // Calculate the next execution time and sleep until then.
                        //***********************************************************************

                        morningTime = GetExecTime(morningTimeConfig);
                        eveningTime = GetExecTime(eveningTimeConfig);



                        if (DateTime.Now.Subtract(morningTime).Hours >= 0 && DateTime.Now.Subtract(morningTime).Hours < 1)
                        {
                            //var path = @"F:\TVS Laptop\Tanesh\Whatsapp Audio\Aankhy gazal hein aap ki.mp3";

                            //Process.Start("wmplayer.exe",);

                            //WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

                            //wplayer.URL = path; // "My MP3 file.mp3";
                            //wplayer.controls.play();

                            //File.Open(path, FileMode.Open);

                            var browser = new ProcessStartInfo("chrome.exe");
                            //var browser = new ProcessStartInfo("iexplore.exe");
                            browser.Arguments = morningURL; // "https://www.youtube.com/watch?v=ocqgFDfq_2o"; // morning
                            p = Process.Start(browser);

                            Thread.Sleep(3600000); // 1 hour
                            p.Kill();
                            Thread.Sleep(eveningTime - DateTime.Now);
                        }
                        else if (DateTime.Now.Subtract(eveningTime).Hours >= 0 && DateTime.Now.Subtract(eveningTime).Hours < 1)
                        {
                            var browser = new ProcessStartInfo("chrome.exe");
                            //var browser = new ProcessStartInfo("iexplore.exe");
                            browser.Arguments = eveningURL; // "https://www.youtube.com/watch?v=DouvJOVDbRw"; // Evening
                            p = Process.Start(browser);

                            Thread.Sleep(2040000); // 34 minutes
                            p.Kill();
                            Thread.Sleep(morningTime.AddHours(24) - DateTime.Now);
                        }
                        else if (DateTime.Now > morningTime)
                        {
                            Thread.Sleep(eveningTime - DateTime.Now);
                        }
                        else if (DateTime.Now > eveningTime)
                        {
                            Thread.Sleep(morningTime.AddHours(24) - DateTime.Now);
                        }
                    }
                    catch (Exception Ex)
                    {
                    }

                } while (true);
            }
            catch
            {
            }
        }

        internal static DateTime GetExecTime(string textTime)
        {
            string[] execTimeParts = textTime.Split(new char[] { ':' });
            DateTime todayExecTime = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day, int.Parse(execTimeParts[0]), int.Parse(execTimeParts[1]), int.Parse(execTimeParts[2]));
            return todayExecTime;
        }
    }
}
