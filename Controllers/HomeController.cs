using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using THAMCOMVC.Models;
using THAMCOMVC.Services;

namespace THAMCOMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var products = ProductService.GetProducts();
        return View(products);
    }

    public IActionResult Search(string query)
        {
            var allProducts = ProductService.GetProducts();
            var filteredProducts = allProducts.Where(p => p.ProductName.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return View("Index", filteredProducts); // Return filtered products to the Index view
        }

         [HttpPost]
    public IActionResult AddToBasket(int productId, int quantity)
    {
        var identity = User.Identity?.IsAuthenticated;
        if (identity==true)
        {
            return RedirectToAction("Login", "Account");
        }

        // Simulate adding to basket (implement actual logic here)
        // Example: Add the product to a database or session-based cart
        TempData["Message"] = $"Added {quantity} of Product ID {productId} to the basket.";

        return RedirectToAction("Index");
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
