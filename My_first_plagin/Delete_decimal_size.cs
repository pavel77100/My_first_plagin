using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace My_first_plagin


{

    [Transaction(TransactionMode.Manual)]
    public class Delete_decimal_size : IExternalCommand
    {

        public bool Bool_test(string element)
        {
            bool otvet = new bool();

            try
            {
                if (element.Contains("половина"))
                {
                    otvet = true;
                }
                else
                {
                    otvet = false;
                }
            }
            finally
            {
                otvet = false;
            }




            return otvet;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

         

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyManager fm = doc.FamilyManager;

            IList<Element> couchSymbol = new FilteredElementCollector(doc).OfClass(typeof(Dimension))
                                                                       .OfCategory(BuiltInCategory.OST_Dimensions)
                                                                       .Where(it => it.Name.Contains("половина")).ToList();

            IList<FamilyParameter> renames_parameters = fm.GetParameters().Where(it => it.Definition.Name.Contains("половина")).ToList();
            /*
            IList<FamilyParameter> list_formula = fm.GetParameters().Where(it =>  Bool_test(it.Formula)).ToList();
            string ttt = "";
            
            foreach (FamilyParameter element in list_formula) 
            {
                ttt += "\n" + element.Formula;
            }
            */
            //TaskDialog.Show("Привет!", ttt);

            
                       // IList<FamilyParameter> list_formula = fm.GetParameters().Where(it => it != null).Where(it => it.Formula.Contains("половина")).ToList();

                        using (Transaction transaction = new Transaction(doc))
                        {

                            transaction.Start("Удаление половинчатых размеров");

                            foreach(Element elem in couchSymbol)
                            {
                                doc.Delete(elem.Id);
                            }


                            /*
                            foreach (FamilyParameter elem in list_formula)
                            {
                                string form = elem.Formula.Replace("половина", "/ 2");
                                fm.SetFormula(elem, form);
                            }
                            */
                            foreach (FamilyParameter elem in renames_parameters)
                            {
                                fm.RemoveParameter(elem);
                            }

                            transaction.Commit();

                        }
            
            return Result.Succeeded;
        }
    }
}
