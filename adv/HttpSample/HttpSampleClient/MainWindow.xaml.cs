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
            _vm.PersonNo = "100";
            _vm.Status = "帰宅";
            this.DataContext = _vm;
        }

        /// 出勤状態のリストを取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void clickGetList(object sender, RoutedEventArgs e)
        {
#if DEBUG
            var URL = "http://localhost:7071/api/ReadData";
#else
            var URL = "";
#endif
            var cl = new HttpClient();
            var json = await cl.GetStringAsync(URL);
            var data = JsonConvert.DeserializeObject<List<Persons>>(json);
            _vm.Items = data;
        }

        /// <summary>
        /// 出勤状態を送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        private async void clickSend(object sender, RoutedEventArgs e)
        {
#if DEBUG
            var URL = "http://localhost:7071/api/WriteData";
#else
            var URL = "";
#endif
            var cl = new HttpClient();
            var content = new StringContent(
                $"{{ pno:\"{_vm.PersonNo}\", status:\"{_vm.Status}\" }}");
            var res = await cl.PostAsync(URL, content);
            var result = await res.Content.ReadAsStringAsync();
            MessageBox.Show(result);
        }
    }
}
