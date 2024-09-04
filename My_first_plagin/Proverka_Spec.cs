using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace My_first_plagin
{


    [Transaction(TransactionMode.Manual)]
    internal class Proverka_Spec : IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Autodesk.Revit.DB.Document doc = uiDoc.Document;

            SketchPlane PlaneForExtr = new FilteredElementCollector(doc).OfClass(typeof(SketchPlane)).Where(it => it.Name.Contains("Опорный")).First() as SketchPlane;

            TaskDialog.Show("ffff", PlaneForExtr.ToString());

            XYZ centre = new XYZ(5, 5, 0);

            double radius = 2;

            CurveArrArray curveArrArray = new CurveArrArray();

            CurveArray curveArray = new CurveArray();

            //var circle = Arc.Create(centre, radius, 0, ConvertDegreesToRadians(360), XYZ.BasisX, XYZ.BasisY);

            CurveLoop cl = new CurveLoop();

            cl.Append(Arc.Create(centre, radius, 0, ConvertDegreesToRadians(180), XYZ.BasisX, XYZ.BasisY));
            cl.Append(Arc.Create(centre, radius, ConvertDegreesToRadians(180), ConvertDegreesToRadians(360), XYZ.BasisX, XYZ.BasisY));

            IList<CurveLoop> myIList = new List<CurveLoop>();

            myIList.Add(cl);

            //Curve curve = circle as Curve;

           // Il

            //curveArray.Append(circle);

            //curveArrArray.Append(curveArray);

            int n = 4;

            



            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Создание круга");

                //Extrusion extrusion = doc.FamilyCreate.NewExtrusion(true, curveArrArray, PlaneForExtr, 0.078740157480314987);

                Solid sol = GeometryCreationUtilities.CreateExtrusionGeometry(myIList, centre, 3);

                //doc.Regenerate();
                transaction.Commit();
            }


            // List<CurveElement> curves = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Lines)
            //     .Cast<CurveElement>()
            //     .Where(it => it.CurveElementType == CurveElementType.DetailCurve)
            //         .ToList();
            /*
                       // IList<Element> errorObob = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_DuctCurves)
                                                                                     .Cast<Element>().Where(it => it.Bou == CurveElementType.DetailCurve).ToList();

                        String errors = "";

                        foreach(Element elem in errorObob)
                        {
                            //String Parameter_select = elem.GetParameters("ББББ").Count.ToString();
                            String Parameter_select = elem.Id.ToString();
                            errors += " " + Parameter_select;
                        }
                        TaskDialog.Show("1", errors);
            */
            return Result.Succeeded;
        }
        public static double ConvertDegreesToRadians(double degrees)
        {
            return ((Math.PI / 180) * degrees);
        }

        const double _mm_to_foot = 1 / 304.8;
        public double MmToFoot(double length_in_mm)
        {
            return _mm_to_foot * length_in_mm;
        }

    }
}
