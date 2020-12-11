using MSAccessApp.Persistence;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class MSysObjectsForm : Form
    {
        private readonly IDatabaseProvider _databaseProvider;
        private readonly int _listViewWidth = (int)(799 * 2);
        private readonly int _listViewHieght = (int)(469 * 2);

        public MSysObjectsForm(IDatabaseProvider dataBaseProvider)
        {
            _databaseProvider = dataBaseProvider;
            InitializeComponent();
            ShowMSysObjectsTable();
        }

        private void ShowMSysObjectsTable()
        {
            (var rows, var columns) = _databaseProvider.GetRowsFromTable("MSysObjects");

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

            foreach (var columnName in columns)
            {
                listVeiw.Columns.Add(columnName.ToString(), _listViewWidth / columns.Count, HorizontalAlignment.Left);
            }

            listVeiw.Items.AddRange(items);

            Controls.Add(listVeiw);
        }

        private void ListViewInitilze(ListView listView)
        {
            listView.AllowColumnReorder = true;
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Location = new Point(10, 10);
            listView.Size = new Size(_listViewWidth, _listViewHieght);
        }
    }
}
