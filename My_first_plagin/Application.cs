using Autodesk.Revit.UI;
using System;
using System.Diagnostics.Tracing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace My_first_plagin
{
    internal class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                   iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\",
                   tabName = "Бфзовый курс";

            application.CreateRibbonTab(tabName);

            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Первый плагин");

            PushButtonData bottonData = new PushButtonData(nameof(Command_first), "Приветствие", assemblyLocation, typeof(Command_first).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
            };

            PushButtonData bottonData2 = new PushButtonData(nameof(SecondCommand), "Гол", assemblyLocation, typeof(SecondCommand).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
            };

           

            PushButtonData bottonData4 = new PushButtonData(nameof(Four_Command), "Гооoл", assemblyLocation, typeof(Four_Command).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
            };

            PushButtonData bottonData5 = new PushButtonData(nameof(Five_Command), "Гооoл", assemblyLocation, typeof(Five_Command).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
            };

            PushButtonData bottonData6 = new PushButtonData(nameof(Seven_command), "Гооoл", assemblyLocation, typeof(Seven_command).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
            };

            panel.AddItem(bottonData);
            panel.AddItem(bottonData2);
            
            panel.AddItem(bottonData4);
            panel.AddItem(bottonData5);
            panel.AddItem(bottonData6);
            panel.AddSeparator();

            return Result.Succeeded;

        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


    }
}
