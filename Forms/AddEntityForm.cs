using MSAccessApp.Modules;
using MSAccessApp.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class AddEntityForm : Form
    {
        private readonly IDatabaseProvider _dataBaseProvider;

        private List<RadioButton> _tablesRadioButtons = new List<RadioButton>();
        private Button _addEntityButton;
        private GroupBox _currentInputs;

        public AddEntityForm(IDatabaseProvider databaseProvider)
        {
            _dataBaseProvider = databaseProvider;
            InitializeComponent();
            _addEntityButton = new Button();
            _addEntityButton.Hide();
            _addEntityButton.Text = "Добавить";
            _addEntityButton.Location = new Point(650, 75);
            _addEntityButton.Click += HandleOnSumbit;
            Controls.Add(_addEntityButton);
            PrintTablesList();
        }

        private void PrintTablesList()
        {
            var tables = _dataBaseProvider.GetTables();
            var groupBox = new GroupBox();
            groupBox.Text = "Выберите таблицу";
            groupBox.Location = new Point(30, 70);
            groupBox.Size = new Size(230, (tables.Count + 2) * 20);

            var y = 20;
            foreach (var tableName in tables)
            {
                var button = new RadioButton();
                button.Location = new Point(31, y);
                button.Size = new Size(190, 20);
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

                _addEntityButton.Show();
                var groupBox = new GroupBox();
                groupBox.Text = "Заполните поля новой записи";
                (var rows, var columns) = _dataBaseProvider.GetRowsFromTable(button.Name);
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
            var typeOnColumns = _dataBaseProvider.GetTableColumnsWithTypes(tableName);
            (var rows, var columns) = _dataBaseProvider.GetRowsFromTable(tableName);
            var success = true;

            for (var i = 0; i < _currentInputs.Controls.Count; ++i)
            {
               values[i] = Parser.GetValueFromInput(_currentInputs.Controls[i].Text, typeOnColumns[columns[i]]);
                success &= !string.IsNullOrEmpty(values[i]);
                _currentInputs.Controls[i].Text = columns[i] + ":";
            }

            if (success)
            {
                success &= _dataBaseProvider.AddRowToTable(tableName, values);
            }

            if (success)
            {
                MessageBox.Show("Запись успешно добавлена.");
            }
            else
            {
                MessageBox.Show("Запись добавлена не была. Попробуйте снова.");
            }
        }
    }
}
