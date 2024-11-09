using System.Diagnostics;
using Blood_Donation.Models;
using Blood_Donation.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBloodRepo _bloodRepo;

        public HomeController(ILogger<HomeController> logger, IBloodRepo bloodRepo)
        {
            _logger = logger;
            _bloodRepo = bloodRepo;
        }

        public async Task<IActionResult> Index()
        {
            // Await the asynchronous method to get blood records
            var bloods = await _bloodRepo.GetAllBloodsAsync();

            // Ensure bloods is not null and contains elements
            if (bloods.Any())
            {
                Blood.RecentDonationDate = bloods.Max(b => b.DonationDate);
            }

            // Calculate if the user is eligible based on recent donation date
            ViewBag.IsEligible = Blood.RecentDonationDate.AddDays(100) <= DateTime.Now;
            ViewBag.RecentDonationDate = Blood.RecentDonationDate;

            // Pass the blood records to the view
            return View(bloods);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
