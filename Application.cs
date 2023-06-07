using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Revitamin
{
    internal class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                   iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\",
                   tabName = "Yousee";

            application.CreateRibbonTab(tabName);

            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Info panel");

            PushButtonData buttonData = new PushButtonData( "Full info", "Приветствие", assemblyLocation, typeof(CMD_GetInfo).FullName );

            var LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "volume.png"));

            panel.AddItem( buttonData );

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
