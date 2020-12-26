using MSAccessApp.Persistence;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MSAccessApp.Forms
{
    public partial class StartMenu : Form
    {
        private readonly IDatabaseProvider _databaseProvider;

        public StartMenu(IDatabaseProvider databaseProvider)
        {
            this.BackColor = Color.DarkOliveGreen;
            _databaseProvider = databaseProvider;
            InitializeComponent();
        }

        private void HadnleOpenFormClick(object sender, EventArgs eventArgs)
        {
            var button = sender as Button;

            if (button == null) { return; }

            AllEntitiesFromTableFormButton.BackColor = Color.DarkSeaGreen;
            AddEntityFormButton.BackColor = Color.DarkSeaGreen;
            RemoveEntityFromTableFormButton.BackColor = Color.DarkSeaGreen;
            EditEntityFromTableFormButton.BackColor = Color.DarkSeaGreen;
            MSysObjectsFormsButton.BackColor = Color.DarkSeaGreen;
            QueryAndFromsFromButton.BackColor = Color.DarkSeaGreen;

            button.BackColor = Color.LimeGreen;

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
