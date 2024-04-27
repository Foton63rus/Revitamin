using System;

namespace Revitamin.Entity.json
{
    [Serializable]
    public class ElementJsonView
    {
        public int object_id;
        public string category;
        public string name;
        public string class_name;
        public string level;

        public JKVPair[] parameters = { };
        //public JKVPair[] Properties = { };
        //public JKVPair[] Geometry = { };
        //public JKVPair[] BoundingBox = { };
        public JKVPair[] material = { };
    }
}
