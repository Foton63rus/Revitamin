using Autodesk.Revit.DB;
using Revitamin.Entity.json;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Office.Interop.Excel;

namespace Revitamin.Entity
{
    public class Specificator
    {
        private Document _document;

        public Specificator(Document document, string outputPath = "")
        {
            this._document = document;
        }

        private string PrepareSpecificationToJson(List<Element> filter)
        {
            string filename = _document.PathName;
            string project = _document.ProjectInformation.Name;
            List<JsonElement> objects = new List<JsonElement>();
            JsonElement tmpObject = null;
            foreach (Element element in filter)
            {
                tmpObject = CreateJsonElement(element);
                if (tmpObject != null)
                {
                    objects.Add(tmpObject);
                }
            }
            JsonSpecification specification = new JsonSpecification(filename, objects.ToArray(), project);
            string JSON = JsonConvert.SerializeObject(specification, Formatting.Indented);
            return JSON;
        }
        public string GetJson()
        {
            List<Element> Filter = new FilteredElementCollector(_document).WhereElementIsNotElementType().ToList();
            string json = PrepareSpecificationToJson(Filter);

            string filename = _document.PathName;
            string project = _document.ProjectInformation.Name;
            List<JsonElement> objects = new List<JsonElement>();
            JsonElement tmpObject = null;
            foreach (Element element in Filter)
            {
                tmpObject = CreateJsonElement(element);
                if (tmpObject != null)
                {
                    objects.Add(tmpObject);
                }
            }
            JsonSpecification specification = new JsonSpecification(filename, objects.ToArray(), project);
            string JSON = JsonConvert.SerializeObject(specification, Formatting.Indented);
            return JSON;
        }

        private JsonElement CreateJsonElement(Element element)
        {
            try
            {
                JsonElement je = new JsonElement();

                je.category = element.Category.Name;
                je.name = element.Name;
                je.className = "";
                je.material = getMaterials(element);
                je.level = GetLevel(element);
                je.v = GetComputedVolumeOfElement(element);
                je.m = 0;//GetMass(element);
                je.s = GetComputedAreaOfElement(element);

                if (je.material.Length == 0 &&
                    je.level == null &&
                    je.v == 0 &&
                    je.m == 0 &&
                    je.s == 0)
                {
                    return null;
                }

                return je;
            }
            catch
            {
                return null;
            }
        }

        private string[] getMaterials(Element e)
        {
            try
            {
                var matsIds = e.GetMaterialIds(false).ToList();
                string[] mats = new string[matsIds.Count];
                if (matsIds.Count > 0)
                {
                    for (int i = 0; i < matsIds.Count; i++)
                    {
                        mats[i] = _document.GetElement(matsIds[i]).Name;
                    }
                }
                return mats;
            }
            catch
            {
                return new string[] { };
            }
        }

        private string GetLevel<T>(T el) where T : Element
        {
            Level level = el.Document.GetElement(el.LevelId) as Level;
            return level?.Name;
        }
        private double GetComputedAreaOfElement<T>(T el) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(el.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble(), UnitTypeId.SquareMeters), 2);
        }
        private double GetComputedVolumeOfElement<T>(T e) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), UnitTypeId.CubicMeters), 2);
        }
        private double GetMass<T>(T e) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_STRUCTURAL_DENSITY).AsDouble(), UnitTypeId.KilogramsPerCubicMeter), 2) * GetComputedVolumeOfElement(e);
        }
    }
}
