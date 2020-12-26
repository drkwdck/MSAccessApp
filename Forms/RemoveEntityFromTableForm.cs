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
    public partial class RemoveEntityFromTableForm : Form
    {
        private readonly IDatabaseProvider _databaseProvider;

        private List<RadioButton> _tablesRadioButtons = new List<RadioButton>();
        private Button _deleteEntityButton;
        private GroupBox _inputEntityId;
        private Button _closeButton = new Button();

        public RemoveEntityFromTableForm(IDatabaseProvider databaseProvider)
        {
            this.BackColor = Color.DarkOliveGreen;
            _closeButton.Location = new Point(30, 350);
            _closeButton.Size = new Size(100, 50);
            _closeButton.Show();
            _closeButton.BackColor = Color.DarkSeaGreen;
            _closeButton.ForeColor = Color.White;
            _closeButton.Click += (_, __) => this.Close();
            Controls.Add(_closeButton);
            _closeButton.Text = "В главное меню";

            _databaseProvider = databaseProvider;
            InitializeComponent();
            _deleteEntityButton = new Button();
            _deleteEntityButton.Text = "Удалить";
            _deleteEntityButton.ForeColor = Color.White;
            _deleteEntityButton.Location = new Point(650, 70);
            _deleteEntityButton.Click += HandleOnSumbit;
            _deleteEntityButton.BackColor = Color.DarkSeaGreen;
            Controls.Add(_deleteEntityButton);
            _deleteEntityButton.Hide();
            PrintTablesList();
        }

        private void PrintTablesList()
        {
            var tables = _databaseProvider.GetTables();
            var groupBox = new GroupBox();
            groupBox.Text = "Выберите таблицу";
            groupBox.ForeColor = Color.White;
            groupBox.Location = new Point(30, 70);
            groupBox.Size = new Size(220, (tables.Count + 2) * 20);

            var y = 20;
            foreach (var tableName in tables)
            {
                var button = new RadioButton();
                button.Location = new Point(31, y);
                button.Name = tableName;
                button.ForeColor = Color.White;
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
                if (_inputEntityId != null)
                {
                    Controls.Remove(_inputEntityId);
                    _inputEntityId?.Dispose();
                }

                _deleteEntityButton.Show();

                var groupBox = new GroupBox();
                groupBox.Text = "Введите Id записи, которую необходимо удалить";
                groupBox.ForeColor = Color.White;
                groupBox.Location = new Point(300, 70);
                groupBox.Size = new Size(300, 70);
                var input = new TextBox();
                input.Location = new Point(5, 40);
                input.Size = new Size(250, 30);
                groupBox.Controls.Add(input);
                Controls.Add(groupBox);
                _inputEntityId = groupBox;
            }
        }

        private void HandleOnSumbit(object sender, EventArgs e)
        {
            var tableName = _tablesRadioButtons.FirstOrDefault(_ => _.Checked).Name;

            var parsed = Parser.GetValueFromInput(_inputEntityId.Controls[0].Text, typeof(int));
            
            if (string.IsNullOrEmpty(parsed))
            {
                MessageBox.Show("Вы ввели невалидное значение");
            }
            else
            {
                if(!_databaseProvider.RemoveRowFromTable(tableName, parsed))
                {
                    MessageBox.Show("Запись удалена не была. Попробуйте снова.");
                }
                else
                {
                    MessageBox.Show("Запись удалена.");
                }
            }

            _inputEntityId.Controls[0].Text = "";
        }
    }
}
