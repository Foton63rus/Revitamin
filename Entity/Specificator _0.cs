using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Revitamin.Entity
{
    public class Specificator2
    {
        private Document _document;
        private string _outputFilePath;
        private CategoriesInfo _categories;
        private Dictionary<BuiltInCategory, List<Element>> notEmptyCategories = 
            new Dictionary<BuiltInCategory, List<Element>>();

        BuiltInCategory[] categoriesWithMaterialsOutput = new BuiltInCategory[]
        {
            BuiltInCategory.OST_Walls,
            BuiltInCategory.OST_Floors,
            BuiltInCategory.OST_Ceilings
        };

        public Specificator2(Document document, string outputPath = "")
        {
            this._document = document;
            this._outputFilePath = outputPath;
            _categories = new CategoriesInfo();
            foreach (BuiltInCategory builtInCategory in Enum.GetValues(typeof(BuiltInCategory)))
            {
                try
                {
                    List<Element> catFilter = new FilteredElementCollector(document).WhereElementIsNotElementType().
                    OfCategory(builtInCategory).ToElements().ToList();
                    if (catFilter.Count > 0 
                        //&& !categoriesExcludedFromFilter.Contains(builtInCategory) 
                        )
                    {
                        _categories.AddCategory(builtInCategory, catFilter);
                    }
                }
                catch { }
            }
        }

        public string GetSpecificationByElement()
        {
            StringBuilder outputMSG = new StringBuilder("");
            
            if (_outputFilePath != "")
            {
                File.WriteAllText(_outputFilePath, outputMSG.ToString());
                MessageBox.Show("Specifiaction writed");
            }
            return outputMSG.ToString();
        }

        public string GetSpecificationByCategories()
        {
            return $"\nSpecification\n {_categories.GetInfo()}";
        }
    }
}
