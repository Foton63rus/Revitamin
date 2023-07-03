using Autodesk.Revit.DB;
using Revitamin.Entity.json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
                je.name = element.Name + $" = {Calculation.GetStructuralElevation(element)}";
                je.className = _document.GetElement(element.GetTypeId())?
                                  .get_Parameter(BuiltInParameter.UNIFORMAT_CODE)?
                                  .AsString() ?? "Undefined";
                je.material = Calculation.getMaterials(element, _document);
                je.level = Calculation.GetLevel(element);
                je.v = Calculation.GetComputedVolumeOfElement(element);
                je.m = 0;//Calculation.GetMass(element);
                je.s = Calculation.GetComputedAreaOfElement(element);

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
    }
}
