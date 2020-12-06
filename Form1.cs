using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSAccessApp
{
    public partial class Form1 : Form
    {
        #region Fields

        private readonly string _connectionStringForDatabase = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\EReshetnikov\Desktop\lab-3.accdb";
        private readonly int _listViewWidth = 799;
        private readonly int _listViewHieght = 469;

        private Action<object, EventArgs> _handleStadiumsGetOnClick;
        private Action<object, EventArgs> _handleTeamsGetOnClick;
        private Action<object, EventArgs> _handleSportmansGetOnClick;
        private Action<object, EventArgs> _handleResultsGetOnClick;
        private Action<object, EventArgs> _handleSportTypeGetOnClick;

        #endregion

        #region .ctor

        public Form1()
        {
            _handleStadiumsGetOnClick = CreateButtonOnClickHandler("Стадион");
            _handleTeamsGetOnClick = CreateButtonOnClickHandler("Команда");
            _handleSportmansGetOnClick = CreateButtonOnClickHandler("Спортсмен");
            _handleResultsGetOnClick = CreateButtonOnClickHandler("Результат");
            _handleSportTypeGetOnClick = CreateButtonOnClickHandler("Вид_спорта");
            InitializeComponent();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Фабрика обработчиков кликов по кнопкам
        /// </summary>
        /// <param name="tableName">Имя таблицы, для которой нужно получить выборку</param>
        /// <returns></returns>
        private Action<object, EventArgs> CreateButtonOnClickHandler(string tableName)
        {
            return (sender, args) =>
            {
                using (var connection = new OleDbConnection(_connectionStringForDatabase))
                {
                    try
                    {
                        // Получаем выборку
                        connection.Open();
                        var stringQuery = $"SELECT * FROM {tableName}";
                        OleDbDataAdapter adapter = new OleDbDataAdapter(stringQuery, connection);
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count == 0) { return; }

                        // Формируем строки для модалки, отображающей выборку
                        var items = dataSet.Tables[0].AsEnumerable().Where(row => row as DataRow != null)
                            .Select(dataRow =>
                            {
                                var listViewItem = new ListViewItem(dataRow.ItemArray[0]?.ToString());

                                if (dataRow.ItemArray.Length > 1)
                                {
                                    listViewItem.SubItems.AddRange(dataRow.ItemArray.Skip(1).Select(_ => _?.ToString()).ToArray());

                                }

                                return listViewItem;
                            })
                            .ToArray();

                        // Формируем саму модалку
                        var listVeiw = new ListView();
                        ListViewInitilze(listVeiw);

                        foreach (var columnName in dataSet.Tables[0].Columns)
                        {
                            listVeiw.Columns.Add(columnName.ToString(), _listViewWidth / dataSet.Tables[0].Columns.Count, HorizontalAlignment.Left);
                        }

                        listVeiw.Items.AddRange(items);

                        Controls.Add(listVeiw);
                        // Ставим фокус на модалку, чтобы закрыть её при потере фокуса
                        listVeiw.Focus();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Ошибка во время получения списка записей из таблицы {tableName} {e.Message}");
                    }
                }
            };
        }

        /// <summary>
        /// Сетит окну со списком размер, положений и ставит обработчик на автоматическое закрытие при потере фокуса
        /// </summary>
        /// <param name="listView"></param>
        private void ListViewInitilze(ListView listView)
        {
            listView.AllowColumnReorder = true;
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new Point(69, 89);
            listView.Size = new Size(_listViewWidth, _listViewHieght);
            listView.LostFocus += HandleLostFocus;

            void HandleLostFocus(object sender, EventArgs e)
            {
                var listViewToDispose = sender as ListView;
                Controls.Remove(listViewToDispose);
                listViewToDispose.LostFocus -= HandleLostFocus;
            }
        }

        #endregion
    }
}
