using ArquitecturaModel;
using ArquitecturaModel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Confiteria.Controllers
{
    public class Reportes : Controller
    {
        private readonly AplicationDbContext _context;

        public Reportes(AplicationDbContext context)
        {
            _context = context;
        }
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
        public IActionResult ReporteGanancia()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReporteGanancia(int id)
        {
            return View();
        }
        public IActionResult ReporteVentasMensuales()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReporteVentasMensuales(ReporteVentasMensualesViewModel viewModel)
        {
            var d = new DateTime(viewModel.Desde.Year, viewModel.Desde.Month, viewModel.Desde.Day, 0, 0, 0);
            var h = new DateTime(viewModel.Hasta.Year, viewModel.Hasta.Month, viewModel.Hasta.Day, 23, 59, 59);
            List<ArquitecturaModel.Model.Facturacion> facturacions = new List<ArquitecturaModel.Model.Facturacion>();
            var consulta = _context.Facturacion.Include(d => d.DetalleFacturas)
                .Include("DetalleFacturas.Productos")
                .Include(i => i.FormaPago)
                .Include(i => i.Clientes).Where(f => f.FechaRegistro >= d && f.FechaRegistro <= h).OrderBy(o=> o.FormaPagoId).ToList();
            if(consulta.Count != 0)
                facturacions = consulta;
            return new ViewAsPdf(nameof(RptVentasMensuales), facturacions)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 5, 10, 5)
            };
        }
        public IActionResult RptVentasMensuales()
        {
            return View();
        }
    }
}
