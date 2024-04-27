using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace Revitamin.Entity
{
    public class GlobalVariables
    {
        public static ExternalCommandData CommandData;
        public Dictionary<string, BuiltInCategory> BuiltInCategories = new Dictionary<string, BuiltInCategory>();
    }
}
