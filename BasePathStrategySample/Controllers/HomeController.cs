using System.Linq;
using Farmer.Data;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;

namespace BasePathStrategySample.Controllers
{
    public class HomeController : Controller
    {
        private FarmerDataContext _context;
        public HomeController(FarmerDataContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            //var transactions = _context.Transactions.ToList();
            var ti = HttpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo;
            return View(ti);
        }
    }
}
