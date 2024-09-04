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
    public class Formula_find : IExternalCommand
    {

        public void Change_deep(IList<FamilyParameter> par_names_start, string par_names_end, FamilyManager f_m)
        {
            FamilyParameter finish_fam_par = f_m.GetParameters().First(it => it.Definition.Name == par_names_end);

            foreach (FamilyParameter item in par_names_start)
            {
                try
                {
                    FamilyParameter start_fam_par = f_m.GetParameters().First(it => it.Definition.Name == item.Definition.Name);
                    ParameterSet parset = start_fam_par.AssociatedParameters;

                    foreach (Parameter elem in parset)
                    {
                        f_m.AssociateElementParameterToFamilyParameter(elem, finish_fam_par);
                    }
                    f_m.RemoveParameter(start_fam_par);
                }
                catch
                {
                    TaskDialog.Show("Error!", "Проблема в неправильном параметре: " + item.Definition.Name);
                }
            }
        }


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //parameters_search5.Add("Горизонтальный импост 2 ширина");
            //parameters_search5.Add("Створка стекла 2 ширина");
            //parameters_search5.Add("Ширина двери графическая");

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyManager fm = doc.FamilyManager;

            using (Transaction transaction = new Transaction(doc))
            {

                transaction.Start("Приведение параметров");
                IList<FamilyParameter> start_fam_par = fm.GetParameters();
                IList<FamilyParameter> paramsWithFormuls = new List<FamilyParameter>();
                IList<FamilyParameter> paramsWithoutFormuls =  new List<FamilyParameter>();
                IList<string> paramsWithFormulsNamesFinish = new List<string>();
                IList<FamilyParameter> paramsStart = new List<FamilyParameter>();
                //IList<IList<string>> paramsWithFormulsNamesStart = new List<IList<string>>();
                IList<ElementId> paramsAccept = new List<ElementId>();
                string sss = "";

                foreach (var pair in start_fam_par)
                {
                    try
                    {
                        string formula = pair.Formula.ToString();
                        bool test_cont = formula.Contains("+") || formula.Contains("-") || formula.Contains("*") || formula.Contains(@"\") || formula.Contains("(") || formula.Contains(")") || formula.Contains(@"/");
                        
                        if (!test_cont) 
                        {
                            sss += pair.Formula + "\n";
                            paramsWithFormulsNamesFinish.Add(pair.Formula);
                            paramsStart.Add(pair);
                            //string couchSymbol = pair.Definition.Name;
                            //sss += couchSymbol + "\n"  + "______________________________________" + "\n";
                        }
                        //paramsWithFormulsNamesId.Add(pair.Id);
                    }
                    catch (Exception ex)
                    {
                        //sss += ex;    
                    }    
                }

                IList<string> FinishFinal = paramsWithFormulsNamesFinish.Distinct().ToList();
                try
                {
                    foreach (var pair in FinishFinal)
                    {
                        //List<string> params = paramsStart.Where(it => it.Definition.Name == pair).ToList();
                        Change_deep(paramsStart.Where(it => it.Formula == pair).ToList(), pair, fm);
                    }
                }
                catch
                { }

                sss += "\n" + "************************"+ "\n";
                
                    
                TaskDialog.Show("VVV", sss);


                transaction.Commit();

            }

            return Result.Succeeded;
        }
    }



    [Transaction(TransactionMode.Manual)]
    public class FormulaChangeRazmreToRasschet : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //parameters_search5.Add("Горизонтальный импост 2 ширина");
            //parameters_search5.Add("Створка стекла 2 ширина");
            //parameters_search5.Add("Ширина двери графическая");

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyManager fm = doc.FamilyManager;

            using (Transaction transaction = new Transaction(doc))
            {
                double counter = 0;

                transaction.Start("Меняем формулы с размерами");
                IList<FamilyParameter> AllParameter = fm.GetParameters();
                IList<FamilyParameter> paramsWithFormuls = new List<FamilyParameter>();

                

                foreach (var par in AllParameter)
                {
                    try
                    {
                        string formula = par.Formula;
                        if (!(formula.Contains("if")) && formula.Contains("размер")) {

                            fm.SetFormula(par, formula.Replace("размер", "расчёт"));
                            counter += 1;
                        }
                        




                    }
                    catch (Exception ex)
                    {
                            
                    }
                }
                TaskDialog.Show("Результат","Изменено параметров:" + counter.ToString());

                transaction.Commit();

            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class TransferParametersIntoArrayGropes : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            FamilyManager fm = doc.FamilyManager;

            IList<(string, BuiltInParameterGroup)> parameters = new List<(string, BuiltInParameterGroup)>();
            parameters.Add(("Откосы внутренние глубина", BuiltInParameterGroup.PG_CONSTRAINTS));
            parameters.Add(("Створка рама толщина", BuiltInParameterGroup.PG_CONSTRAINTS));

            IList<FamilyParameter> AllParameter = fm.GetParameters();
            double counter = 0;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Перемещаем параметры");
                
                foreach ((string, BuiltInParameterGroup) elem in parameters)
                {
                    try
                    {
                        FamilyParameter pars = AllParameter.Where(it => it.Definition.Name == elem.Item1 && it.Definition.ParameterGroup != elem.Item2).First();

                        InternalDefinition def = pars.Definition as InternalDefinition;

                        if (def != null)
                        {
                            counter += 1;
                            def.set_ParameterGroup(elem.Item2);
                        }
                    }
                    catch (Exception ex) { }
                }

                

                transaction.Commit();

                TaskDialog.Show("Перемещаем параметры", "Перемещено " + counter.ToString() + " из " + parameters.Count);

            }

            return Result.Succeeded;
        }
    }




}

