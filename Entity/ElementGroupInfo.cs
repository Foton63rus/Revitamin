using Autodesk.Revit.DB;

namespace Revitamin
{
    public class ElementGroupInfo
    {
        public int id;
        public string Name;
        public int Count;
        public double Area;
        public double Volume;
        public ElementGroupInfo( string name )
        {
            this.Name = name;
        }
        public string getInfo()
        {
            return $"{this.Name} count={this.Count} S={this.Area}m2 V={this.Volume}m3";
        }
    }
}
