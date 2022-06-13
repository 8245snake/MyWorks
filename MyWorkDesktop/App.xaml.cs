using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Threading;
using MyWorkDashboard.Shared.Services;
using MyWorkDesktop.Services;


namespace MyWorkDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();

            serviceCollection.AddAntDesign();

            string myDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);
            string baseDir = Path.Combine(myDir, "data");

            string masterFilePath = Path.Combine(baseDir, "master.txt");
            TsvWorkCodeFamilyRepository masterRepo = new TsvWorkCodeFamilyRepository(masterFilePath);

            string dutyDataDir = Path.Combine(baseDir, "json");
            JsonDutyRepository dataRepo = new JsonDutyRepository(dutyDataDir);

            string todoDataDir = Path.Combine(baseDir, "todo");
            JsonToDoItemRepository todoRepo = new JsonToDoItemRepository(todoDataDir);

            serviceCollection.AddScoped(sp => new SchedulingServive(dataRepo, masterRepo, masterRepo, todoRepo));

            serviceCollection.AddScoped(sp => new ControlService(new FocusManeger(this.Handle)));
            serviceCollection.AddSingleton<PageNavigatingService>();

            Resources.Add("services", serviceCollection.BuildServiceProvider());

            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("例外が発生したためアプリケーションを終了します");
        }

        // ウィンドウハンドルを取得
        public IntPtr Handle
        {
            get
            {
                var helper = new System.Windows.Interop.WindowInteropHelper(this.MainWindow);
                return helper.Handle;
            }
        }
    }
}
