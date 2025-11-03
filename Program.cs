using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Services;
using PrintJobInterceptor.Core.Services.Helper;
using PrintJobInterceptor.Presentation;
using PrintJobInterceptor.UI.Interfaces;
using Serilog;
using System;
using System.Windows.Forms;

namespace PrintJobInterceptor
{
    internal static class Program
    {
        private const bool IS_TEST_MODE = true;
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
            if (IS_TEST_MODE)
            {
                try
                {
                    var testForm = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions
                            .GetRequiredService<TestScenarioForm>(serviceProvider);
                    testForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load Test Scenario form: {ex.Message}", "Test Mode Error");
                }
            }
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
                    rollingInterval: RollingInterval.Day, 
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10485760, //10mb
                    retainedFileCountLimit: 5       
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
            services.AddSingleton<MainFormPresenter>();
            if (IS_TEST_MODE)
            {
                services.AddTransient<TestScenarioForm>();
            }


        }
    }
}