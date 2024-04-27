using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Revitamin.Entity.json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System;
//using Autodesk.Revit.Creation;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Office.Interop.Excel;
using Autodesk.Revit.DB.Structure;

namespace Revitamin.Entity
{
    public class Specificator
    {
        private Document _document;
        public Specificator(Document document, string outputPath = "")
        {
            this._document = document;
        }
        public string GetJson()
        {
            List<Element> Filter = new FilteredElementCollector(_document).WhereElementIsNotElementType().ToList();
            string project_id = "1";//_document.GetProjectId();
            string project_key = "key";//_document.ProjectInformation.Name.ToString();
            string project_version = _document.ProjectInformation.VersionGuid.ToString();
            string app_version = GlobalVariables.CommandData.Application.Application.VersionNumber.ToString();
            string filename = _document.PathName;
            string project = _document.ProjectInformation.Name;
            List<ElementJsonView> objects = new List<ElementJsonView>();
            ElementJsonView tmpObject = null;
            foreach (Element element in Filter)
            {
                tmpObject = null;//CreateJsonElement(element);
                if (tmpObject != null)
                {
                    objects.Add(tmpObject);
                }
            }
            JsonSpecification specification = new JsonSpecification(app_version, project_id, project_key, project_version, filename, objects.ToArray(), project);
            string JSON = JsonConvert.SerializeObject(specification, Formatting.Indented);
            return JSON;
        }

        public JsonElement CreateJsonElement(Element element)
        {
            try
            {
                JsonElement je = new JsonElement();

                je.object_id = element.Id.IntegerValue;
                je.category = element.Category.Name;
                try { je.name = element.Name; } catch { }
                try { je.class_name = _document?.GetElement(element.GetTypeId())
                    ?.get_Parameter(BuiltInParameter.UNIFORMAT_CODE)
                    ?.AsString() ?? "Undefined"; } catch { }
                try { je.material = Calculation.getMaterials(element, _document); } catch { }
                try { je.level = Calculation.GetLevel(element); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "id", value: element.Id.ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "group_id", value: element.GroupId.ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "volume", value: Calculation.GetConvertedParameter(element, BuiltInParameter.HOST_VOLUME_COMPUTED, UnitTypeId.CubicMeters).ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "area", value: Calculation.GetConvertedParameter(element, BuiltInParameter.HOST_AREA_COMPUTED, UnitTypeId.SquareMeters).ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(name: "boundingboxxyz_origin", value: element.get_BoundingBox(_document.ActiveView)?.Transform.Origin.ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(name: "boundingboxxyz_scale", value: element.get_BoundingBox(_document.ActiveView)?.Transform.Scale.ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "boundingboxxyz_min", value: Calculation.XYZByMeters(element.get_BoundingBox(_document.ActiveView)?.Min).ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "boundingboxxyz_max", value: Calculation.XYZByMeters(element.get_BoundingBox(_document.ActiveView)?.Max).ToString())); } catch { }
                try { Options opt = new Options();
                    opt.DetailLevel = ViewDetailLevel.Coarse;
                    foreach (var wall in element.get_Geometry(opt))
                    {
                        je.parameters.Add(new JKVPair(
                        name: "ComputeCentroid", value: (wall as Solid)?.ComputeCentroid().ToString()));
                    }
                } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "LocationPoint", value: Calculation.XYZByMeters(((LocationPoint)element.Location)?.Point).ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "Length", value: Calculation.ConvertToMeters(element.LookupParameter("Длина")?.AsDouble()).ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "Width", value: Calculation.ConvertToMeters(element.LookupParameter("ADSK_Размер_Ширина")?.AsDouble()).ToString())); } catch { }
                try { je.parameters.Add(new JKVPair(
                    name: "Heigth", value: Calculation.ConvertToMeters(element.LookupParameter("ADSK_Размер_Высота")?.AsDouble()).ToString())); } catch { }
                try{ je.parameters.Add(new JKVPair(
                    name: "LocationPoint", value: Calculation.GetStructuralElevation(element).ToString()));} catch { }
                /*

                je.parameters.Add(new ElementParameter(
                    name: "FloorHeightAboveLevel", value: Calculation.GetConvertedParameter(element, BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM, UnitTypeId.Meters).ToString()));
                je.parameters.Add(new ElementParameter(
                    name: "ElevationAtTop", value: Calculation.GetConvertedParameter(element, BuiltInParameter.STRUCTURAL_ELEVATION_AT_TOP, UnitTypeId.Meters).ToString()));
                je.parameters.Add(new ElementParameter(
                    name: "ElevationAtBottom", value: Calculation.GetConvertedParameter(element, BuiltInParameter.STRUCTURAL_ELEVATION_AT_BOTTOM, UnitTypeId.Meters).ToString()));

                //var test = element.get_Parameter(BuiltInParameter.Name);
                //je.parameters.Add(new ElementParameter(name: "Test", value: test?.ToString()));

                //foreach (var builtInParameter in System.Enum.GetValues(typeof(BuiltInParameter)))
                //{
                //    je.parameters.Add(new ElementParameter(name: builtInParameter.ToString(), value: builtInParameter.GetType().ToString() ));
                //}

                if (je.material.Length == 0 &&
                    je.level == null &&
                    je.parameters.Any(x => x.name == "volume" && x.value == "0") &&
                    je.parameters.Any(x => x.name == "area" && x.value == "0"))
                {
                    return null;
                }*/

                return je;
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        public string CreateJsonView()
        {
            List<Element> Filter = new FilteredElementCollector(_document).WhereElementIsNotElementType().ToList();
            string project_id = "1";//_document.GetProjectId();
            string project_key = "key";//_document.ProjectInformation.Name.ToString();
            string project_version = _document.ProjectInformation.VersionGuid.ToString();
            string app_version = GlobalVariables.CommandData.Application.Application.VersionNumber.ToString();
            string filename = _document.PathName;
            string project = _document.ProjectInformation.Name;
            List<ElementJsonView> objects = new List<ElementJsonView>();
            ElementJsonView tmpObject = null;
            foreach (Element element in Filter)
            {
                tmpObject = CreateElementJsonView(element);
                if (tmpObject != null)
                {
                    objects.Add(tmpObject);
                }
            }
            JsonSpecification specification = new JsonSpecification(app_version, project_id, project_key, project_version, filename, objects.ToArray(), project);
            string JSON = JsonConvert.SerializeObject(specification, Formatting.Indented);
            return JSON;
        }

        public ElementJsonView CreateElementJsonView( Element selected)
        {
            PropertyInfo[] propertyInfos = selected.GetType().GetProperties();

            List<JKVPair> propertiesKVs = new List<JKVPair>();
            List<JKVPair> parametersKVs = new List<JKVPair>();
            List<JKVPair> geometryKVs = new List<JKVPair>();
            List<JKVPair> bBoxKVs = new List<JKVPair>();
            List<JKVPair> materialsKVs = Calculation.GetMaterialsAllInfo(selected, _document);

            if (materialsKVs.Count == 0)
            {
                return null;
            }

            ElementJsonView ejw = new ElementJsonView();

            ejw.object_id = selected.Id.IntegerValue;
            ejw.category = selected.Category.Name;
            ejw.name = selected.Name;
            ejw.class_name = selected.get_Parameter(BuiltInParameter.UNIFORMAT_CODE)?.AsString() ?? "Undefined";
            try { ejw.level = Calculation.GetLevel(selected); } catch { }

        /*Dictionary<string, object> bipMap = new Dictionary<string, object>();
        foreach (string name in Enum.GetNames(typeof(BuiltInParameter)))
        {
            bipMap.Add(name, Enum.Parse(typeof(BuiltInParameter), name));
        }*/

            foreach (var propertyInfo in propertyInfos)
            {
                try
                {
                    switch (propertyInfo.Name)
                    {
                        case "Category":
                            Category category = propertyInfo.GetValue(selected) as Category;
                            parametersKVs.Add(new JKVPair(
                                propertyInfo.Name,
                                new JKVPair(category.Name, category.BuiltInCategory.ToString())));
                            break;

                        case "Parameters":
                            foreach (string bipName in Enum.GetNames(typeof(BuiltInParameter)))
                            {
                                try
                                {
                                    var parameter = selected.get_Parameter((BuiltInParameter)Enum.Parse(typeof(BuiltInParameter), bipName));
                                    string strValue = parameter.AsValueString().Trim();
                                    if (parameter.HasValue && strValue != "" && strValue != null)
                                    {
                                        parametersKVs.Add(new JKVPair(bipName, strValue));
                                    }
                                }
                                catch { }
                            }
                            break;

                        case "BoundingBox":
                            BoundingBoxXYZ bbox = selected.get_BoundingBox(_document.ActiveView);
                            PropertyInfo[] bBoxPropertyInfos = bbox.GetType().GetProperties();
                            foreach (PropertyInfo bBoxPropertyInfo in bBoxPropertyInfos)
                            {
                                try
                                {
                                    if (bBoxPropertyInfo.Name == nameof(Transform))
                                    {
                                        Transform tr = selected.get_BoundingBox(_document.ActiveView)?.Transform;
                                        List<JKVPair> trKVs = new List<JKVPair>();
                                        PropertyInfo[] transformPropertyInfos = tr.GetType().GetProperties();
                                        foreach (PropertyInfo transformPropertyInfo in transformPropertyInfos)
                                        {
                                            try
                                            {
                                                trKVs.Add(new JKVPair(transformPropertyInfo.Name, transformPropertyInfo.GetValue(tr).ToString()));
                                            }
                                            catch { }
                                        }
                                        bBoxKVs.Add(new JKVPair(bBoxPropertyInfo.Name, trKVs.ToArray()));
                                    }
                                    else
                                    {
                                        bBoxKVs.Add(new JKVPair(bBoxPropertyInfo.Name, bBoxPropertyInfo.GetValue(bbox).ToString()));
                                    }
                                }
                                catch {  }
                            }
                            break;

                        case "Geometry":

                            Options optGeom = new Options();
                            optGeom.DetailLevel = ViewDetailLevel.Fine;

                            foreach (var elem in selected.get_Geometry(optGeom))
                            {
                                geometryKVs.Add(new JKVPair( name: "ComputeCentroid", value: (elem as Solid)?.ComputeCentroid().ToString()));
                                PropertyInfo[] geometryPropertyInfos = elem.GetType().GetProperties();
                                foreach (var geometryPropertyInfo in geometryPropertyInfos)
                                {
                                    try
                                    {
                                        if (geometryPropertyInfo.Name == nameof(Transform))
                                        {
                                            Transform tr = geometryPropertyInfo.GetValue(elem) as Transform;
                                            PropertyInfo[] transformPropertyInfos = tr.GetType().GetProperties();
                                            foreach (var transformPropertyInfo in transformPropertyInfos)
                                            {
                                                try
                                                {
                                                    geometryKVs.Add(new JKVPair(transformPropertyInfo.Name, transformPropertyInfo.GetValue(tr).ToString()));
                                                }
                                                catch
                                                {
                                                    geometryKVs.Add(new JKVPair("transform_parametr", "error"));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            geometryKVs.Add(new JKVPair(geometryPropertyInfo.Name, geometryPropertyInfo.GetValue(elem).ToString()));
                                        }
                                    }
                                    catch
                                    {
                                        geometryKVs.Add(new JKVPair($"{geometryPropertyInfo.Name}", "error"));
                                    }
                                }
                            }
                            break;

                        default:
                            propertiesKVs.Add(new JKVPair(propertyInfo.Name, propertyInfo.GetValue(selected).ToString()));
                            break;
                    }
                }
                catch
                {
                    //sb.AppendLine($"{propertyInfo.Name}");
                }
            }


            parametersKVs.AddRange(propertiesKVs);
            parametersKVs.AddRange(geometryKVs);
            parametersKVs.AddRange(bBoxKVs);

            ejw.parameters = parametersKVs.ToArray();

            //ejw.Properties = propertiesKVs.ToArray();

            //ejw.Geometry = geometryKVs.ToArray();

            //ejw.BoundingBox = bBoxKVs.ToArray();

            ejw.material = materialsKVs.ToArray();

            string json = JsonConvert.SerializeObject(ejw);
            return ejw;
        } 
    }
}
