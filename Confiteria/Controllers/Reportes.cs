using ArquitecturaModel;
using ArquitecturaModel.Model;
using ArquitecturaModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Confiteria.Controllers
{
    [Authorize]
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
        public IActionResult ReporteGanancia(ReporteVentasMensualesViewModel viewModel)
        {
            var desde = new DateTime(viewModel.Desde.Year, viewModel.Desde.Month, viewModel.Desde.Day, 0, 0, 0);
            var hasta = new DateTime(viewModel.Hasta.Year, viewModel.Hasta.Month, viewModel.Hasta.Day, 23, 59, 59);
            var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            List<RptGananciaViewModel> rpt = new List<RptGananciaViewModel>();
            List<DetalleFacturas> detalle = new List<DetalleFacturas>();
            if(User.IsInRole("Admin") || User.IsInRole("sa"))
            {
                detalle = _context.DetalleFacturas
                .Include(i => i.Productos)
                .Include(i => i.Facturacion)
                .Include(i => i.Sucursales)
                .AsNoTracking()
                .Where(f => (f.FechaRegistro >= desde && f.FechaRegistro <= hasta) )
                .ToList();
            }
            else
            {
                detalle = _context.DetalleFacturas
                .Include(i => i.Productos)
                .Include(i => i.Facturacion)
                .Include(i => i.Sucursales)
                .AsNoTracking()
                .Where(f => (f.FechaRegistro >= desde && f.FechaRegistro <= hasta) && f.SucursalesId == UsuarioId!.SucursalesId)
                .ToList();
            }
                var productIds = detalle.Select(i => new { Id = i.Productos.Id }).Distinct().ToList();
            foreach (var id in productIds)
            {
                var sucursales = detalle.Select(s=> s.Sucursales).DistinctBy(d=> d.SucursalId).ToList();
                foreach (var item in sucursales.Distinct().ToList())
                {
                    var ganancia = new RptGananciaViewModel();
                    ganancia.Id = id.Id;
                    var d = detalle.Where(w => w.ProductosId == id.Id && w.SucursalesId == item.SucursalId).ToList();
                    var product = d.FirstOrDefault()!.Productos;
                    decimal total = 0;
                    foreach (var f in d)
                    {
                        var tasa = _context.TasaDolar.Find(f.Facturacion.TasaDolarId);
                        total += f.SubTotal / tasa.Tasa;
                    }
                    ganancia.NombreProducto = product.Descripcion;
                    ganancia.Cantidad = d.Sum(s => s.Cantidad);
                    ganancia.Total = total;
                    ganancia.Ganancia = total - (product.PrecioCosto * ganancia.Cantidad);
                    ganancia.Sucursales = item;
                    rpt.Add(ganancia);
                }
            }
            return new ViewAsPdf(nameof(RptReporteGanancia), rpt)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 5, 10, 5)
            };
        }
        public IActionResult InventarioTotal()
        {
            var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            //var totalInventario = _context.Productos.Include(i=> i.Inventario)
            //    .Sum(p => p.Precio * (p.Inventario.Any(f => f.ProductosId == p.Id && f.SucursalesId == UsuarioId!.SucursalesId)? p.Inventario.FirstOrDefault(f=> f.ProductosId == p.Id && f.SucursalesId == UsuarioId!.SucursalesId)!.Stock : 0));

            var inventario = _context.Inventario.Include(i => i.Productos).Where(i => i.SucursalesId == UsuarioId!.SucursalesId).ToList();
            var totalCosto = inventario.Sum(p => p.Productos.PrecioCosto * p.Stock);
            return View(totalCosto);
        }
        public IActionResult RptReporteGanancia()
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
			List<ArquitecturaModel.Model.Facturacion> consulta = new List<ArquitecturaModel.Model.Facturacion>();
            var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity!.Name);
           if(User.IsInRole("Admin") || User.IsInRole("sa"))
            {
                consulta = _context.Facturacion
               .Include(d => d.DetalleFacturas)
               //.Include("DetalleFacturas.Productos")
               .Include(i => i.FormaPago)
               .Include(i => i.Sucursales)
               .Include(i => i.Clientes)
               .Where(f => (f.FechaRegistro >= d && f.FechaRegistro <= h))
               .OrderBy(o => o.FormaPagoId)
               .ToList();
            }
           else
            {
                consulta = _context.Facturacion
               .Include(d => d.DetalleFacturas)
               //.Include("DetalleFacturas.Productos")
               .Include(i => i.FormaPago)
               .Include(i => i.Sucursales)
               .Include(i => i.Clientes)
               .Where(f => (f.FechaRegistro >= d && f.FechaRegistro <= h) && f.SucursalesId == UsuarioId!.SucursalesId)
               .OrderBy(o => o.FormaPagoId)
               .ToList();
            }
            if (consulta.Count != 0)
                facturacions = consulta;
           // Agrupar por Forma de Pago y calcular el total
               var reportePorFormaPago = consulta
                   .GroupBy(f => f.FormaPago.Name) // Agrupa por el nombre de la forma de pago
                   .Select(g => new
                   {
                       FormaPago = g.Key,
                       Total = g.Sum(x => x.Total) 
                   })
                   .ToList();

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
