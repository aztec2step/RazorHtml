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

namespace RazorHtmlEmails.RazorClassLib2
{
	public static class ExtensionMethods
	{
		public static IServiceCollection UseRazorViewRendererInConsole(this IServiceCollection services)
		{
			// It's required by IOC
			var diagnosticListener = new DiagnosticListener("Microsoft.AspNetCore");
			services.AddSingleton(diagnosticListener);
			services.AddSingleton<DiagnosticSource>(diagnosticListener);

			// Here we need to register assembly to make razor find compiled views            
			services.AddRazorPages()
			  .AddApplicationPart(Assembly.GetExecutingAssembly())
			  .AddRazorRuntimeCompilation();

			// Now we need to add IWebHostEnvironment with custom implementation as default one is internal.
			// Implementation is just bunch of properties without any additional logic.
			var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
			services.AddSingleton<IWebHostEnvironment>(new WebHostEnvironment
			{
				ApplicationName = Assembly.GetEntryAssembly().GetName().Name,
				WebRootFileProvider = fileProvider,
				ContentRootFileProvider = fileProvider
			});

			return services;
		}
	}
}
