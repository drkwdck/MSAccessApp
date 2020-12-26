﻿using MSAccessApp.Persistence;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class QueryAndFromsFrom : Form
    {
        private readonly IDatabaseProvider _databaseProvider;

        public QueryAndFromsFrom(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            InitializeComponent();
        }

        /// <summary>
        /// Проверяет существование запроса по таблице MSysObjects
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private bool IsQueryExist(string query)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    connection.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT MSysObjects.Id, MSysObjects.Name FROM MSysObjects WHERE(((MSysObjects.Name) = @Temp1))";
                    cmd.Parameters.AddWithValue("@Temp1", query);
                    OleDbDataReader rd = cmd.ExecuteReader();
                    return rd.HasRows;
                }
                catch { }
            }

            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist(button.Text)) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = "SELECT COUNT(*) FROM Студент WHERE Пол='ж'";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    MessageBox.Show($"кол-во студентов женщин: {dataSet.Tables[0].Rows[0].ItemArray[0]}");
                }
                catch { }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist(button.Text)) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = "SELECT MAX([Количество студентов]) AS [Максимальное количество студентов] FROM Группа; ";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    var result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        result += $"{row.ItemArray[0]}";
                    }

                    MessageBox.Show($"Максмальное количество студентов:\n{ result}");
                }
                catch { }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist(button.Text)) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = "SELECT * FROM Группа WHERE[Количество студентов] = 2;";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    var result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        result += $"Номер группы: {row.ItemArray[0]}    ИД: {row.ItemArray[1]}   Факультет: {row.ItemArray[2]}   Кол-во: {row.ItemArray[3]}\n";
                    }

                    MessageBox.Show($"{button.Text}:\n{ result}");
                }
                catch { }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist("Спортсмен по номеру команды")) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = $"SELECT * FROM Спортсмен WHERE Спортсмен.[Идентификатор команды]= {textBox1.Text}; ";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    var result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        foreach (var filed in row.ItemArray)
                        {
                            result += filed.ToString() + " ";
                        }

                        result += "\n";
                    }

                    MessageBox.Show($"Спортсмен по номеру команды:\n{result}");
                }
                catch
                {
                    MessageBox.Show("Попробуйте снова.");
                }
                finally
                {
                    textBox1.Text = "";
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist("поиск по полу")) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = $"SELECT * FROM Студент WHERE Пол = '{textBox2.Text}';";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    var result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        var i = 0;
                        foreach (var filed in row.ItemArray.Skip(1))
                        {
                            if (i++ >= 3) { break; } 
                            result += filed.ToString() + "  ";
                        }

                        result += "\n";
                    }

                    MessageBox.Show($"Студениты по полу:\n{result}");
                }
                catch
                {
                    MessageBox.Show("Попробуйте снова.");
                }
                finally
                {
                    textBox2.Text = "";
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist("Кол-во соревнований по виду спорта")) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = $"SELECT COUNT(*) AS [Кол-во соревнований] FROM Соревнование WHERE[Идентификатор вида спорта] = {textBox3.Text}; ";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    var result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        foreach (var filed in row.ItemArray)
                        {
                            result += filed.ToString() + " ";
                        }

                        result += "\n";
                    }

                    MessageBox.Show($"Кол-во соревнований по виду спорта:\n{result}");
                }
                catch
                {
                    MessageBox.Show("Попробуйте снова.");
                }
                finally
                {
                    textBox3.Text = "";
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist("6_delete_1")) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE * FROM Соревнование WHERE[Идентификатор вида спорта] IS NULL;";
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Попробуйте снова.");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist("Обновить имя цветковой")) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE Спортсмен SET Фамилия = 'Фамилия после адпейта' WHERE Фамилия = 'Цветкова';";
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Попробуйте снова.");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) { return; }
            if (!IsQueryExist("6_hart_insert_1")) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO Спортсмен ( Фамилия ) SELECT Тренер FROM Команда;";
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Попробуйте снова.");
                }
            }
        }
    }
}
