using MSAccessApp.Modules;
using MSAccessApp.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class EditRowFromTableForm : Form
    {
        private readonly IDatabaseProvider _databaseProvider;

        public EditRowFromTableForm(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            InitializeComponent();
        }

        //private void PrintTablesList()
        //{
        //    var tables = _databaseProvider.GetTables();
        //    var groupBox = new GroupBox();
        //    groupBox.Text = "Выберите таблицу";
        //    groupBox.Location = new Point(30, 70);
        //    groupBox.Size = new Size(220, (tables.Count + 2) * 20);

        //    var y = 20;
        //    foreach (var tableName in tables)
        //    {
        //        var button = new RadioButton();
        //        button.Location = new Point(31, y);
        //        button.Name = tableName;
        //        button.Text = tableName;
        //        button.CheckedChanged += HandleTablesCheckedChanged;

        //        groupBox.Controls.Add(button);
        //        _tablesRadioButtons.Add(button);
        //        y += 20;
        //    }

        //    Controls.Add(groupBox);
        //}
    }
}
