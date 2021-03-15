using System.Collections.Generic;

namespace app.Web.ViewModels
{
    public class AuditLogListVM
    {
        public List<AuditLogVM> Logs { get; set; }

        public AuditLogListVM()
        {
            Logs = new List<AuditLogVM>();
        }
    }
}
