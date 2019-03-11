using Microsoft.EntityFrameworkCore;
using System;

public class KhataContext : DbContext
{
    public KhataContext(DbContextOptions<KhataContext> opts)
        : base(opts) { }

    public void CreateBackup(string path)
    {
        var sql = $"BACKUP DATABASE {Database.GetDbConnection().Database} TO DISK = '{path}';";
        
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
        _ = Database.ExecuteSqlCommand(sql);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
    }

    public void RestoreBackup(string path)
    {
        var sql =$"RESTORE FILELISTONLY {Database.GetDbConnection().Database} FROM DISK = '{path}';";

#pragma warning disable EF1000 // Possible SQL injection vulnerability.
        _ = Database.ExecuteSqlCommand(sql);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
    }
}