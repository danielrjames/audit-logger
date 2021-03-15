using app.Domain.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Data.Repositories
{
    public interface IAuditRepository : IDisposable
    {
        Task<IEnumerable<AuditLog>> GetAuditLogs();
    }
}
