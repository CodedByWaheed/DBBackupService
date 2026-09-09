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
using System.IO;
using System.Data.SqlClient;
using System.Timers;

namespace DatabaseBackupServiceProject
{
   
    public partial class DatabaseBackupService : ServiceBase
    {
        private string logFolder;
        private string logFilePath;
        private string BackupFolder;
        private int intervalMilisecond;
        private Timer timer;
        public DatabaseBackupService()
        {
            InitializeComponent();

            CanPauseAndContinue = true; 

            CanShutdown = true;

            InitializeService();
        }
        
        private void LogServiceEvent(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n";

            File.AppendAllText(logFilePath, logMessage);

            // Write to console if running interactively
            if (Environment.UserInteractive)
            {
                Console.WriteLine(logMessage);
            }

        }

        private void ExecuteBackupQuery()
        {
            
            string backupFilePath = Path.Combine(BackupFolder, $"BreadApp_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
            string query = $"BACKUP DATABASE [BreadApp]  TO DISK = N'{backupFilePath}' WITH INIT, FORMAT, STATS = 10;";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    LogServiceEvent($"Database backup successful: {backupFilePath}");
                }
                catch (Exception ex)
                {
                    LogServiceEvent(ex.Message);
                }

            }
        }

        private void InitializeService()
        {
            // Read log directory path from App.config
            //The service reads the log directory path from an external configuration file (App.config) for flexibility.
            logFolder = ConfigurationManager.AppSettings["LogFolder"];
            if (logFolder == null)
            {
                LogServiceEvent("LogFolder is not specified in the configuration file.");
            }
            BackupFolder = ConfigurationManager.AppSettings["BackupFolder"];
            if(BackupFolder == null)
            {
                LogServiceEvent("BackupFolder is not specified in the configuration file.");
            }

            string Intervl = ConfigurationManager.AppSettings["BackupIntervalMinutes"]; 
            if(Intervl == null)
            {
                LogServiceEvent("BackupIntervalMinutes is not specified in the configuration file.");
            }

            intervalMilisecond = Convert.ToInt32(Intervl) * 60 * 1000;
            timer = new Timer(intervalMilisecond);
            timer.AutoReset = true;

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }
            if (!Directory.Exists(BackupFolder))
            {
                Directory.CreateDirectory(BackupFolder);
            }

            logFilePath = Path.Combine(logFolder, ConfigurationManager.AppSettings["LogFileName"]);
        }
        protected override void OnStart(string[] args)
        {
            LogServiceEvent("Service Started");
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteBackupQuery();
        }

        protected override void OnStop()
        {
            LogServiceEvent("Service Stopped");
            timer.Stop();
            timer.Dispose();
        }
        protected override void OnPause()
        {
            LogServiceEvent("Service Paused");
            timer.Stop();
        }

        protected override void OnContinue()
        {
            LogServiceEvent("Service Resumed");
            timer.Start();
        }

        protected override void OnShutdown()
        {
            LogServiceEvent("Service Shutdown due to system shutdown");
            timer.Dispose();
        }


        // Simulate service behavior in console mode
        public void StartInConsole()
        {
            OnStart(null); // Trigger OnStart logic
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine(); // Wait for user input to simulate service stopping
            OnStop(); // Trigger OnStop logic
            Console.ReadKey();

        }
    }
}
