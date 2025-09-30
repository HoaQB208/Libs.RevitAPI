using Autodesk.Revit.UI;
using Libs.RevitAPI._Common;
using System;
using System.Collections.Generic;
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
                Image = icon,
                LargeImage = icon,
            };
            panel.AddItem(data);
        }

        public static void AddBigButton(RibbonPanel panel, string assemblyPath, string cmdClassName, string text, byte[] imgBytes, string toolTip = "", string longDescription = "")
        {
            BitmapImage icon = ImageUtils.ByteArrayToBitmapImage(imgBytes, 32, 32);
            PushButtonData data = new PushButtonData(Guid.NewGuid().ToString(), text, assemblyPath, cmdClassName)
            {
                ToolTip = toolTip,
                LongDescription = longDescription,
                Image = icon,
                LargeImage = icon,
            };
            panel.AddItem(data);
        }

        public static void AddBigButton(RibbonPanel panel, string assemblyPath, BaseButtonData buttonData)
        {
            BitmapImage icon = ImageUtils.ByteArrayToBitmapImage(buttonData.Icon, 32, 32);
            PushButtonData data = new PushButtonData(Guid.NewGuid().ToString(), buttonData.DisplayName, assemblyPath, buttonData.GetType().FullName)
            {
                ToolTip = buttonData.Description,
                LongDescription = buttonData.LongDescription,
                Image = icon,
                LargeImage = icon,
            };
            if (!string.IsNullOrEmpty(buttonData.HelpUrl))
            {
                data.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, buttonData.HelpUrl));
            }
            panel.AddItem(data);
        }

        public static void AddStackedItems(RibbonPanel panel, List<PushButtonData> buttons)
        {
            if (buttons.Count == 2) panel.AddStackedItems(buttons[0], buttons[1]);
            else if (buttons.Count == 3) panel.AddStackedItems(buttons[0], buttons[1], buttons[2]);
        }

        public static PushButtonData GetPushButtonData(string assemblyPath, string cmdClassName, string text, byte[] imgBytes, int size = 32, string toolTip = "", string longDescription = "")
        {
            BitmapImage icon = ImageUtils.ByteArrayToBitmapImage(imgBytes, size, size);
            PushButtonData data = new PushButtonData(Guid.NewGuid().ToString(), text, assemblyPath, cmdClassName)
            {
                ToolTip = toolTip,
                LongDescription = longDescription,
                Image = icon,
                LargeImage = icon,
            };
            return data;
        }

        public static PushButtonData GetPushButtonData(string assemblyPath, BaseButtonData buttonData, int size = 16)
        {
            BitmapImage icon = ImageUtils.ByteArrayToBitmapImage(buttonData.Icon, size, size);
            PushButtonData data = new PushButtonData(Guid.NewGuid().ToString(), buttonData.DisplayName, assemblyPath, buttonData.GetType().FullName)
            {
                ToolTip = buttonData.Description,
                LongDescription = buttonData.LongDescription,
                Image = icon,
                LargeImage = icon,

            };
            if (!string.IsNullOrEmpty(buttonData.HelpUrl))
            {
                data.SetContextualHelp(new ContextualHelp( ContextualHelpType.Url, buttonData.HelpUrl));
            }
            return data;
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