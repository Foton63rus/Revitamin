using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Windows;

namespace Revitamin.Entity.WebService
{
    internal class WebService : IWebService
    {
        const string url = "http://uniceros.alexanderivanof.ru/revit-project-objects-add";

        public void GetRequest()
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(url);
            ResponsedJson responsedJson = JsonConvert.DeserializeObject<ResponsedJson>(reply);
            MessageBox.Show(responsedJson.ToString());
        }

        public void PostRequest( string json)
        {
            string responsebody = null;

            using (WebClient myWebClient = new WebClient())
            {
                try
                {
                    byte[] postArray = Encoding.ASCII.GetBytes(json);
                    myWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    byte[] responseArray = myWebClient.UploadData(url, "POST", postArray);
                    responsebody = Encoding.ASCII.GetString(responseArray);
                    ResponsedJson responsedJson = JsonConvert.DeserializeObject<ResponsedJson>(responsebody);
                    MessageBox.Show(responsedJson.ToString());
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.TargetSite.ToString());
                }
                
            }
        }
    }
}
