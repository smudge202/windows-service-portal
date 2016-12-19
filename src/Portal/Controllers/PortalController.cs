using Microsoft.AspNetCore.Mvc;

namespace Gilmond.WindowsService.Portal.Controllers
{
	[Route("")]
	public class PortalController : Controller
	{
		// GET: /<controller>/
		public IActionResult React()
			=> View();
	}
}
