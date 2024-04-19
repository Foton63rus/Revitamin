namespace Revitamin.Entity.WebService
{
    internal interface IWebService
    {
        void GetRequest();

        void PostRequest(string json);
    }
}
