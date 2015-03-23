using System.Web.Mvc;
using NiceGuid.Generator;
using NiceGuid.Web.Configuration;

namespace NiceGuid.Web.Controllers
{
    public class GuidController : Controller
    {
        private readonly IGuidGeneratorFactory _guidGeneratorFactory;
        private readonly IAppSettings _appSettings;

        public GuidController(IGuidGeneratorFactory guidGeneratorFactory, IAppSettings appSettings)
        {
            _guidGeneratorFactory = guidGeneratorFactory;
            _appSettings = appSettings;
        }

        public ActionResult Index()
        {
            ViewBag.Guid = GetGuid();
            ViewBag.TrackingCode = _appSettings.TrackingCode;
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