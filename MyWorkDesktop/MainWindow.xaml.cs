using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWorkDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //var provider = App.Current.Resources["services"] as IServiceProvider;

            var asm = Assembly.Load(new AssemblyName("MyWorkDashboard.Shared"));
            var version = asm.GetName()?.Version?.ToString();
            if (!string.IsNullOrWhiteSpace(version))
            {
                this.Title += $"    V{version}";
            }


        }

    }
}
