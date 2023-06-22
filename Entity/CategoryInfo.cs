using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revitamin
{
    public class CategoryInfo
    {
        public BuiltInCategory Category;
        public List<ElementGroupInfo> Elements;
        public CategoryInfo(BuiltInCategory categoryName) 
        {
            this.Category = categoryName;
            Elements = new List<ElementGroupInfo>();
        }
        public bool hasElementWithName(string name) => Elements.Any(el => el.Name == name);

        public ElementGroupInfo this[string elementName] => Elements?.First(el => el.Name == elementName);

        public void AddElementInfo(ElementGroupInfo elementInfo )
        {
            Elements.Add(elementInfo);
        }

        public string GetInfo()
        {
            StringBuilder output = new StringBuilder( $"{Category.ToString()}:\n" );
            //foreach (ElementGroupInfo ei in Elements.OrderBy(x => x.Name) )
            foreach (ElementGroupInfo ei in Elements)
            {
                output.AppendLine( ei.getInfo() );
            }
            return output.ToString();
        }
    }
}
