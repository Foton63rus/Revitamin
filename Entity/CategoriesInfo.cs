using Autodesk.Revit.DB;
using Revitamin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revitamin
{
    public class CategoriesInfo
    {
        public Dictionary<BuiltInCategory, List<Element>> categoriesInfo;
        private List<ElementsInfo> _elementsInfoList;
        BuiltInCategory[] categoriesExcludedFromFilter = new BuiltInCategory[]
        {
            BuiltInCategory.INVALID            
        };
        public CategoriesInfo() 
        {
            categoriesInfo = new Dictionary<BuiltInCategory, List<Element>>();
            _elementsInfoList = new List<ElementsInfo>();
        }
        public bool hasElementWithName(BuiltInCategory category) => categoriesInfo.Any(el => el.Key == category);

        public List<Element> this[BuiltInCategory category] => categoriesInfo?.First(el => el.Key == category).Value;

        public void AddCategory(BuiltInCategory category, List<Element> elements)
        {
            categoriesInfo.Add(category, elements);
        }

        public Dictionary<BuiltInCategory, List<Element>> Get => categoriesInfo;

        public string GetInfo()
        {
            StringBuilder output = new StringBuilder( "" );
            foreach (var kv in categoriesInfo.OrderBy(x => x.Key.ToString()) )
            {
                if ( categoriesExcludedFromFilter.Contains( kv.Key )) 
                {
                    continue;
                }
                _elementsInfoList.Clear();
                ElementsInfo elInfo = null;
                output.AppendLine(new String('=', kv.Key.ToString().Length));
                output.AppendLine(kv.Key.ToString());
                output.AppendLine(new String('=', kv.Key.ToString().Length));
                output.AppendLine($"count: {kv.Value.Count}");
                
                foreach (Element e in kv.Value.OrderBy(x => x.Name)) //.Value.OrderBy(x=>x.Name))
                {
                    if (e.Category != null) //e.IsValidType( e.GetTypeId())
                    {
                        if (elInfo != null && elInfo.Name == e.Name)
                        {
                            elInfo.AddElement(e);
                        }
                        else
                        {
                            elInfo = ElementsInfo.Create(e);
                            _elementsInfoList.Add(elInfo);
                        }
                    }
                }
                foreach (ElementsInfo einfo in _elementsInfoList)
                {
                    try
                    {
                        output.AppendLine(einfo.GetInfo());
                    }
                    catch { }
                    //output.AppendLine(einfo.GetInfo());
                }
            }
            return output.ToString();
        }

        public string GetLevel<T>(T el) where T : Element
        {
            Level level = el.Document.GetElement(el.LevelId) as Level;
            return level?.Name;
        }
        public double GetComputedAreaOfElement<T>(T el) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(el.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble(), UnitTypeId.SquareMeters), 2);
        }
        public double GetComputedVolumeOfElement<T>(T e) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), UnitTypeId.CubicMeters), 2);
        }
    }
}
