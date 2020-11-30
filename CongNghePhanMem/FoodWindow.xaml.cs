using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml;
using xNet;
using Formatting = Newtonsoft.Json.Formatting;
using MultipartContent = xNet.MultipartContent;
using StringContent = xNet.StringContent;

namespace CongNghePhanMem
{
    /// <summary>
    /// Interaction logic for FoodWindow.xaml
    /// </summary>
    public partial class FoodWindow : Window
    {
        public static string apiFoodUrl, apiEditFoodUrl, apiDeleteFoodUrl;
        public static void UrlDifine(int num)// 1 hosting online, else local
        {
            if(num == 1)
            {
                apiFoodUrl = "https://rescom.000webhostapp.com/public/api/food/";
                apiEditFoodUrl = "https://rescom.000webhostapp.com/public/api/food/update/";
                apiDeleteFoodUrl = "https://rescom.000webhostapp.com/public/api/food/delete/";
            }
            else
            {
                apiFoodUrl = "http://res.com/api/food";
                apiEditFoodUrl = "http://res.com/api/food/update/";
                apiDeleteFoodUrl = "http://res.com/api/food/delete/";

            }

        }
         
        public FoodWindow()
        {
            InitializeComponent();
            UrlDifine(0);
            getAll(null, null);
        }

        private  void getAll(object sender, RoutedEventArgs e)
        {
            var resp =  GetData(apiFoodUrl);
            var respon = JsonConvert.DeserializeObject<List<Food1>>(resp.ToString());
            ListViewProducts.ItemsSource = respon;
        }

       
        string GetData(string url, string cookie = null)
        {
            HttpRequest http = new HttpRequest();
            http.Cookies = new CookieDictionary();
            string html = http.Get(url).ToString();
            return html;
        }
        string DeleteData(string url, string cookie = null)
        {
            string html = "";
            try
            {
                HttpRequest http = new HttpRequest();
                http.Cookies = new CookieDictionary();
                html = http.Get(url).ToString();
                getAll(null, null);

            }
            catch
            {

            }
            return html;
        }


        private async void btnPostClick(object sender, RoutedEventArgs e)
        {
            var path = pathImage.Text;
            MultipartContent data;
            try
            {
                data = new MultipartContent() {
                { new StringContent(tbName.Text), "name"},
                { new StringContent(tbPrice.Text), "price"},
                { new FileContent(path), "image", "1.jpg"}
                };
            }
            catch
            {
                data = new MultipartContent() {
                { new StringContent(tbName.Text), "name"},
                { new StringContent(tbPrice.Text), "price"}
                };
            }

            var html = UploadData(null, "http://res.com/api/food", data);
            getAll(sender, e);
            //txt.Text = html;
        }



        string UploadData(HttpRequest http, string url, MultipartContent data = null, string contentType = null, string userArgent = "", string cookie = null)
        {
            if (http == null)
            {
                http = new HttpRequest();
                http.Cookies = new CookieDictionary();
            }

            //if (!string.IsNullOrEmpty(cookie))
            //{
            //    AddCookie(http, cookie);
            //}

            if (!string.IsNullOrEmpty(userArgent))
            {
                http.UserAgent = userArgent;
            }

            string html = http.Post(url, data).ToString();
            return html;
        }

        string UpdateData(HttpRequest http, string url, MultipartContent data = null, string contentType = null, string userArgent = "", string cookie = null)
        {
            if (http == null)
            {
                http = new HttpRequest();
                http.Cookies = new CookieDictionary();
            }

            //if (!string.IsNullOrEmpty(cookie))
            //{
            //    AddCookie(http, cookie);
            //}

            if (!string.IsNullOrEmpty(userArgent))
            {
                http.UserAgent = userArgent;
            }

            string html = http.Post(url, data).ToString();
            return html;
        }
        void UploadFile(string path)
        {

            //var dataRes = JsonConvert.DeserializeObject<UploadFileModel>(html);

        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                pathImage.Text = dialog.FileName;
            }
        }

        private void btnDelClick(object sender, RoutedEventArgs e)
        {
            var id = tbId.Text;
            txt.Text = id;
            var html = DeleteData($"http://res.com/api/food/delete/{id}");
        }

        private void btnEditClick(object sender, RoutedEventArgs e)
        {
            var id = tbId.Text;
            string path = pathImage.Text;
            txt.Text = path;
            MultipartContent data;
            if (path == "")
            {
                data = new MultipartContent() {
                { new StringContent(tbName.Text), "name"},
                { new StringContent(tbPrice.Text), "price"},
                };
            }
            else
            {
                data = new MultipartContent() {
                { new StringContent(tbName.Text), "name"},
                { new StringContent(tbPrice.Text), "price"},
                { new FileContent(path), "image", "1.jpg"}
                };
            }
            try
            {
                var html = UpdateData(null, $"http://res.com/api/food/update/{id}", data);
            }
            catch
            {

            }

            getAll(sender, e);
        }
    }

    public class Food1
    {
        public int id { get; set; }
        public string name { get; set; }

        public string price { get; set; }

        public string image { get; set; }


    }
}
