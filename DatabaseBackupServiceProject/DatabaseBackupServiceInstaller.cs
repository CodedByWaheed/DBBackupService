using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace DatabaseBackupServiceProject
{
    [RunInstaller(true)]
    public partial class DatabaseBackupServiceInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;
        public DatabaseBackupServiceInstaller()
        {
            // Initialize ServiceProcessInstaller
            processInstaller = new ServiceProcessInstaller
            {
                // Run the service under the local system account
                Account = ServiceAccount.LocalSystem
            };



            // Initialize ServiceInstaller
            serviceInstaller = new ServiceInstaller
            {
                // Set the name of the service
                ServiceName = "DBBackupService",
                DisplayName = "Database Backup Service",
                Description = "A Windows Service that create backup file for Database every spcific time.",
                ServicesDependedOn = new string[] {
                    "MSSQLSERVER",
                    "RpcSs",
                    "EventLog"

                }, // Specify that the service depends on the SQL Server service}
                StartType = ServiceStartMode.Automatic // Automatically starts the service on system boot
            };

            // Add both installers to the Installers collection
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
