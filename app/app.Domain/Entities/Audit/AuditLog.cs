using System;

namespace app.Domain.Entities.Audit
{
    public class AuditLog
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Table { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
    }
}
