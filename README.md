# DBBackupService
1. Introduction
The Database Backup Windows Service is a Windows-based background application developed using C# and the .NET Framework. Its main purpose is to automatically create full backups of a specified SQL Server database at configurable time intervals.
The service runs in the background without requiring continuous user interaction. It connects to SQL Server using a configurable connection string, performs a full database backup, and saves the resulting .bak file in a configured backup directory.
The application also provides logging for service activities, successful backups, and errors. A console mode is included for development and debugging so the service logic can be tested without installing it as a Windows Service.


2. Build Instructions

1.Open the solution in Visual Studio.

2.Update App.config with the correct connection string and paths.

3.Select Release configuration.

4.Select Build → Build Solution.

5.Verify that the compiled files are available in bin\Release.
bin\Release

4. Deployment Instructions


Open Developer Command Prompt for Visual Studio as Administrator and navigate to the Release directory.

cd "C:\Path\To\Project\bin\Release"

InstallUtil.exe DatabaseBackupServiceProject.exe


After successful installation, the service should appear in Windows Services as DatabaseBackupService.

6. Starting and Stopping the Service

   
The service can be controlled through services.msc or from an elevated Command Prompt.
Start:

sc start DatabaseBackupService

Stop:

sc stop DatabaseBackupService


The Windows Services console can be opened with:
services.msc

8. Uninstalling the Service


To remove the service from Windows:

InstallUtil.exe /u DatabaseBackupServiceProject.exe


10. Test Log

    
The following is an example format. Replace it with the actual log generated during testing.

[2026-09-09 14:00:00] Service Started.

[2026-09-09 14:10:00] Database backup successful: C:\DatabaseBackups\Backup_20260909_141000.bak

[2026-09-09 15:10:00] Database backup successful: C:\DatabaseBackups\Backup_20260909_151000.bak

[2026-09-09 16:10:00] Error during backup: Network-related or instance-specific error occurred while establishing a connection to SQL Server.

[2026-09-09 17:10:00] Database backup successful: C:\DatabaseBackups\Backup_20260909_171000.bak

[2026-09-09 18:00:00] Service Stopped.





