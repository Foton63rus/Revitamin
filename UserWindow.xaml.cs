using System.Net;
using System.Text;
using System.Windows;
using Revitamin.Entity;

namespace Revitamin
{
    public partial class UserWindow : Window
    {
        string url = "http://edin.starkandbau.ru/write";
        Specificator specificator;
        public UserWindow( Specificator specificator)
        {
            InitializeComponent();
            this.specificator = specificator;
            string json = specificator.GetJson();
            ConsoleBlock.Text += json;

            //WebPostRequest(url, json);
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
    }
}
