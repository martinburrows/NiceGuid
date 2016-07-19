using System.Web.Mvc;
using NiceGuid.Web.Services;
using NiceGuid.Web.Configuration;

namespace NiceGuid.Web.Controllers
{
    public class GuidController : Controller
    {
        private readonly IGuidGenerator _generator;
        private readonly string _trackingCode;

        public GuidController(IGuidGenerator generator, IAppSettings appSettings)
        {
            _generator = generator;
            _trackingCode = appSettings.TrackingCode;
        }

        public ActionResult Index()
        {
            ViewBag.TrackingCode = _trackingCode;
            return View();
        }

        public ActionResult Generate()
        {
#if !DEBUG
            if (HttpContext.Request.UrlReferrer?.ToString() != "http://niceguid.com/")
                return new HttpNotFoundResult();
#endif

            return Json(_generator.GetNiceGuid());
        }
    }
}