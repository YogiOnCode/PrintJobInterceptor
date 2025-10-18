using Microsoft.Extensions.DependencyInjection;
using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Services;
using PrintJobInterceptor.Presentation;
using PrintJobInterceptor.UI;
using PrintJobInterceptor.UI.Interfaces;
using System;
using System.Windows.Forms;


namespace PrintJobInterceptor
{
    internal static class Program
    {
        [System.STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services); // This method call needs the definition below
            var serviceProvider = services.BuildServiceProvider();

            var mainForm = ServiceProviderServiceExtensions.GetService<MainForm>(serviceProvider);
            Application.Run(mainForm);
        }

        // This method was missing from your file
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPrintJobService, PrintJobService>();
            services.AddTransient<MainFormPresenter>();
            services.AddTransient<MainForm>();
        }
    }
}