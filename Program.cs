using Microsoft.Extensions.DependencyInjection;
using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Services;
using PrintJobInterceptor.Presentation;
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
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var mainForm = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions
                                .GetRequiredService<IMainFormView>(serviceProvider) as Form;

            if (mainForm == null)
                throw new InvalidOperationException("IMainFormView resolve returned null or is not a Form.");

            Application.Run(mainForm);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPrintJobService, PrintJobService>();
            services.AddTransient<MainFormPresenter>();
            services.AddTransient<IMainFormView, MainForm>();
        }
    }
}