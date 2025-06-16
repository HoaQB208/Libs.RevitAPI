using Autodesk.Revit.UI;
using Libs.RevitAPI._Ribbon;

namespace Libs.RevitAPI.Samples
{
    public class CreateRibbonMenu : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            CreateMenu(application, "HOA PHAM");
            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


        private void CreateMenu(UIControlledApplication app, string appName)
        {
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // AddTab
            AddMenu.AddTab(app, appName);

            // ABOUT
            RibbonPanel panel_About = AddMenu.AddPanel(app, appName, "ABOUT");

            /// Sử dụng nameof(HelloCmd) khi HelloCmd không sử dụng namespace. Nếu có sử dụng namespace thì hãy điền đầy đủ là 'Revit_API.Samples.HelloCmd'
            AddMenu.AddPushButton(panel_About, assemblyPath, nameof(HelloCmd), "Hello", "Show 'Hello'", "Show the simple text 'Hello' with OK button");
        }
    }
}