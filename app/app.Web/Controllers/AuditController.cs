using app.Service.Services;
using app.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace app.Web.Controllers
{
    [Authorize]
    [Route("audit")]
    public class AuditController : Controller
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        /// <summary>
        /// Audit index page to view all logs
        /// </summary>
        /// <returns>view with list of logs (if they exist)</returns>
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var model = new AuditLogListVM();

            var logs = await _auditService.GetAllLogs();

            if (logs.Any())
            {
                model.Logs = logs.Select(l => new AuditLogVM(l)).ToList();
            }

            return View(model);
        }
    }
}
