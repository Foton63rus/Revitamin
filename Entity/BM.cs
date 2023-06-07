using Autodesk.Revit.DB;
using System.Collections.Generic;
using System;
using Autodesk.Revit.UI;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;

namespace Revitamin.Entity
{
    public class BM //bim model
    {
        private Document document;
        string outputFilePath;
        public BM(Document document, string outputPath = "" )
        {
            this.document = document;
            this.outputFilePath = outputPath;
        }

        public string GetInfo()
        {
            StringBuilder outputMSG = new StringBuilder("");
            foreach (var builtInCategory in Enum.GetValues(typeof(BuiltInCategory)))
            {
                try
                {
                    List<Element> testFilter = new FilteredElementCollector(document).WhereElementIsNotElementType().
                    OfCategory((BuiltInCategory)builtInCategory).ToElements().ToList();
                    if (testFilter.Count > 0)
                    {
                        CategoryInfo catInfo = getCategoryInfo(testFilter, (BuiltInCategory)builtInCategory);
                        outputMSG.AppendLine(catInfo.GetInfo());
                        //MessageBox.Show(catInfo.GetInfo());
                    }
                }
                catch { }
            }
            if (outputFilePath != "")
            {
                File.WriteAllText(outputFilePath, outputMSG.ToString());
                MessageBox.Show(outputMSG.ToString(), "CategoryInfo");
            }
            return outputMSG.ToString();
        }

        public CategoryInfo getCategoryInfo(List<Element> elements, BuiltInCategory category)
        {
            CategoryInfo catInfo = new CategoryInfo(category);

            foreach (Element el in elements)
            {
                string elName = el.Name;
                if (catInfo.hasElementWithName(elName))
                {
                    ElementInfo ei = catInfo[elName];
                    ei.Area += getComputedAreaOfElement(el);
                    ei.Volume += getComputedAreaOfElement(el);
                    ei.Count += 1;
                }
                else
                {
                    ElementInfo ei = new ElementInfo(elName);
                    ei.Area = getComputedAreaOfElement(el);
                    ei.Volume = getComputedVolumeOfElement(el);
                    ei.Count = 1;

                    catInfo.AddElementInfo(ei);
                }
            }

            return catInfo;
        }

        public double getComputedAreaOfElement(Element e)
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble(), UnitTypeId.SquareMeters), 2);
        }
        public double getComputedVolumeOfElement(Element e)
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), UnitTypeId.CubicMeters), 2);
        }
    }
}
