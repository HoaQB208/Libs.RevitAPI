using Autodesk.Revit.UI;
using System;
using System.Windows.Media.Imaging;

namespace Libs.RevitAPI._Ribbon
{
    public class AddMenu
    {
        public static void AddTab(UIControlledApplication app, string toolName)
        {
            app.CreateRibbonTab(toolName);
        }

        public static RibbonPanel AddPanel(UIControlledApplication app, string toolName, string panelName)
        {
            return app.CreateRibbonPanel(toolName, panelName);
        }

        public static void AddBigButton(RibbonPanel panel, string assemblyPath, string cmdClassName, string text, BitmapImage icon, string toolTip = "", string longDescription = "")
        {
            PushButtonData data = new PushButtonData(Guid.NewGuid().ToString(), text, assemblyPath, cmdClassName)
            {
                ToolTip = toolTip,
                LongDescription = longDescription,
                LargeImage = icon,
            };
            panel.AddItem(data);
        }

        public static void AddButton(RibbonPanel panel, string assemblyPath, string cmdClassName, string text, string toolTip = "", string longDescription = "")
        {
            PushButtonData data = new PushButtonData(Guid.NewGuid().ToString(), text, assemblyPath, cmdClassName)
            {
                ToolTip = toolTip,
                LongDescription = longDescription,
            };
            panel.AddItem(data);
        }

        public static void AddPushButton(RibbonPanel panel, string assemblyPath, string cmdClassName, string text, string toolTip = "", string longDescription = "")
        {
            PushButtonData data = new PushButtonData(Guid.NewGuid().ToString(), text, assemblyPath, cmdClassName)
            {
                ToolTip = toolTip,
                LongDescription = longDescription
            };
            panel.AddItem(data);
        }

        public static PulldownButton AddPulldownButton(RibbonPanel panel, string text, string toolTip = "", string longDescription = "")
        {
            PulldownButtonData data = new PulldownButtonData(Guid.NewGuid().ToString(), text)
            {
                ToolTip = toolTip,
                LongDescription = longDescription,
            };
            return panel.AddItem(data) as PulldownButton;
        }


    }
}