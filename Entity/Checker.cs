using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using Microsoft.Office.Interop.Excel;

namespace Revitamin.Entity
{
    internal class Checker : IChecker
    {
        private Document _document;
        private UserWindow _userWindow;
        public Checker(Document document) 
        {
            _document = document;
        }
        void IChecker.SetUserWindow(UserWindow userWindow)
        {
            _userWindow = userWindow;
        }

        string IChecker.check()
        {
            string parameter = _userWindow.tboxCheckerParameter.Text;
            string category = _userWindow.ComboBoxCategoryParametrChecker.Text;
            List<Element> Filter;
            StringBuilder sb = new StringBuilder();
            int writeCount;
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
            sb.AppendLine($"Поиск параметров завершён с {writeCount} совпадений");
            return sb.ToString();
        }
        int writeParameterValue(StringBuilder sb, List<Element> filter, string parameter)
        {
            int counter = 0;
            foreach (Element element in filter)
            {
                try
                {
                    string parameterValue = _document.GetElement(element.GetTypeId()).LookupParameter(parameter).AsValueString();
                    sb.AppendLine($"{element.Id} {element.Name} => {parameterValue}");
                    counter++;
                }
                catch
                {
                    //sb.AppendLine($"{{{element.Id} {element.Name}}} => - "); 
                }
            }
            return counter;
        }
    }

    public interface IChecker
    {
        void SetUserWindow(UserWindow userWindow);
        string check();
    }
}
