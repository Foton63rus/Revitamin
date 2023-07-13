using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Revitamin.Entity
{
    public static class Calculation
    {
        public static double GetComputedAreaOfElement<T>(T el) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(el.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble(), UnitTypeId.SquareMeters), 2);
        }
        public static double GetComputedVolumeOfElement<T>(T e) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), UnitTypeId.CubicMeters), 2);
        }
        public static string GetLevel<T>(T el) where T : Element
        {
            Level level = el.Document.GetElement(el.LevelId) as Level;
            return level?.Name;
        }
        public static double GetMass<T>(T e) where T : Element
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_STRUCTURAL_DENSITY).AsDouble(), UnitTypeId.KilogramsPerCubicMeter), 2) * GetComputedVolumeOfElement(e);
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
            double top = Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.STRUCTURAL_ELEVATION_AT_TOP).AsDouble(), UnitTypeId.Meters), 2);
            double bottom = Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.STRUCTURAL_ELEVATION_AT_BOTTOM).AsDouble(), UnitTypeId.Meters), 2);
            double max = Math.Max(top, bottom);
            double min = Math.Min(top, bottom);

            return max - min;
        }
        private static double Elevation4StructuralColumns(Element e)
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(e.get_Parameter(BuiltInParameter.INSTANCE_LENGTH_PARAM).AsDouble(), UnitTypeId.Meters), 2);
        }
        public static double ConvertToMeters(double number, int symbolsAfterPoint = 2)
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(number, UnitTypeId.Meters), symbolsAfterPoint);
        }
    }
}
