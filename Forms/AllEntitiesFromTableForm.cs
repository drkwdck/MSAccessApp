using MSAccessApp.Persistence;
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class AllEntitiesFromTableForm : Form
    {
        #region Fields

        private readonly IDatabaseProvider _databaseProvider;

        private readonly string _connectionStringForDatabase = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\EReshetnikov\source\repos\MSAccessApp\Persistence\lab-3.accdb";
        private readonly int _listViewWidth = 799;
        private readonly int _listViewHieght = 469;

        private Action<object, EventArgs> _handleStadiumsGetOnClick;
        private Action<object, EventArgs> _handleTeamsGetOnClick;
        private Action<object, EventArgs> _handleSportmansGetOnClick;
        private Action<object, EventArgs> _handleResultsGetOnClick;
        private Action<object, EventArgs> _handleSportTypeGetOnClick;

        #endregion

        #region .ctor

        public AllEntitiesFromTableForm(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
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
                var rows = _databaseProvider.GetRowsFromTable(tableName);

                // Формируем строки для модалки, отображающей выборку
                var items = rows.AsEnumerable().Where(row => row as DataRow != null)
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

                var columns = _databaseProvider.GetColumnsFromTable(tableName);

                foreach (var columnName in columns)
                {
                    listVeiw.Columns.Add(columnName.ToString(), _listViewWidth / columns.Count, HorizontalAlignment.Left);
                }

                listVeiw.Items.AddRange(items);

                Controls.Add(listVeiw);
                // Ставим фокус на модалку, чтобы закрыть её при потере фокуса
                listVeiw.Focus();
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
