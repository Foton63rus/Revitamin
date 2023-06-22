using System;

namespace Revitamin.Entity.json
{
    [Serializable]
    public class JsonElement
    {
        public string category;
        public string name;
        public string className;
        public string[] material;
        public string level;
        public double v;
        public double m;
        public double s;
    }
}