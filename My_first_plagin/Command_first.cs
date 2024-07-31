using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    public class Command_first : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Привет!", "Привет мир!");
            //TaskDialogButton.Show(this, message);
            return Result.Succeeded;

        }
    }
}
