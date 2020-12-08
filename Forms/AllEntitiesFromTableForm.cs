using MSAccessApp.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class AllEntitiesFromTableForm : Form
    {
        #region Fields

        private readonly IDatabaseProvider _databaseProvider;

        private readonly int _listViewWidth = 799;
        private readonly int _listViewHieght = 469;
        private readonly Dictionary<Button, EventHandler> _clickHandlerOnButton = new Dictionary<Button, EventHandler>();

        #endregion

        #region .ctor

        public AllEntitiesFromTableForm(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            InitializeComponent();
            InitializeSelectTableButtons();
        }

        #endregion

        #region Private methods


        /// <summary>
        /// Создает кнопки для показа содержиого таблиц и ставит на них обработчики
        /// </summary>
        private void InitializeSelectTableButtons()
        {
            var x = 20;
            var y = 20;
            var buttonCounter = 0;

            foreach(var tableTitle in _databaseProvider.GetTables())
            {
                var showTableButton = new Button();
                showTableButton.Text = tableTitle;
                showTableButton.Location = new Point(x, y);
                showTableButton.Size = new Size(_listViewWidth / 5, 40);

                var onButtonClickHandler = CreateButtonOnClickHandler(tableTitle);
                showTableButton.Click += onButtonClickHandler;
                _clickHandlerOnButton[showTableButton] = onButtonClickHandler;

                Controls.Add(showTableButton);
                showTableButton.Show();

                x += (_listViewWidth / 5) + 50;

                // В строку влезает 5 кнопок
                if (++buttonCounter % 5 == 0)
                {
                    y += 50;
                    x = 20;
                }
            }
        }

        /// <summary>
        /// Фабрика обработчиков кликов по кнопкам
        /// </summary>
        /// <param name="tableName">Имя таблицы, для которой нужно получить выборку</param>
        /// <returns></returns>
        private EventHandler CreateButtonOnClickHandler(string tableName)
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
            listView.Location = new Point(69, 150);
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
