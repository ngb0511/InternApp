using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Base.WebApp.Controllers
{
    public class UpdateController : Controller
    {
        public MaterialMasterVM ? materialMasterVM;
        public IActionResult Index()
        {
            return View();
        }
    }
}
