using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace My_first_plagin
{
    [Transaction(TransactionMode.Manual)]
    internal class CreateLines : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            doc.PrintManager.SelectNewPrintDriver("Foxit PhantomPDF Printer");

            PrintManager pm = doc.PrintManager;

            View activeView = doc.ActiveView;

            //pm.PrintRange = PrintRange.CurrentView;

            pm.PrintToFile = true;


            pm.PrintToFileName = "1111111111111.pdf";

            PaperSizeSet myPSize = pm.PaperSizes;

            string tt = doc.PrintManager.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSize.ToString();


            TaskDialog.Show("PaperSize.Name", tt);
            //

            //var vss = pm.ViewSheetSetting;


            //
            IList<UIView> list_of_ui = uiDoc.GetOpenUIViews();
            
            ElementId idActiveView = activeView.Id;

            UIView aciualUIView = list_of_ui.Cast<UIView>().First(it => it.ViewId == idActiveView);

            IList<XYZ> CoordinatesOfAngle = aciualUIView.GetZoomCorners();

            Line geomLine = Line.CreateBound(CoordinatesOfAngle[0], CoordinatesOfAngle[1]);

            int ScaleList = activeView.Scale;

            double XListSize = (CoordinatesOfAngle[1].X - CoordinatesOfAngle[0].X) / ScaleList * 304.8;

            double YListSize = (CoordinatesOfAngle[1].Y - CoordinatesOfAngle[0].Y) / ScaleList * 304.8;

            double XYKoef = XListSize / YListSize;

            if (XListSize > 0)
            {

            }

            string ResultString = "";

            foreach(XYZ coord in CoordinatesOfAngle)
            {
                ResultString += coord.ToString() + "\n";
            }
            ResultString += aciualUIView.ViewId + "\n";

            TaskDialog.Show("Ответ", ResultString);

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Гооооооооол!!!!");

                TaskDialog.Show("Папир сайз", ResultString);

                doc.Create.NewDetailCurve(activeView, geomLine);

                pm.PrintToFile = true;

                //PrintParameters pp = new PrintParameters();

                pm.PrintToFileName = "C:\\Users\\user187\\Downloads\\Проект244e33ee5533.pdf";

                pm.SubmitPrint(activeView);
                    
                    

                transaction.Commit();
            }
  
            return Result.Succeeded;
        }
       
    }
}
