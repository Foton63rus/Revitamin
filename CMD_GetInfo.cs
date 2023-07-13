using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Revitamin.Entity;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Revitamin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CMD_GetInfo : IExternalCommand
    {
        public static GlobalVariables GLOBAL_VARIABLES;
        public static UserWindow userWindow;
        private static Document document = null;
        private Specificator specificator;
        public static IChecker checker;
        public static Document Document
        {
            get
            {
                return document;
            }
            set
            {
                document = value;
            } 
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            InitVariables();

            Document = commandData.Application.ActiveUIDocument.Document;
            string saveSpecificationPath = @"C:\Ruza_KR.txt";
            specificator = new Specificator(document, saveSpecificationPath);
            InitSystems();

            userWindow = new UserWindow(specificator);
            userWindow.ShowDialog();

            
            return Result.Succeeded;
        }
        private void InitVariables()
        {
            GLOBAL_VARIABLES = new GlobalVariables();
            GLOBAL_VARIABLES.BuiltInCategories = getBuiltInCategories();
        }
        private Dictionary<string, BuiltInCategory> getBuiltInCategories()
        {
            Dictionary<string, BuiltInCategory> tmpDict = new Dictionary<string, BuiltInCategory>();
            foreach (BuiltInCategory category in Enum.GetValues(typeof(BuiltInCategory)))
            {
                tmpDict.Add(category.ToString(), category);
            }
            tmpDict.OrderBy(x => x.Key);
            return tmpDict;
        }
        private void InitSystems()
        {
            checker = new Checker( Document );
        }
    }
}
