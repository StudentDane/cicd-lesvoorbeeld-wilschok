using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICD.Web.Models;
using CICD.Lib;
using CICD.Web.ViewModels;

namespace CICD.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return Index("1,2,3");
        }

        [HttpPost("input")]
        public IActionResult Index(string input)
        {
            return View(CalculateSum(input));
        }

        private StringCalculatorViewModel CalculateSum(string input)
        {
            StringCalculatorViewModel scvm = new StringCalculatorViewModel
            {
                Input = input
            };
            try
            {
                StringCalculator stringCalculator = new StringCalculator();
                int sum = stringCalculator.Add(input);
                scvm.Sum = sum;
            }
            catch (Exception ex)
            {
                scvm.Sum = 0;
                scvm.Error = ex.Message;
            }
            return scvm;

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
