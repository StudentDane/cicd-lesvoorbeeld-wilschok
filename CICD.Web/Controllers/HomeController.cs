using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICD.Web.Models;
using CICD.Lib;
using CICD.Web.ViewModels;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace CICD.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            return await Index("1,2,3");
        }

        [HttpPost("input")]
        public async Task<IActionResult> Index(string input)
        {
            StringCalculatorViewModel calculatorViewModel = CalculateSum(input);
            await SendMail(calculatorViewModel);
            return View(calculatorViewModel);
        }

        private async Task SendMail(StringCalculatorViewModel content)
        {
            // const string MAIL_HOST = "localhost"; // voor lokale uitvoering
            const string MAIL_HOST = "mail"; // in docker

            const int MAIL_PORT = 1025;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CICD Web Application", "cicdsolution@howestgp.be"));
            message.To.Add(new MailboxAddress("Student", "student@sumwanted.com"));
            message.Subject = "Your calculated Sum";
            message.Body = new TextPart("plain")
            {
                Text = $"Hello, your calculation result: sum of {content.Input} = {content.Sum} " +
                $"{(content.Error != String.Empty ? $"\n\nError: {content.Error} " : "")}"
            };
            using (var mailClient = new SmtpClient())
            {
                await mailClient.ConnectAsync(MAIL_HOST, MAIL_PORT, SecureSocketOptions.None);
                await mailClient.SendAsync(message);
                await mailClient.DisconnectAsync(true);
            }

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
