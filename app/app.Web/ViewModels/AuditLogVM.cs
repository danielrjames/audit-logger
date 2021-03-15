using app.Domain.Entities.Audit;

namespace app.Web.ViewModels
{
    public class AuditLogVM
    {
        public string User { get; set; }
        public string Action { get; set; }
        public string Table { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Date { get; set; }

        public AuditLogVM(AuditLog log)
        {
            User = log.UserName;
            Action = log.Action;
            Table = log.Table;
            OldValue = log.OldValue;
            NewValue = log.NewValue;
            Date = log.Date.ToLongDateString();
        }
    }
}
