using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BackupRestore.Models;
using static System.Console;
using System.IO;
using System;

namespace BackupRestore.Services
{
    public interface IBackupRestoreService
    {
        void Run();
    }

    public class BackupRestoreService : IBackupRestoreService
    {
        private readonly ILogger<BackupRestoreService> _logger;
        private readonly KhataContext _context;
        private readonly AppSettings _config;

        public BackupRestoreService(ILogger<BackupRestoreService> logger,
             KhataContext context,
            IOptions<AppSettings> config)
        {
            _context = context;
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            _logger.LogWarning($"Wow! We are now in the test service of: {_config.ConsoleTitle}");

            WriteLine("MENU");
            WriteLine("====");
            WriteLine("1. Backup");
            //WriteLine("2. Restore");
            while (true)
            {
                Write("Enter your choice: ");
                if (int.TryParse(ReadLine(), out int choice))
                {
                    var consoleTextColor = ForegroundColor;
                    switch (choice)
                    {
                        case 1:
                            Write("Enter Backup Path: ");
                            var path = ReadLine();
                            if (path == "")
                            {
                                path = @"D:\Khata\Backups\";
                                Directory.CreateDirectory(path);
                            }
                            if (Directory.Exists(path))
                            {
                                path += $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak";
                                _context.CreateBackup(path);
                                if (File.Exists(path))
                                {
                                    ForegroundColor = ConsoleColor.Green;
                                    WriteLine($"Backup Created Successfully At: {path}");
                                    ForegroundColor = consoleTextColor;
                                }
                                else
                                {
                                    ForegroundColor = ConsoleColor.Red;
                                    WriteLine($"Could not Create Database");
                                    ForegroundColor = consoleTextColor;
                                }
                            }
                            else
                            {
                                ForegroundColor = ConsoleColor.Red;
                                WriteLine("Invalid Directory");
                                ForegroundColor = consoleTextColor;
                            }
                            break;
                        //case 2:
                        //    Write("Enter Backup File Path: ");
                        //    path = ReadLine();
                        //    if (File.Exists(path))
                        //    {
                        //        _context.RestoreBackup(path);
                        //        ForegroundColor = ConsoleColor.Green;
                        //        WriteLine($"Query Executed. Check Database");
                        //        ForegroundColor = consoleTextColor;
                        //    }
                        //    else
                        //    {
                        //        ForegroundColor = ConsoleColor.Red;
                        //        WriteLine("Invalid File Path");
                        //        ForegroundColor = consoleTextColor;
                        //    }
                        //        break;
                        case 0: return;
                        default:
                            WriteLine("Invalid Option");
                            break;
                    }
                }
                else
                    WriteLine("Invalid input");
            }
        }
    }
}
