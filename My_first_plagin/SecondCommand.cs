using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static System.Math;

namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    public class SecondCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            CurveArray curves = new CurveArray();

            curves.Append(Arc.Create(XYZ.Zero, 10, 0, 2 * PI, XYZ.BasisX, XYZ.BasisY));

            curves.Append(Arc.Create(XYZ.Zero, 8, 0, 2 * PI, XYZ.BasisX, XYZ.BasisY));

            curves.Append(Arc.Create(new XYZ(-8, 0, 0), new XYZ(-2, 3, 0), new XYZ(0, 3, 0)));

            View activeView = doc.ActiveView;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Гооооооооол!!!!");

                doc.Create.NewDetailCurveArray(activeView, curves);

                transaction.Commit();
            }

            return Result.Succeeded;

        }
    }
}
