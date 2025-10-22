using Microsoft.Extensions.DependencyInjection;
using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Services;
using PrintJobInterceptor.Presentation;
using PrintJobInterceptor.UI.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;
using Serilog;

namespace PrintJobInterceptor
{
    internal static class Program
    {
        private const bool IS_TEST_MODE = false;
        [System.STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
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
            string logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PrintJobInterceptor",
                "log-.txt");
            Serilog.Core.Logger logger = new LoggerConfiguration()
                .MinimumLevel.Debug() 
                .WriteTo.Console()   
                .WriteTo.Debug()     
                .WriteTo.File(        
                    path: logPath,
                    rollingInterval: RollingInterval.Day, // Creates a new file each day
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10485760, // 10 MB file limit
                    retainedFileCountLimit: 5      // Keep the last 5 log files
                )
                .CreateLogger();
            services.AddLogging(builder =>
            {
                builder.ClearProviders(); 
                builder.AddSerilog(logger); 
            });

            if (IS_TEST_MODE) { services.AddSingleton<IPrintJobService, MockPrintJobService>(); }

            else { services.AddSingleton<IPrintJobService, PrintJobService>(); }

            services.AddTransient<IMainFormView, MainForm>();
            services.AddTransient<MainFormPresenter>();


        }
    }
}