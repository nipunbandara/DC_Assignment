using Newtonsoft.Json;
using Registry_WebAPI.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for AllServices.xaml
    /// </summary>
    public partial class AllServices : Window
    {
        int token = 0;

        RestClient Regclient;
        RestClient SerProclient;


        public AllServices(int tok)
        {
            InitializeComponent();

            token = tok;
           
            string RegURL = "http://localhost:61028/";
            Regclient = new RestClient(RegURL);

            string SerProURL = "http://localhost:59522/";
            SerProclient = new RestClient(SerProURL);
        }

        private void Window_Startup(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("api/Registry/AllServices/" + token.ToString());
            RestResponse resp = Regclient.Get(request);
            Thread.Sleep(1000);
            List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(resp.Content);
            List<string> regName = new List<string>();
            foreach(Registry reg in regs)
            {
                regName.Add(reg.Name);
            }
           

            servicesListView.ItemsSource = regName;

            lNum1.Visibility = Visibility.Hidden;
            txtNum1.Visibility = Visibility.Hidden;

            lNum2.Visibility = Visibility.Hidden;
            txtNum2.Visibility = Visibility.Hidden;

            lNum3.Visibility = Visibility.Hidden;
            txtNum3.Visibility = Visibility.Hidden;
        }


        private void ServicesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (servicesListView.SelectedItem != null)
            {
                string serviceSelected = servicesListView.SelectedItem.ToString();
                lServiceSelected.Content = serviceSelected;

                RestRequest request = new RestRequest("api/Registry/search/" + token.ToString() + "/" + serviceSelected);
                RestResponse resp = Regclient.Get(request);

                List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(resp.Content);
                Registry reg = regs[0];

                int numop = reg.numberOfOperands;

                if (numop == 2)
                {
                    lNum1.Visibility = Visibility.Visible;
                    txtNum1.Visibility = Visibility.Visible;

                    lNum2.Visibility = Visibility.Visible;
                    txtNum2.Visibility = Visibility.Visible;

                    lNum3.Visibility = Visibility.Hidden;
                    txtNum3.Visibility = Visibility.Hidden;

                    txtNum1.Text = "";
                    txtNum2.Text = "";
                    txtNum3.Text = "";

                    lValue.Content = "";
                }
                else if (numop == 3)
                {
                    lNum1.Visibility = Visibility.Visible;
                    txtNum1.Visibility = Visibility.Visible;

                    lNum2.Visibility = Visibility.Visible;
                    txtNum2.Visibility = Visibility.Visible;

                    lNum3.Visibility = Visibility.Visible;
                    txtNum3.Visibility = Visibility.Visible;

                    txtNum1.Text = "";
                    txtNum2.Text = "";
                    txtNum3.Text = "";

                    lValue.Content = "";
                }
            }
            
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string sname = txtSearchService.Text;

            RestRequest request = new RestRequest("api/Registry/search/" + token.ToString() + "/" + sname);
            RestResponse resp = Regclient.Get(request);

            List<Registry> regs = JsonConvert.DeserializeObject<List<Registry>>(resp.Content);
            List<string> regName = new List<string>();
            foreach (Registry reg in regs)
            {
                regName.Add(reg.Name);
            }
            servicesListView.ItemsSource = regName;
            
        }

        // Submit button function
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            string serviceSelected = lServiceSelected.Content.ToString();
            if (serviceSelected.ToLower().Contains("add"))
            {
                int num1 = Convert.ToInt32(txtNum1.Text);
                int num2 = Convert.ToInt32(txtNum2.Text);
                RestRequest request;

                if (lNum3.Visibility == Visibility.Hidden)
                {
                    request = new RestRequest("api/service/add/" + token.ToString() + "/" + num1 + "/" + num2);
                }
                else
                {
                    int num3 = Convert.ToInt32(txtNum3.Text);
                    request = new RestRequest("api/service/add/" + token.ToString() + "/" + num1 + "/" + num2 + "/" + num3);
                }

                RestResponse resp = SerProclient.Get(request);

                lValue.Content = resp.Content;

            }
            if (serviceSelected.ToLower().Contains("mul"))
            {
                int num1 = Convert.ToInt32(txtNum1.Text);
                int num2 = Convert.ToInt32(txtNum2.Text);
                RestRequest request;

                if (lNum3.Visibility == Visibility.Hidden)
                {
                    request = new RestRequest("api/service/mul/" + token.ToString() + "/" + num1 + "/" + num2);
                }
                else
                {
                    int num3 = Convert.ToInt32(txtNum3.Text);
                    request = new RestRequest("api/service/mul/" + token.ToString() + "/" + num1 + "/" + num2 + "/" + num3);
                }

                RestResponse resp = SerProclient.Get(request);

                lValue.Content = resp.Content;

            }


           
        }
    }
}
