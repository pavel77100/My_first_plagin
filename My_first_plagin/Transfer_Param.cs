using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace My_first_plagin
{



    struct ParameterAndFormula
    {

        public FamilyParameter Parameter;

        public string Name()
        {
            return Parameter.Definition.Name;
        }
        public string Formula;
        public Dimension Size;

    }

    /*
        struct ParameterAndFormula
        {
            public string Name;
            public FamilyParameter Parameter;
            public string Formula;
            public ParameterSet InsideParams;
        }
    */
    [Transaction(TransactionMode.Manual)]
    internal class Transfer_Param : IExternalCommand
    {
        private bool testFormula(FamilyParameter param)
        {
            bool test = false;
            try
            {
                var t = param.Formula.Length;
            }
            catch 
            {
                test = true;
            }
            return test;
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            Document doc = uiDoc.Document;

            FamilyManager fm = doc.FamilyManager;

            //IList<Element> couchSymbol = new FilteredElementCollector(doc).OfClass(typeof(Dimension))
                                                                    //   .OfCategory(BuiltInCategory.OST_Dimensions)
                                                                    //   .Where(it => it.Name.Contains("Створка двери ширина 1")).ToList();

            

            IList<FamilyParameter> renames_parameters = fm.GetParameters()
                .Where(it => it.Definition.ParameterGroup == BuiltInParameterGroup.INVALID && testFormula(it) && it.Definition.ParameterType != ParameterType.YesNo).ToList();

            



            IList<ParameterAndFormula> parameters = new List<ParameterAndFormula>();
            



            //var aa = param_change.ExternalDefinition;
            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Transfer_Parameters");
                string sss = "";

                foreach (FamilyParameter param in renames_parameters)
                {

                    

                    InternalDefinition def = param.Definition as InternalDefinition;

                    if (def != null)
                    {
                        def.set_ParameterGroup(BuiltInParameterGroup.PG_CONSTRAINTS);
                    }


                    FamilyParameter test = fm.AddParameter(param.Definition.Name + "ъъ", BuiltInParameterGroup.PG_CONSTRAINTS, param.Definition.ParameterType, param.IsInstance);
                    
                    sss += param.Definition.ToString() + "\n";
                   
                }

                TaskDialog.Show("1111111111", sss);
               
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
