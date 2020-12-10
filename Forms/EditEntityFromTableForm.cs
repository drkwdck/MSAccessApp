using MSAccessApp.Modules;
using MSAccessApp.Persistence;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class EditEntityFromTableForm : Form
    { 
        private readonly IDatabaseProvider _databaseProvider;

        private List<RadioButton> _tablesRadioButtons = new List<RadioButton>();
        private Button _editEntityButton;
        private GroupBox _currentInputs;

        public EditEntityFromTableForm(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            InitializeComponent();
            _editEntityButton = new Button();
            _editEntityButton.Hide();
            _editEntityButton.Text = "Редактировать";
            _editEntityButton.Location = new Point(650, 75);
            _editEntityButton.Click += HandleOnSumbit;
            _editEntityButton.Size = new Size(100, 40);
            Controls.Add(_editEntityButton);
            PrintTablesList();
        }

        private void PrintTablesList()
        {
            var tables = _databaseProvider.GetTables();
            var groupBox = new GroupBox();
            groupBox.Text = "Выберите таблицу";
            groupBox.Location = new Point(30, 70);
            groupBox.Size = new Size(220, (tables.Count + 2) * 20);

            var y = 20;
            foreach (var tableName in tables)
            {
                var button = new RadioButton();
                button.Location = new Point(31, y);
                button.Name = tableName;
                button.Text = tableName;
                button.CheckedChanged += HandleTablesCheckedChanged;

                groupBox.Controls.Add(button);
                _tablesRadioButtons.Add(button);
                y += 20;
            }

            Controls.Add(groupBox);
        }

        private void HandleTablesCheckedChanged(object sender, EventArgs e)
        {
            var button = sender as RadioButton;
            if (button == null) { return; }

            if (button.Checked)
            {
                if (_currentInputs != null)
                {
                    Controls.Remove(_currentInputs);
                    _currentInputs?.Dispose();
                }

                _editEntityButton.Show();
                var groupBox = new GroupBox();
                groupBox.Text = "Заполните поля, которые необходимо обновить";
                (var rows, var columns) = _databaseProvider.GetRowsFromTable(button.Name);
                groupBox.Location = new Point(300, 70);
                groupBox.Size = new Size(320, (columns.Count) * 40 + 40);

                var y = 20;

                foreach (var column in columns)
                {
                    var input = new TextBox();
                    input.Location = new Point(5, y);
                    input.Size = new Size(300, 30);
                    input.Text = $"{column}:";
                    y += 40;
                    groupBox.Controls.Add(input);
                }

                Controls.Add(groupBox);
                _currentInputs = groupBox;
            }
        }

        private void HandleOnSumbit(object sender, EventArgs e)
        {
            var tableName = _tablesRadioButtons.FirstOrDefault(_ => _.Checked).Name;
            var values = new string[_currentInputs.Controls.Count];
            var typeOnColumns = _databaseProvider.GetTableColumnsWithTypes(tableName);
            (var rows, var columns) = _databaseProvider.GetRowsFromTable(tableName);

            for (var i = 0; i < _currentInputs.Controls.Count; ++i)
            {
                values[i] = Parser.GetValueFromInput(_currentInputs.Controls[i].Text, typeOnColumns[columns[i]]);
                _currentInputs.Controls[i].Text = columns[i] + ":";
            }

            if (_databaseProvider.UpdateRowFromTable(tableName, values))
            {
                MessageBox.Show("Запись успешно обновлена.");
            }
            else
            {
                MessageBox.Show("Запись обновлена не была. Попробуйте снова.");
            }
        }
    }
}
