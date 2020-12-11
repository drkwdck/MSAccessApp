using MSAccessApp.Persistence;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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
                    var stringQuery = "SELECT AVG([Количество игроков]) AS [Среднее количество игроков] FROM Команда; ";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    MessageBox.Show($"Среднее кол-во игроков: {dataSet.Tables[0].Rows[0].ItemArray[0]}");
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
                    var stringQuery = "SELECT [Идентификатор команды], COUNT(*) AS Количество FROM Спортсмен GROUP BY[Идентификатор команды];";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    var result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        result += $"{row.ItemArray[0]}: {row.ItemArray[1]}\n";
                    }

                    MessageBox.Show($"Кол-во спортсменов по командам\n{ result}");
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
                    var stringQuery = "SELECT Спортсмен.[Фамилия] FROM Спортсмен WHERE Спортсмен.[Номер спортсмена] IN ( SELECT TOP 3 результат.[Номер спортсмена] FROM Результат ORDER BY Результат.[Результат попытки]);";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    var result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        result += $"{row.ItemArray[0]}\n";
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
            if (!IsQueryExist("Спортсмены с заданным именем")) { return; }

            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = $"SELECT * FROM Спортсмен WHERE[Имя] = '{textBox2.Text}'; ";
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

                    MessageBox.Show($"Спортсмены с заданным именем:\n{result}");
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
