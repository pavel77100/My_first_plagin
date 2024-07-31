using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    public class Five_Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilySymbol doorSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                         .OfCategory(BuiltInCategory.OST_Doors)
                                                                         .Cast<FamilySymbol>()
                                                                         .First(it => it.FamilyName == "Одиночные-Щитовые" && it.Name == "0915 x 2134 мм");

            IEnumerable<Element> walls = new FilteredElementCollector(doc).OfClass(typeof(Wall)).ToElements();

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("111111!");

                if (!doorSymbol.IsActive)
                {
                    doorSymbol.Activate();
                }

                foreach (var wall in walls)
                {
                    Curve wallCurve = (wall.Location as LocationCurve).Curve;

                    XYZ wallCenter = wallCurve.Evaluate(0.5, true);

                    doc.Create.NewFamilyInstance(wallCenter, doorSymbol, wall, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                }

                transaction.Commit();
            }

            return Result.Succeeded;

        }
    }
}