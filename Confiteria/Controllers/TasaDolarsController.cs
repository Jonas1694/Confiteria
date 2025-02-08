using ArquitecturaModel;
using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confiteria.Controllers
{
    [Authorize(Roles ="Admin")]
	public class TasaDolarsController : Controller
	{
		private readonly AplicationDbContext _context;

		public TasaDolarsController(AplicationDbContext context)
        {
			_context = context;
		}
        public IActionResult Index()
		{
            var tazaId = _context.TasaDolar.Max(m => m.Id);
            var data = _context.TasaDolar.Find(tazaId);
            return View(data);
		}
		[HttpPost]
		public IActionResult Index(TasaDolar tasaDolar)
		{
			if (tasaDolar == null)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				tasaDolar.Id = 0;
				//tasaDolar.Tasa = tasaDolar.Tasa;
                _context.Add(tasaDolar);
                _context.SaveChanges();
            }
            return View(tasaDolar);
		}
        [HttpPost]
        public IActionResult UpdatePrecio()
		{
			try
			{
                var tazaId = _context.TasaDolar.Max(m => m.Id);
                var data = _context.TasaDolar.Find(tazaId);
                var lis = _context.Productos.ToList();
                foreach (var item in lis)
                {
                    item.Precio = Convert.ToDecimal(item.PrecioDolar * data!.Tasa);
                };
                _context.Productos.UpdateRange(lis);
                _context.SaveChanges();
            }
			catch (Exception e)
			{
				return Json(false);
			}
			
			return Json (true);
		}
	}
	
}
