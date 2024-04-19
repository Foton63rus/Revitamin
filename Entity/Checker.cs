using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revitamin.Entity
{
    
    public class Checker : IChecker
    {
        public delegate void checkedNewItem(ElementId ID, string content);
        public event checkedNewItem OnCheckedNewItem;

        private Document _document;
        private UserWindow _userWindow;
        public Checker(Document document) 
        {
            _document = document;
        }
        void IChecker.SetUserWindow(UserWindow userWindow)
        {
            _userWindow = userWindow;
            OnCheckedNewItem += _userWindow.AddButton;
        }

        string IChecker.check()
        {
            string parameter = _userWindow.tboxCheckerParameter.Text;
            string category = _userWindow.ComboBoxCategoryParametrChecker.Text;
            List<Element> Filter;
            StringBuilder sb = new StringBuilder();
            string writeCount;
            if (category == "All") 
            {
                Filter = new FilteredElementCollector(_document).WhereElementIsNotElementType().ToList();
                writeCount = writeParameterValue(sb, Filter, parameter);
            }
            else
            {
                BuiltInCategory cat = CMD_GetInfo.GLOBAL_VARIABLES.BuiltInCategories[_userWindow.ComboBoxCategoryParametrChecker.Text];
                Filter = new FilteredElementCollector(_document).WhereElementIsNotElementType().OfCategory(cat).ToList();
                writeCount = writeParameterValue(sb, Filter, parameter);
            }
            sb.AppendLine($"Поиск параметров завершён. Заполненность = {writeCount}");
            return sb.ToString();
        }
        string writeParameterValue(StringBuilder sb, List<Element> filter, string parameter)
        {
            int counter = 0;
            int counterParamHasValue = 0;
            _userWindow.CheckerStackPanel.Children.Clear();
            foreach (Element element in filter)
            {
                var currentParameter = _document.GetElement(element.GetTypeId())?.LookupParameter(parameter);
                if (currentParameter != null)
                {
                    string parameterValue = currentParameter.AsValueString();
                    if (parameterValue == "" || parameterValue == null)
                    {
                        parameterValue = "{НЕТ ЗНАЧЕНИЯ}";
                    }// Описание
                    else
                    {
                        counterParamHasValue++;
                    }

                    string output = $"{element.Id} {element.Name} => {parameterValue}";
                    sb.AppendLine(output);
                    ElementId ID = element.Id;
                    //AddButton(ID, output);
                    OnCheckedNewItem?.Invoke(ID, output);
                    counter++;
                }
                else
                {
                    
                }
            }
            return $"{counterParamHasValue} из {counter}";
        }
    }

    public interface IChecker
    {
        void SetUserWindow(UserWindow userWindow);
        string check();
    }

}
