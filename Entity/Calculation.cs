using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Autodesk.Revit.DB;
using Revitamin.Entity.json;

namespace Revitamin.Entity
{
    public static class Calculation
    {
        public static int UnitAccuracy = 3;
        public static double GetComputedVolumeOfElement<T>(T e) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), UnitTypeId.CubicMeters), UnitAccuracy);
        }
        public static string GetLevel<T>(T el) where T : Element
        {
            Level level = el.Document.GetElement(el.LevelId) as Level;
            return level?.Name;
        }
        public static double GetMass<T>(T e) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_STRUCTURAL_DENSITY).AsDouble(), UnitTypeId.KilogramsPerCubicMeter), UnitAccuracy) * GetComputedVolumeOfElement(e);
        }

        public static double GetFloorHeightAboveLevel<T>(T e) where T : Element
        {//Смещение от уровня
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble(), UnitTypeId.Meters), UnitAccuracy);
        }
        public static string[] getMaterials(Element e, Document document)
        {
            try
            {
                var matsIds = e.GetMaterialIds(false).ToList();
                string[] mats = new string[matsIds.Count];
                if (matsIds.Count > 0)
                {
                    for (int i = 0; i < matsIds.Count; i++)
                    {
                        mats[i] = document.GetElement(matsIds[i]).Name;
                    }
                }
                return mats;
            }
            catch
            {
                return new string[] { };
            }
        }

        public static List<JKVPair> GetMaterialsAllInfo(Element e, Document document)
        {
            List <JKVPair> matsList = new List <JKVPair>();
            try
            {
                var matsIds = e.GetMaterialIds(false).ToList();
                string[] mats = new string[matsIds.Count];
                if (matsIds.Count > 0)
                {
                    for (int i = 0; i < matsIds.Count; i++)
                    {
                        var material = document.GetElement(matsIds[i]);
                        List<JKVPair> matProperties = new List<JKVPair>();
                        PropertyInfo[] propertyInfos = material.GetType().GetProperties();
                        foreach (PropertyInfo propertyInfo in propertyInfos)
                        {
                            try
                            {
                                matProperties.Add( new JKVPair( propertyInfo.Name, propertyInfo.GetValue(material).ToString() ) );
                            }
                            catch { }
                        }
                        matsList.Add(new JKVPair(matsIds[i].ToString(), matProperties));
                    }
                }
                //return matsList;
            }
            catch
            {
                //return matsList;
            }
            return matsList;
        }

        public static double GetStructuralElevation<T>(T e) where T : Element
        {
            if (e.Category.BuiltInCategory == BuiltInCategory.OST_StructuralColumns)
            {
                return Elevation4StructuralColumns(e);
            }

            return ElevationBySTRUCTURAL_ELEVATION(e);
        }
        private static double ElevationBySTRUCTURAL_ELEVATION(Element e)
        {
            double top = Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.STRUCTURAL_ELEVATION_AT_TOP).AsDouble(), UnitTypeId.Meters), UnitAccuracy);
            double bottom = Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.STRUCTURAL_ELEVATION_AT_BOTTOM).AsDouble(), UnitTypeId.Meters), UnitAccuracy);
            double max = Math.Max(top, bottom);
            double min = Math.Min(top, bottom);

            return max - min;
        }
        private static double Elevation4StructuralColumns(Element e)
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.INSTANCE_LENGTH_PARAM).AsDouble(), UnitTypeId.Meters), UnitAccuracy);
        }



        public static double GetConvertedParameter<T>(T e, BuiltInParameter bip, ForgeTypeId typeId) where T : Element
        {
            double? value = e.get_Parameter(bip)?.AsDouble();
            return Math.Round(UnitUtils.ConvertFromInternalUnits(value ?? 0, typeId), 3);
        }

        public static double ConvertToMeters(double? number, int symbolsAfterPoint = 2)
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(number ?? 0, UnitTypeId.Meters), symbolsAfterPoint);
        }

        public static XYZ XYZByMeters<T>(T e) where T : XYZ
        {
            return new XYZ(
                Math.Round(UnitUtils.ConvertFromInternalUnits(e.X, UnitTypeId.Meters)),
                Math.Round(UnitUtils.ConvertFromInternalUnits(e.Y, UnitTypeId.Meters)),
                Math.Round(UnitUtils.ConvertFromInternalUnits(e.Z, UnitTypeId.Meters))
            );
        }
    }
}
