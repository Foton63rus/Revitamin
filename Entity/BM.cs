using Autodesk.Revit.DB;
using System.Collections.Generic;
using System;
using Autodesk.Revit.UI;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Xml.Linq;

namespace Revitamin.Entity
{
    public class BM //bim model
    {
        private Document document;
        private Categories categories;
        string outputFilePath;
        BuiltInCategory[] categoriesWithMaterialsOutput = new BuiltInCategory[] { 
            BuiltInCategory.OST_Walls,
            BuiltInCategory.OST_Floors,
            BuiltInCategory.OST_Ceilings};
        public BM(Document document, string outputPath = "" )
        {
            this.document = document;
            this.outputFilePath = outputPath;
        }

        public string GetInfo()
        {
            StringBuilder outputMSG = new StringBuilder("");
            foreach (BuiltInCategory builtInCategory in Enum.GetValues(typeof(BuiltInCategory)))
            {
                try
                {
                    List<Element> catFilter = new FilteredElementCollector(document).WhereElementIsNotElementType().
                    OfCategory(builtInCategory).ToElements().ToList();
                    if (catFilter.Count > 0)
                    {
                        if (categoriesWithMaterialsOutput.Contains(builtInCategory))
                        {
                            outputMSG.AppendLine(builtInCategory.ToString());
                            foreach (Element wall in catFilter)
                            {
                                outputMSG.AppendLine($"{wall.Name} S={getComputedAreaOfElement(wall)}m2 V={getComputedVolumeOfElement(wall)}m3");
                                outputMSG.AppendLine(getMaterials(wall));
                            }
                        }
                        else
                        {
                            CategoryInfo catInfo = getCategoryInfo(catFilter, (BuiltInCategory)builtInCategory);
                            outputMSG.AppendLine(catInfo.GetInfo());
                        }
                    }
                }
                catch { }
            }
            if (outputFilePath != "")
            {
                File.WriteAllText(outputFilePath, outputMSG.ToString());
                MessageBox.Show("CategoryInfo");
            }
            return outputMSG.ToString();
        }

        public string GetInfo2() //чекнуть классы с материалами больше чем 1
        {
            StringBuilder outputMSG = new StringBuilder("чекнуть классы с материалами больше чем 1\n\n");
            foreach (var builtInCategory in Enum.GetValues(typeof(BuiltInCategory)))
            {
                try
                {
                    List<Element> testFilter = new FilteredElementCollector(document).WhereElementIsNotElementType().
                    OfCategory((BuiltInCategory)builtInCategory).ToElements().ToList();
                    if (testFilter.Count > 0)
                    {
                        foreach (var element in testFilter)
                        {
                            if (element.GetMaterialIds(false).Count > 1)
                            {
                                outputMSG.AppendLine($"{builtInCategory.ToString()}");
                                break;
                            }
                        }
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

        public string getWallInfo()
        {
            StringBuilder outputMSG = new StringBuilder("");
            List<Element> walls = new FilteredElementCollector(document).WhereElementIsNotElementType().
                    OfCategory(BuiltInCategory.OST_GenericModel).ToElements().ToList();
            foreach (Element wall in walls)
            {
                Level level = wall.Document.GetElement(wall.LevelId) as Level;
                outputMSG.AppendLine($"{wall.Name} LVL={level.Name} S={getComputedAreaOfElement(wall)}m2 V={getComputedVolumeOfElement(wall)}m3");
                outputMSG.AppendLine(getMaterials(wall));
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
                    ElementGroupInfo ei = catInfo[elName];
                    ei.Area += getComputedAreaOfElement(el);
                    ei.Volume += getComputedAreaOfElement(el);
                    ei.Count += 1;
                }
                else
                {
                    ElementGroupInfo ei = new ElementGroupInfo(elName);
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
        public string getMaterials(Element e)
        {
            try
            {
                var mats = e.GetMaterialIds(false);
                StringBuilder matstring = new StringBuilder("");
                if (mats.Count > 0)
                {
                    foreach (var mat in mats)
                    {
                        //var area = Math.Round(UnitUtils.ConvertFromInternalUnits(e.GetMaterialArea(mat, false), UnitTypeId.SquareMeters), 2);
                        //var volume = Math.Round(UnitUtils.ConvertFromInternalUnits(e.GetMaterialVolume(mat), UnitTypeId.CubicMeters), 2);
                        matstring.AppendLine($"===>{document.GetElement(mat).Name} S={getComputedAreaOfElement(e)}m2 V={getComputedVolumeOfElement(e)}m3");
                    }
                }
                return matstring.ToString();
            }
            catch { return ""; }
        }
    }
}
