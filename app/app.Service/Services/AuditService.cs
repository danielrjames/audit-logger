using app.Data.Repositories;
using app.Domain.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Service.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _repo;

        /// <summary>
        /// audit service constructor
        /// </summary>
        /// <param name="repo">injected audit repo</param>
        public AuditService(IAuditRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// calls audit repo to get all audit logs
        /// </summary>
        /// <returns>list of logs</returns>
        public async Task<IEnumerable<AuditLog>> GetAllLogs()
        {
            return await _repo.GetAuditLogs();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _repo.Dispose();
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
