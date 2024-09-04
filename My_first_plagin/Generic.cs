using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace My_first_plagin
{




    public class Rectangle
    {  
        public double xLenght { get;  set; }
        public double yLenght { get;  set; }

        public double Square{ 
            get { return  xLenght*yLenght; } 
        }
        //public double Square { get; private set; }  
    }

    public class House : Rectangle 
    { 
        public XYZ startPoint { get; private set; }
        public House(XYZ stPoint) { }
    }

    [Transaction(TransactionMode.Manual)]
    internal class Generic : IExternalCommand
    {


        public class RectangleHouse
        {
            public int baceCoordinate
            {
                get;
                private set;
            }
            
            public RectangleHouse(List<CurveElement> curves) 
            { 
            }
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            View activeView = doc.ActiveView;

            List<CurveElement> curves = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Lines)
                                                                         .Cast<CurveElement>()
                                                                         .Where(it => it.CurveElementType == CurveElementType.DetailCurve & it.OwnerViewId == activeView.Id)
                                                                         .ToList();
            //double xMin = curves[0].CreateBound();
            string ttt = "";
            IList<XYZ> rectangleVertexes = new List<XYZ>();
            double zBaced = curves[0].GeometryCurve.GetEndPoint(0).Z;
            double XMinRectangle = curves[0].GeometryCurve.GetEndPoint(0).X;        
            double YMinRectangle = curves[0].GeometryCurve.GetEndPoint(0).Y;
            double XMaxRectangle = curves[0].GeometryCurve.GetEndPoint(0).X;
            double YMaxRectangle = curves[0].GeometryCurve.GetEndPoint(0).Y;

            foreach (CurveElement curve in curves)
            {
                if(curve.GeometryCurve.GetEndPoint(0).X > XMaxRectangle)
                {
                    XMaxRectangle = curve.GeometryCurve.GetEndPoint(0).X;
                }

                if (curve.GeometryCurve.GetEndPoint(0).X < XMinRectangle)
                {
                    XMinRectangle = curve.GeometryCurve.GetEndPoint(0).X;
                }

                if (curve.GeometryCurve.GetEndPoint(0).Y > YMaxRectangle)
                {
                    YMaxRectangle = curve.GeometryCurve.GetEndPoint(0).Y;
                }

                if (curve.GeometryCurve.GetEndPoint(0).Y < YMinRectangle)
                {
                    YMinRectangle = curve.GeometryCurve.GetEndPoint(0).Y;
                }
            }

            XYZ minRec = new XYZ(XMinRectangle, YMinRectangle, zBaced);
            XYZ maxRec = new XYZ(XMaxRectangle, YMaxRectangle, zBaced);
            TaskDialog.Show("Ответ", XMaxRectangle.ToString()+"\n"+ XMinRectangle.ToString()+ "\n"+ YMaxRectangle.ToString() + "\n"+ YMinRectangle.ToString() );

            return Result.Succeeded;
        }

        
    }

    
}
