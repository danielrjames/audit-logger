using app.Data.Contexts;
using app.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Data.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Audit repo constructor
        /// </summary>
        /// <param name="context">dbcontext injection</param>
        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve audit logs from dbcontext
        /// </summary>
        /// <returns>list of audit logs</returns>
        public async Task<IEnumerable<AuditLog>> GetAuditLogs()
        {
            return await _context.AuditLogs.ToListAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
