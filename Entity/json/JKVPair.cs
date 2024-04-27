using System;

namespace Revitamin.Entity.json
{
    [Serializable]
    public class JKVPair
    {
        public string name;
        public object value;

        public JKVPair(string name, object value) 
        { 
            this.name = name;
            this.value = value;
        }
    }
}
