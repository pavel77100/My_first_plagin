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
    public class CommandForWindowParameter : IExternalCommand
    {

        public void Change_deep(IList<string> par_names_start, string par_names_end, FamilyManager f_m)
        {
            FamilyParameter finish_fam_par = f_m.GetParameters().First(it => it.Definition.Name == par_names_end);
            
            foreach (string item in par_names_start)
            {
                try
                {
                    FamilyParameter start_fam_par = f_m.GetParameters().First(it => it.Definition.Name == item);
                    ParameterSet parset = start_fam_par.AssociatedParameters;

                    foreach (Parameter elem in parset)
                    {
                        f_m.AssociateElementParameterToFamilyParameter(elem, finish_fam_par);
                        
                    }
                    f_m.RemoveParameter(start_fam_par);

                }
                catch
                {
                    TaskDialog.Show("Error!", "Проблема в неправильном параметре: " + item);
                }
            }
        }
        

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            IList<string> right_parameters = new List<string>();
            right_parameters.Add("Рама глубина");//0
            right_parameters.Add("Ширина");//1
            right_parameters.Add("Высота");//2
            right_parameters.Add("Высота двери");//3
            right_parameters.Add("Ширина двери");//4
            right_parameters.Add("Высота до подоконника");//5
            right_parameters.Add("Створка рама толщина");//6
            right_parameters.Add("Ширина 1");//7

            //--------------------------------------------------------------------------------------------------------------------------
            IList<string> parameters_search1 = new List<string>();//0
            parameters_search1.Add("Вертикальный импост глубина");
            parameters_search1.Add("Монтажный зазор глубина");
            parameters_search1.Add("Створка стекла рама глубина");
            parameters_search1.Add("Горизонтальный импост глубина");
            parameters_search1.Add("Створка двери глубина");
            parameters_search1.Add("Створка рама глубина");


            IList<string> parameters_search2 = new List<string>();//1
            parameters_search2.Add("Монтажный зазор ширина");
            //parameters_search2.Add("Подоконник ширина");

            IList<string> parameters_search3 = new List<string>();//2
            parameters_search3.Add("Монтажный зазор высота");

            IList<string> parameters_search4 = new List<string>();//3
            parameters_search4.Add("Створка двери высота");

            IList<string> parameters_search5 = new List<string>();//4
            //parameters_search5.Add("Створка двери ширина");
            //parameters_search5.Add("Створка 1 ширина");
            parameters_search5.Add("Ширина двери графическая");
            parameters_search5.Add("Ширина двери отображение");
            //parameters_search5.Add("Створка стекла 2 ширина");
            //parameters_search5.Add("Горизонтальный импост ширина 2");
            parameters_search5.Add("Монтажный зазор ширина двери");

            IList<string> parameters_search6 = new List<string>();//5
            parameters_search6.Add("Монтажный зазор высота до подоконника");

            IList<string> parameters_search7 = new List<string>();//6
            parameters_search7.Add("Рама стекла толщина");

            IList<string> parameters_search8 = new List<string>();//7
            parameters_search8.Add("Монтажный зазор ширина 1");
            parameters_search8.Add("Откосы наружные боковые ширина 1");
            parameters_search8.Add("Рама ширина 1");
            parameters_search8.Add("Подоконник ширина 2");


            IList<IList<string>> lists = new List<IList<string>>();
            lists.Add(parameters_search1);
            lists.Add(parameters_search2);
            lists.Add(parameters_search3);
            lists.Add(parameters_search4);
            lists.Add(parameters_search5);
            lists.Add(parameters_search6);
            lists.Add(parameters_search7);
            lists.Add(parameters_search8);





            //parameters_search5.Add("Горизонтальный импост 2 ширина");
            //parameters_search5.Add("Створка стекла 2 ширина");
            //parameters_search5.Add("Ширина двери графическая");

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyManager fm = doc.FamilyManager;

            using (Transaction transaction = new Transaction(doc))
            {

                transaction.Start("Приведение параметров");

                var zipped = right_parameters.Zip(lists, (i, s) => Tuple.Create(i, s));

                foreach (var pair in zipped)
                {
                    IList<string> errors = new List<string>();
                    try
                    {
                        Change_deep(pair.Item2, pair.Item1, fm);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error!", "Проблема в правильном параметре: "+pair.Item1);
                    }
                }

                


                transaction.Commit();

            }

            return Result.Succeeded;
        }
    }
}
