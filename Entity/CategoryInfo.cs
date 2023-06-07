using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revitamin
{
    public class CategoryInfo
    {
        public BuiltInCategory Category;
        public List<ElementInfo> Elements;
        public CategoryInfo(BuiltInCategory categoryName) 
        {
            this.Category = categoryName;
            Elements = new List<ElementInfo>();
        }
        public bool hasElementWithName(string name) => Elements.Any(el => el.Name == name);

        public ElementInfo this[string elementName] => Elements?.First(el => el.Name == elementName);

        public void AddElementInfo( ElementInfo elementInfo )
        {
            Elements.Add(elementInfo);
        }

        public string GetInfo()
        {
            StringBuilder output = new StringBuilder( $"{Category.ToString()}:\n" );
            foreach ( ElementInfo ei in Elements )
            {
                output.AppendLine( $"{ei.Name} count={ei.Count} S={ei.Area}m2 V={ei.Volume}m3" );
            }
            return output.ToString();
        }
    }
}
