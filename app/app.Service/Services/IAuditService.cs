using app.Domain.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.Service.Services
{
    public interface IAuditService : IDisposable
    {
        Task<IEnumerable<AuditLog>> GetAllLogs();
    }
}
