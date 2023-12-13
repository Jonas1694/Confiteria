using ArquitecturaModel;
using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
			var t = _context.TasaDolars.FirstOrDefault();
			return View(t);
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
                _context.Update(tasaDolar);
                _context.SaveChanges();
            }
            return View(tasaDolar);
		}
        [HttpPost]
        public IActionResult UpdatePrecio()
		{
			try
			{
                var data = _context.TasaDolars.FirstOrDefault();
                var lis = _context.Productos.ToList();
                foreach (var item in lis)
                {
                    item.Precio = Convert.ToDecimal(item.PrecioDolar * data.Tasa);
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
