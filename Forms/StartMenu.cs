using MSAccessApp.Persistence;
using System;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class StartMenu : Form
    {
        private readonly IDatabaseProvider _databaseProvider;

        public StartMenu(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            InitializeComponent();
        }

        private void HadnleOpenFormClick(object sender, EventArgs eventArgs)
        {
            var button = sender as Button;

            if (button == null) { return; }

            // TODO вычислять имена кнопок налету через nameof([класс формы])
            Form form = button.Name switch
            {
                "AllEntitiesFromTableFormButton" => new AllEntitiesFromTableForm(_databaseProvider),
                "AddEntityFormButton" => new AddEntityForm(_databaseProvider),
                "RemoveEntityFromTableFormButton" => new RemoveEntityFromTableForm(_databaseProvider),
                "EditEntityFromTableFormButton" => new EditEntityFromTableForm(_databaseProvider),
                "MSysObjectsFormButton" => new MSysObjectsForm(_databaseProvider),
                "QueryAndFromsFromButton" => new QueryAndFromsFrom(_databaseProvider),
                 _ => null
            };

            form?.Show();
        }
    }
}
