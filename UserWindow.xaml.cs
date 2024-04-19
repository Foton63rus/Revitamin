using Autodesk.Revit.DB;
using Revitamin.Entity;
using Revitamin.Entity.WebService;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Revitamin
{
    public partial class UserWindow : Window
    {
        IWebService webService = new WebService();
        //string url = "http://uniceros.alexanderivanof.ru/revit-project-objects-add";
        Specificator specificator;
        IChecker checker;

        public UserWindow( Specificator specificator)
        {
            InitializeComponent();
            initializeCheckerTab();
            this.specificator = specificator;
            string json = specificator.GetJson();
            ConsoleBlock.Text += json;
            //MessageBox.Show(json);
            //WebPostRequest(url, json);
        }
        private void initializeCheckerTab()
        {
            checker = CMD_GetInfo.checker;
            checker.SetUserWindow(this);

            tboxCheckerConsole.Text = "";

            foreach (string key in CMD_GetInfo.GLOBAL_VARIABLES.BuiltInCategories.Keys.OrderBy(x => x))
            {
                ComboBoxCategoryParametrChecker.Items.Add(key);
            }
        }

        void btnCheckerCheckClick(object sender, RoutedEventArgs e)
        {
            tboxCheckerConsole.Text = $"{checker.check()}\n";
        }
        public void AddButton(ElementId ID, string content)
        {
            Button btn = new Button();
            btn.Width = 400;
            btn.Content = content;
            btn.Height = 30;
            //Trigger trigger = new Trigger();
            //Setter setter = new Setter();
            //trigger.Property = IsMouseOverProperty;
            //trigger.Value = "True";
            //setter.Property = BackgroundProperty;
            //setter.Value = "DarkGoldenrod";
            //trigger.Setters.Add(setter);
            //btn.Style.Triggers.Add(trigger);
            btn.Click += (s, e) => { CMD_GetInfo.CommandData.Application.ActiveUIDocument.Selection.SetElementIds(new List<ElementId>() { ID }); };
            this.CheckerStackPanel.Children.Add(btn);
        }

        private void btnSend2ServerClick(object sender, RoutedEventArgs e)
        {
            webService.PostRequest(specificator.GetJson());
        }
    }
}
