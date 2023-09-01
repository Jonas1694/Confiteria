using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Confiteria.Controllers
{
    public class Reportes : Controller
    {
        public static int Id { get; set; }
        public IActionResult Index(int id)
        {
            Id = id;
			return View();
        }
        public IActionResult GetReport()
        {
            var report = new StiReport();
            report["Id"] = Id;
            report.Load(StiNetCoreHelper.MapPath(this, "Reports/Factura.mrt"));
            return StiNetCoreViewer.GetReportResult(this, report);
        }
        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
    }
}
