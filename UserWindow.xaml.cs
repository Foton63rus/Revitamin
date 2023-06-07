using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.DB;
using System;
using Revitamin.Entity;

namespace Revitamin
{
    public partial class UserWindow : Window
    {
        IList<Element> allWalls;
        BM bm;
        public UserWindow( BM bm)
        {
            InitializeComponent();
            this.bm = bm;

            ConsoleBlock.Text += "Info:\n";
            ConsoleBlock.Text += bm.GetInfo();
        }
    }
}
