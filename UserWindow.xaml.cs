using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Revitamin.Entity;

namespace Revitamin
{
    public partial class UserWindow : Window
    {
        string url = "http://edin.starkandbau.ru/write";
        Specificator specificator;
        private IChecker checker;
        public UserWindow( Specificator specificator)
        {
            InitializeComponent();
            initializeCheckerTab();
            this.specificator = specificator;
            string json = specificator.GetJson();
            ConsoleBlock.Text += json;
            //WebPostRequest(url, json);
        }
        private void initializeCheckerTab()
        {
            checker = CMD_GetInfo.checker;
            checker.SetUserWindow(this);

            tboxCheckerConsole.Text = "";

            foreach (string key in CMD_GetInfo.GLOBAL_VARIABLES.BuiltInCategories.Keys)
            {
                ComboBoxCategoryParametrChecker.Items.Add(key);
            }
        }
        public string WebPostRequest(string url, string data)
        {
            using (WebClient client = new WebClient())
            {
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("data", data);
                byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                MessageBox.Show(responsebody);
                return responsebody;
            }
        }
        void btnCheckerCheckClick(object sender, RoutedEventArgs e)
        {
            //tboxCheckerConsole.Text += $"{ComboBoxCategoryParametrChecker.SelectedItem} {tboxCheckerParameter.Text}\n";
            tboxCheckerConsole.Text += $"{checker.check()}\n";
        }
        void btnCheckerClearConsoleClick(object sender, RoutedEventArgs e)
        {
            tboxCheckerConsole.Text = "";
        }
    }
}
