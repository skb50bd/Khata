using System;
using System.IO;

using BackupRestore.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using static System.Console;

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
            WriteLine("1. Backup to bak");
            WriteLine("2. Backup to json");
            while (true)
            {
                Write("Enter your choice: ");
                if (int.TryParse(ReadLine(), out int choice))
                {
                    var consoleTextColor = ForegroundColor;
                    string path;
                    switch (choice)
                    {
                        case 1:
                            Write("Enter Backup Path: ");
                            path = ReadLine();
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
                        case 2:
                            Write("Enter Backup Path: ");
                            path = ReadLine();
                            if (path == "")
                            {
                                path = @"D:\Khata\JsonBackups\";
                                Directory.CreateDirectory(path);
                            }
                            if (Directory.Exists(path))
                            {
                                path += $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.json";
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
