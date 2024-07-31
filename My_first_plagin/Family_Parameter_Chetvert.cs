using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    public class Family : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            ISelectionFilter selFilter = new MassSelectionFilter();




            //FamilyParameter couchSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilyParameter))
            //                                                            .OfCategory(BuiltInCategory.OST_GenericModel)
            //                                                            .Cast<FamilyParameter>()
            //                                                            .First(it => it.Ge == "Семейство1" && it.Name == "111");

            //FamilyManager manager = doc.FamilyManager;
            //IList<FamilyParameter> test = manager.GetParameters();
            //string couchSymbol = new FilteredElementCollector(test).Cast<FamilyParameter>().First(it => it.Ge == "Семейство1" && it.Name == "111");
            //string ppp = manager.Parameters.Cast<FamilyParameter>().First().ToString();
            IEnumerable<ParameterElement> ppp22 = new FilteredElementCollector(doc).OfClass(typeof(ParameterElement))
                                                                        .Cast<ParameterElement>();
                                                                        
            TaskDialog.Show("Привет!", ppp22 + "333");
            //TaskDialogButton.Show(this, message);
            return Result.Succeeded;

        }


        public class MassSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element element)
            {
                if (element.Category.Name == "Обобщенные модели")
                {
                    return true;
                }
                return false;
            }

            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }
    }
}
