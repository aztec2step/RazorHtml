
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace RazorHtmlEmails.RazorClassLib2
{
	public class WebHostEnvironment : IWebHostEnvironment
	{
		public string WebRootPath { get; set; } = string.Empty;
		public IFileProvider WebRootFileProvider { get; set; }
		public string ApplicationName { get; set; } = string.Empty;
		public IFileProvider ContentRootFileProvider { get; set; }
		public string ContentRootPath { get; set; } = string.Empty;
		public string EnvironmentName { get; set; } = string.Empty;
	}
}