using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Revitamin.Entity;
using System.Windows;
using Newtonsoft.Json;

namespace Revitamin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CMD_GetSelectionInfo : IExternalCommand
    {

        private Specificator specificator;
        private UserWindow userWindow;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            specificator = new Specificator(document);
            var eId = commandData.Application.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select element or ESC to reset the view");
            Element selected = document.GetElement(eId);

            string json = JsonConvert.SerializeObject( specificator.CreateElementJsonView(selected) );
            MessageBox.Show($"props: {json}");

            return Result.Succeeded;
        }
    } 
}
