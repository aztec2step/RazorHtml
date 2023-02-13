using Microsoft.Extensions.DependencyInjection;
using RazorHtmlEmails.RazorClassLib;
using RazorHtmlEmails.RazorClassLib.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ConfirmAccount;
using RazorHtmlEmails.RazorClassLib2;
using RazorHtmlEmails.RazorClassLib2.Templates;
using RazorHtmlEmails.RazorClassLib2.Views;
using System.Reflection;

namespace RazorHtmlEmails.Tests
{
    public class RendererTests
    {
        private readonly IRazorViewToStringRenderer? renderer;
        private readonly string expected1;
        private readonly string expected2;

        public RendererTests()
        {
            //Setup (as required)

            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("appsettings.json", optional: true);
            builder.Environment.IncludeRazorTemplates();
            builder.Services.UseRazorViewRendererInConsole();
            var app = builder.Build();
            renderer = app.Services.GetRequiredService<IRazorViewToStringRenderer>();


            var assembly = typeof(Template1Model).GetTypeInfo().Assembly;

			using var stream = assembly!.GetManifestResourceStream("RazorHtmlEmails.RazorClassLib2.Views.Expected.html");
			using var reader = new StreamReader(stream);
            expected1 = reader.ReadToEnd();

            using var stream2 = assembly!.GetManifestResourceStream("RazorHtmlEmails.RazorClassLib2.Templates.Expected.html");
            using var reader2 = new StreamReader(stream2);
            expected2 = reader2.ReadToEnd();
        }

        [Fact]
        public async void Test1()
        {
            var model1 = new Template1Model();
            model1.MenuItems.Add("Home");
            model1.MenuItems.Add("Login");
            model1.MenuItems.Add("About");
            model1.Items.Add("Alpha");
            model1.Items.Add("Beta");
            model1.Items.Add("Production");

            //string body = await renderer.RenderViewToStringAsync("Template1", model1);
            string body = await renderer.RenderViewToStringAsync("/Views/Template1.cshtml", model1);
            Assert.Equal(expected1, body);

            var model2 = new Template2Model();
            model2.MenuItems.Add("Home");
            model2.MenuItems.Add("Login");
            model2.MenuItems.Add("About");
            model2.Items.Add("Alpha");
            model2.Items.Add("Beta");
            model2.Items.Add("Production");

            //body = await renderer.RenderViewToStringAsync("Template2", model2);
            body = await renderer.RenderViewToStringAsync("/Templates/Template2.cshtml", model2);
            Assert.Equal(expected2, body);

        }

        [Fact]
        public async void Test2()
        {


            var model = new ConfirmAccountEmailViewModel($"https://localhost/{Guid.NewGuid()}");

            string body = await renderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", model);

            Assert.False(string.IsNullOrWhiteSpace(body));
        }
    }
}