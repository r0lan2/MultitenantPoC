using System.Collections.Generic;
using System.Linq;
using Farmer.Data;
using Farmer.Data.Model;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;

namespace BasePathStrategySample.Controllers
{
    public class HomeController : Controller
    {
        //private FarmerDataContext _context;
        //public HomeController(FarmerDataContext context)
        //{
        //    _context = context;
        //}
        
        public IActionResult Index()
        {
           
            var ti = HttpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo;
            if (ti == null)
                return View(new TransactionViewModel()
                {
                    TenantInfo = null,
                    Transactions = new List<Transaction>()
                });
            
            using (var context = new FarmerDataContext(ti))
            {
                var transactions = context.Transactions.ToList();
                return View(new TransactionViewModel()
                {
                    TenantInfo = ti,
                    Transactions = transactions
                });
            }




          
        }
    }


    public class TransactionViewModel
    {
        public TenantInfo TenantInfo { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

}
