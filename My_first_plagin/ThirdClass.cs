using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    public class ThirdClass : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            List<CurveElement> curves = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Lines)
                                                                         .Cast<CurveElement>()
                                                                         .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
                                                                         .ToList();


            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Гооооооооол2!!!!");

                foreach (CurveElement curve in curves)
                {
                    doc.Delete(curve.Id);
                }

                transaction.Commit();
            }

            return Result.Succeeded;

        }
    }
}