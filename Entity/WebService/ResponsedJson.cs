using System;

namespace Revitamin.Entity.WebService
{
    [Serializable]
    public class ResponsedJson
    {
        public string result;
        public string message;
        public string link;
        public string error_code;

        public override string ToString() => 
            $"[ResponsedJson] result: {result}, message: {message}, link: {link}, error_code: {error_code}";
    }
}
