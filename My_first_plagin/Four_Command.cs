using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;


namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    public class Four_Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilySymbol couchSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                                                                        .OfCategory(BuiltInCategory.OST_GenericModel)
                                                                        .Cast<FamilySymbol>()
                                                                        .First(it => it.FamilyName == "Семейство1" && it.Name == "111");
            XYZ couchLocation = XYZ.Zero;

            Level level = new FilteredElementCollector(doc).OfClass(typeof(Level)).FirstElement() as Level;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("ьбьбьбьбьбьбьбьбьб");

                if (!couchSymbol.IsActive)
                {
                    couchSymbol.Activate();
                }

                doc.Create.NewFamilyInstance(couchLocation, couchSymbol, level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                transaction.Commit();

            }

            return Result.Succeeded;

        }
    }
}