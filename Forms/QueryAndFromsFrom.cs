using MSAccessApp.Persistence;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
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

                    foreach(DataRow row in dataSet.Tables[0].Rows)
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
            var stringQuery = "SELECT Спортсмен.[Фамилия] FROM Спортсмен WHERE Спортсмен.[Номер спортсмена] IN ( SELECT TOP 3 результат.[Номер спортсмена] FROM Результат ORDER BY Результат.[Результат попытки]);";
        }
    }
}
