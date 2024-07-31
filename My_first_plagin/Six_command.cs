using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    public class Six_command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            ReferenceArray ra = new ReferenceArray();

            ISelectionFilter selFilter = new MassSelectionFilter();

            IList<Element> eList = uiDoc.Selection.PickElementsByRectangle(selFilter,
                    "Выберите обобщенные модели") as IList<Element>;

            Int32 coutn_of_array = eList.Count();

            String value_for_viveska;

            if (coutn_of_array > 0)
            {
                value_for_viveska = "Вcего задан список параметров:";

                foreach (Element elem in eList)
                {
                    String Parameter_select = elem.GetParameters("Параметр экз")[0].AsString();

                    value_for_viveska += " " + Parameter_select;
                }

                TaskDialog.Show("Успех!", value_for_viveska);
            }

            else
            {
                TaskDialog.Show("Пусто", "Ничего не выделено");
            }


            //Element elementarray = eList[0];

            //IList<Parameter> listuuu = elementarray.GetParameters("Параметр экз");

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
