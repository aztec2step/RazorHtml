using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

using RazorHtmlEmails.RazorClassLib2;
using RazorHtmlEmails.RazorClassLib.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ConfirmAccount;

namespace RazorHtmlEmails.RazorClassLib2
{
    public static class ExtensionMethods
    {
        public static IServiceCollection UseRazorViewRendererInConsole(this IServiceCollection services)
        {
            //// It's required by IOC
            //var diagnosticListener = new DiagnosticListener("Microsoft.AspNetCore");
            //services.AddSingleton(diagnosticListener);
            //services.AddSingleton<DiagnosticSource>(diagnosticListener);

            var fp1a = new EmbeddedFileProvider(typeof(Templates.Template2Model).Assembly, "RazorHtmlEmails.RazorClassLib2");
            var fp1b = new EmbeddedFileProvider(typeof(ConfirmAccountEmailViewModel).Assembly, "RazorHtmlEmails.RazorClassLib");
            var fp2 = new PhysicalFileProvider(Directory.GetCurrentDirectory()); //Does not work.

            // Here we need to register assembly to make razor find compiled views            
            services.AddRazorPages()
                //.AddApplicationPart(Assembly.GetExecutingAssembly())
                .AddApplicationPart(typeof(Templates.Template2Model).Assembly)
                .AddApplicationPart(typeof(ConfirmAccountEmailViewModel).Assembly);
                //.AddRazorRuntimeCompilation(o =>
                //{
                //    //o.FileProviders.Add(fp1a);
                //    //o.FileProviders.Add(fp1b);
                //    //o.FileProviders.Add(fp2);
                //});
            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            // Now we need to add IWebHostEnvironment with custom implementation as default one is internal.
            // Implementation is just bunch of properties without any additional logic.
            //var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            //services.AddSingleton<IWebHostEnvironment>(new WebHostEnvironment
            //{
            //	ApplicationName = Assembly.GetEntryAssembly().GetName().Name,
            //	WebRootFileProvider = fileProvider,
            //	ContentRootFileProvider = fileProvider
            //});

            return services;
        }

        public static IWebHostEnvironment IncludeRazorTemplates(this IWebHostEnvironment host)
        {
            // Now we need to add IWebHostEnvironment with custom implementation as default one is internal.
            // Implementation is just bunch of properties without any additional logic.
            var fp1 = new EmbeddedFileProvider(typeof(Templates.Template2Model).Assembly, "RazorHtmlEmails.RazorClassLib2");
            var fp2 = new PhysicalFileProvider(Directory.GetCurrentDirectory()); //Does not work.
            host.WebRootFileProvider = fp1;
            host.ContentRootFileProvider = fp1;

            return host;
        }
    }
}
