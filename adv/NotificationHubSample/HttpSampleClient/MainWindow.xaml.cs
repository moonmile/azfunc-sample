using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace HttpSampleClient
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        ViewModel _vm;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = new ViewModel();
            this.DataContext = _vm;
        }


        /// <summary>
        /// プッシュメッセージを送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        private async void clickSend(object sender, RoutedEventArgs e)
        {
            var URL = "http://localhost:7071/api/FuncPush";
            var cl = new HttpClient();
            var content = new StringContent(
                $"{{ name:\"{_vm.Name}\", message:\"{_vm.Message}\" }}");
            var res = await cl.PostAsync(URL, content);
            var result = await res.Content.ReadAsStringAsync();
            MessageBox.Show(result);
        }
    }
}
