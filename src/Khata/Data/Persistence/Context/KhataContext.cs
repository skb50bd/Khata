using System;
using System.Linq;

using Data.Persistence.DbViews;

using Domain;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace Data.Persistence
{
    public sealed partial class KhataContext : IdentityDbContext
    {
        private readonly ILogger<KhataContext> _logger;
        private readonly KhataSettings _settings;
        public KhataContext(
            DbContextOptions<KhataContext> options,
            ILogger<KhataContext> logger, 
            IOptionsMonitor<KhataSettings> settings) : base(options)
        {
            _logger = logger;
            _settings = settings.CurrentValue; 
            var pm = Database.GetPendingMigrations();

            try
            {
                if (!pm.Any()) return;

                if (_settings.DbProvider == DbProvider.SQLServer)
                {
                    Database.CreateAllSQLServerViews();
                }

                Database.Migrate();
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "Could not apply Migrations"
                  + JsonConvert.SerializeObject(pm));
                _logger.LogError(e.Message);
            }

            Database.EnsureCreated();
        }

        //public KhataContext(
        //    DbContextOptions<KhataContext> options)
        //    : base(options) {}

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.BuildEntities()
                   .BuildQueries(this);
        }
    }
}