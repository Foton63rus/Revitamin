using System;
using System.Collections.Generic;

namespace Revitamin.Entity.json
{
    [Serializable]
    public class JsonElement
    {
        public int object_id;
        public string category;
        public string name;
        public string class_name;
        public string[] material;
        public string level;
        public List<JKVPair> parameters = new List<JKVPair>();
    }
}