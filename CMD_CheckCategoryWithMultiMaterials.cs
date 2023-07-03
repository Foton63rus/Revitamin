using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;

namespace Revitamin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class CMD_CheckCategoryWithMultiMaterials //: IExternalCommand
    {
        private UserWindow userWindow;
    }
}
