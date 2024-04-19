using System;

namespace Revitamin.Entity.json
{
    [Serializable]
    public class JsonSpecification
    {
        public string project_id;
        public string project_key;
        public string project_version;

        public string filename;
        public string project;
        public JsonElement[] objects;
        public JsonSpecification(string project_id, string project_key, string project_version, string filename, JsonElement[] objects, string project = "")
        {
            this.project_id = project_id;
            this.project_key = project_key;
            this.project_version = project_version;
            this.filename = filename;
            this.project = project;
            this.objects = objects;
        }
    }
}
