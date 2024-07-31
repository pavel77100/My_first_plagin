using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace My_first_plagin
{

    [Transaction(TransactionMode.Manual)]
    internal class Detect_Boarder_Mark : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Autodesk.Revit.DB.Document doc = uiDoc.Document;

            ReferenceArray ra = new ReferenceArray();

            ISelectionFilter selFilter = new MassSelectionFilter();

            View act_view = doc.ActiveView;

            IList<Element> eList = uiDoc.Selection.PickElementsByRectangle(selFilter,
                    "Выберите марки труб") as IList<Element>;


            var mess = "";
            IList<XYZ> dots = new List<XYZ>();

            IList<Line> lines= new List<Line>();

            foreach (Element e in eList)
            {

                
                BoundingBoxXYZ b_b = e.get_BoundingBox(doc.ActiveView);

                XYZ min_coord = b_b.Min;

                XYZ max_coord = b_b.Max;

                Line line_s_e = Line.CreateBound(min_coord, max_coord);

                lines.Add(line_s_e);

            }


            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Рисуем линии");

                foreach (Line l in lines)
                {
                    doc.Create.NewDetailCurve(act_view, l);
                }

                transaction.Commit();

            }
            
            TaskDialog.Show("Тип", mess);

            return Result.Succeeded;
        }

        public class MassSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element element)
            {
                // if (element.Category.Name == "Марки труб"|| element.Category.Name == "Марки оборудования"|| element.Category.Name == "Марки арматуры трубопроводов")
                if (true)
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
