using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using FluentAssertions.Json;
using System.Threading.Tasks;
using Xunit;

namespace CICD.IntegrationTest
{
    public class WebEmailTest
    {
        // lokale uitvoering
        public const string mvcAppRoot = "http://localhost:8080";
        public const string mailHogApiV2Root = "http://localhost:8025/api/v2";

        // uitvoering in Docker container
        //public const string mvcAppRoot = "http://mvcapp";
        //public const string mailHogApiV2Root = "http://mail:8025/api/v2";

        [Fact]
        public async Task SendEmailWithNames_IsFromGenerator()
        {
            // send email
            var client = new HttpClient();
            var sendEmail = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{mvcAppRoot}")
            };
            Console.WriteLine($"Sending email: {sendEmail.RequestUri}");
            using (var response = await client.SendAsync(sendEmail))
            {
                response.EnsureSuccessStatusCode();
            }

            // check if email is sent
            var checkEmails = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{mailHogApiV2Root}/messages")
            };
            Console.WriteLine($"Checking emails: {checkEmails.RequestUri}");
            using (var response = await client.SendAsync(checkEmails))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var messages = JObject.Parse(content);
                messages.Should().HaveElement("total").Which.Should().HaveValue("1");
                messages.Should().HaveElement("items")
                    .Which.Should().BeOfType<JArray>()
                    .Which.First.Should().HaveElement("Raw")
                    .Which.Should().HaveElement("From")
                    .Which.Should().HaveValue("cicdsolution@howestgp.be");
            }
        }
    }
}

