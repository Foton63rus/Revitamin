using System;

namespace Revitamin.Entity.json
{
    [Serializable]
    public class JsonSpecification
    {
        public string filename;
        public string project;
        public JsonElement[] objects;
        public JsonSpecification(string filename, JsonElement[] objects, string project = "")
        {
            this.filename = filename;
            this.project = project;
            this.objects = objects;
        }
    }
}
