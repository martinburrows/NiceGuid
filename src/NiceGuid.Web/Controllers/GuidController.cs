using System.Web.Mvc;
using NiceGuid.Generator;

namespace NiceGuid.Web.Controllers
{
    public class GuidController : Controller
    {
        private readonly IGuidGeneratorFactory _guidGeneratorFactory;

        public GuidController(IGuidGeneratorFactory guidGeneratorFactory)
        {
            _guidGeneratorFactory = guidGeneratorFactory;
        }

        public ActionResult Index()
        {
            ViewBag.Guid = GetGuid();
            return View();
        }

        public ActionResult Generate()
        {
            if (!Request.IsAjaxRequest()) return HttpNotFound();
            return Content(GetGuid());
        }

        private string GetGuid()
        {
            return _guidGeneratorFactory.GetGuidGenerator().GetNiceGuid();
        }
    }
}