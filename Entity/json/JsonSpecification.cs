using System;

namespace Revitamin.Entity.json
{
    [Serializable]
    public class JsonSpecification
    {
        public string project_id;
        public string project_key;
        public string project_version;
        public string app_version;

        public string filename;
        public string project;
        public ElementJsonView[] objects;
        public JsonSpecification(string app_version, string project_id, string project_key, string project_version, string filename, ElementJsonView[] objects, string project = "")
        {
            this.app_version = app_version;
            this.project_id = project_id;
            this.project_key = project_key;
            this.project_version = project_version;
            this.filename = filename;
            this.project = project;
            this.objects = objects;
        }
    }
}
