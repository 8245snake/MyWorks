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
#if DEBUG
            serviceCollection.AddBlazorWebViewDeveloperTools();
#endif

            serviceCollection.AddAntDesign();

            string myDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().FullName);
            string baseDir = Path.Combine(myDir, "data");

            var masterRepo = GetTsvWorkCodeFamilyRepository(baseDir);
            var dataRepo = GetDutyRepository(baseDir);
            var todoRepo = GetToDoItemRepository(baseDir);
            var preferenceRepository = new JsonPreferenceRepository(baseDir);
            serviceCollection.AddScoped(sp => new SchedulingServive(dataRepo, masterRepo, masterRepo, todoRepo, preferenceRepository));
            serviceCollection.AddScoped(sp => new UserPreferenceService(preferenceRepository));
            serviceCollection.AddScoped<JsInteropService>();
            serviceCollection.AddScoped<ClipboardService>();
            serviceCollection.AddScoped(sp => new ControlService(new FocusManeger(this.Handle)));
            serviceCollection.AddSingleton<PageNavigatingService>();

            Resources.Add("services", serviceCollection.BuildServiceProvider());

            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private static JsonToDoItemRepository GetToDoItemRepository(string baseDir)
        {
            string todoDataDir = Path.Combine(baseDir, "todo");
            JsonToDoItemRepository todoRepo = new JsonToDoItemRepository(todoDataDir);
            return todoRepo;
        }

        private static JsonDutyRepository GetDutyRepository(string baseDir)
        {
            string dutyDataDir = Path.Combine(baseDir, "json");
            JsonDutyRepository dataRepo = new JsonDutyRepository(dutyDataDir);
            return dataRepo;
        }

        private static TsvWorkCodeFamilyRepository GetTsvWorkCodeFamilyRepository(string baseDir)
        {
            try
            {
                string masterFilePath = Path.Combine(baseDir, "master.txt");
                if (!File.Exists(masterFilePath)) File.WriteAllText(masterFilePath, "");

                var text = File.ReadAllText(masterFilePath);
                if (string.IsNullOrWhiteSpace(text))
                {
                    string templateFilePath = Path.Combine(baseDir, "master.tmplate");
                    File.Copy(templateFilePath, masterFilePath, true);
                }

                TsvWorkCodeFamilyRepository masterRepo = new TsvWorkCodeFamilyRepository(masterFilePath);
                return masterRepo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
