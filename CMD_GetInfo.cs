using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Revitamin.Entity;

namespace Revitamin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CMD_GetInfo : IExternalCommand
    {
        private Specificator specificator;
        private UserWindow userWindow;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            string saveSpecificationPath = @"C:\Ruza_KR.txt";
            specificator = new Specificator(document, saveSpecificationPath);

            userWindow = new UserWindow(specificator);
            userWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}
