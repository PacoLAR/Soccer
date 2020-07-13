using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoccerMVC.Models;
using LibreriaSoccer;
using System.Text.Json;

namespace SoccerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> TableOfSeason(String name,string country, string year){
            Console.WriteLine($"{name}{country}{year}");
            if(country == "MEXICO"){
                country = "mx.1.csv";
            }else if(country == "ENGLAND"){
                country = "eng.1.csv";
            }else{
                country= "es.1.csv";
            }
            Season temporada = new Season(null,country);
            String[]lineas = await temporada.ReadSeasonFromFile(country);
          
            ViewData["lineas"]= lineas.ToList();
            
            return View();
        }
        /*
        public IActionResult Privacy()
        {
            return View();
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
