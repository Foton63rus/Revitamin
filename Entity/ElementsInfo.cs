using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Revitamin.Entity
{
    public class ElementsInfo
    {
        List<Element> elements = new List<Element>();
        double _totalArea = 0d;
        double _totalVolume = 0d;
        public static ElementsInfo Create(Element element)
        {
            try
            {
                ElementsInfo elementsInfo = new ElementsInfo();
                elementsInfo.elements.Add(element);
                elementsInfo._totalArea = elementsInfo.GetComputedAreaOfElement(element);
                elementsInfo._totalVolume = elementsInfo.GetComputedVolumeOfElement(element);
                return elementsInfo;
            }
            catch
            {
                return null;
            }
        }
        public bool AddElement(Element element)
        {
            try
            {
                elements.Add(element);
                _totalArea += GetComputedAreaOfElement(element);
                _totalVolume += GetComputedVolumeOfElement(element);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string GetInfo()
        {
            string level;
            Double area;
            Double volume;

            StringBuilder sb = new StringBuilder("");
            foreach (Element e in elements)
            {
                level = GetLevel(e);
                area = GetComputedAreaOfElement(e);
                volume = GetComputedVolumeOfElement(e);
                sb.AppendLine($"> id={e.Id}, Level={level}, S={area}m2, V={volume}m3");
            }
            string strElements = sb.ToString();
            if (strElements == "")
            {
                return "";
            }
            else
            {
                return $"{this.Name}, Count={this.Count}, GroupArea:{_totalArea}m2, GroupVolume:{_totalVolume}m3\n" + strElements;
            }
        }
        public void Clear()
        {
            elements.Clear();
            _totalArea = 0;
            _totalVolume = 0;
        }
        public string Name => elements[0].Name;
        public int Count => elements.Count;

        public string GetLevel<T>(T el) where T : Element
        {
            Level level = el.Document.GetElement(el.LevelId) as Level;
            return level.Name;
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
