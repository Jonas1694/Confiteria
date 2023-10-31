using ArquitecturaModel;
using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Confiteria.Controllers
{
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

	}
}
