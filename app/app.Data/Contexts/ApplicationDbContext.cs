using app.Domain.Entities.Audit;
using app.Domain.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace app.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().ToTable("Product");
            builder.Entity<AuditLog>().ToTable("AuditLog");
        }

        /// <summary>
        /// For the audit logger, we are overriding the SaveChangesAsync method with our audit logging code and then passing through
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // getting a list of changedEntities that have a state of add, modified, or delete
            var changedEntries = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted).ToList();

            if (changedEntries.Count > 0)
            {
                foreach (var entry in changedEntries)
                {
                    SaveLog(entry); // saving to db
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// creating and saving the log to the dbcontext
        /// </summary>
        /// <param name="entry">entry item to be logged</param>
        private void SaveLog(EntityEntry entry)
        {
            var auditEntry = new AuditLog
            {
                UserName = _httpContextAccessor?.HttpContext.User.FindFirstValue(ClaimTypes.Name), // grabbing user from httpContextAccessor middleware
                Table = entry.Entity.GetType().Name,
                Date = DateTime.Now
            };

            if (entry.State == EntityState.Added)
            {
                auditEntry.Action = "Created";
                auditEntry.OldValue = "N/A";
                auditEntry.NewValue = GetValues(entry);
            }
            else if (entry.State == EntityState.Modified)
            {
                auditEntry.Action = "Modified";
                auditEntry.OldValue = GetValues(entry, true);
                auditEntry.NewValue = GetValues(entry);
            }
            else // deleted
            {
                auditEntry.Action = "Deleted";
                auditEntry.OldValue = GetValues(entry, true);
                auditEntry.NewValue = "N/A";
            }

            this.AuditLogs.Add(auditEntry);
        }

        /// <summary>
        /// getting changed values in a json array string format
        /// </summary>
        /// <param name="entry">entry being changed</param>
        /// <param name="oldValue">boolean check if oldvalue or not</param>
        /// <returns>json array string</returns>
        private static string GetValues(EntityEntry entry, bool oldValue = false)
        {
            var valueList = new List<string[]>();

            foreach (var property in entry.Properties)
            {
                if (entry.State == EntityState.Modified)
                {
                    var valid = ModifiedChecks(entry, property);

                    if (!valid)
                    {
                        continue;
                    }
                }

                var value = new string[]
                {
                    property.Metadata.Name,
                    oldValue ? property.OriginalValue?.ToString() : property.CurrentValue?.ToString()
                };

                valueList.Add(value);
            }

            var serializedList = JsonConvert.SerializeObject(valueList); //serialize list

            return serializedList;
        }

        /// <summary>
        /// a few checks for modified entries, primairly for identity
        /// </summary>
        private static bool ModifiedChecks(EntityEntry entry, PropertyEntry property)
        {
            if (entry.State == EntityState.Modified && !property.IsModified) // if property isn't modified, skip
            {
                return false;
            }

            if (property.OriginalValue?.ToString() == property.CurrentValue?.ToString()) // if old and new are the same, skip (mainly for indentity)
            {
                return false;
            }

            string[] skippedIdentityFields = { "concurrencystamp", "securitystamp" };

            if (skippedIdentityFields.Contains(property.Metadata.Name.ToLower())) // skkipping concurrency stamp and security stamp in identity (will always be different)
            {
                return false;
            }

            return true;
        }
    }
}
