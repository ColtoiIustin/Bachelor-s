using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatabaseModel;
using System.Data.Entity;
using System.Data;


namespace Proiect_Licenta
{

    public partial class StocPage : UserControl
    {

        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource stocVSource;

        public StocPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            stocVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stocViewSource")));
            stocVSource.Source = ctx.Stoc.Local;
            ctx.Stoc.Load();

        }

        private void btnAddStoc_Click(object sender, RoutedEventArgs e)
        {

            Stoc stoc = null;
            try
            {
                if (produsTextBox.Text.Trim().Length > 2)
                {
                    stoc = new Stoc()
                    {
                        Produs = produsTextBox.Text.Trim(),
                        Cantitate = decimal.Parse(cantitateTextBox.Text.Trim()),
                        Pret = decimal.Parse(pretTextBox.Text.Trim()),


                    };
                    ctx.Stoc.Add(stoc);
                    ctx.SaveChanges();
                    stocVSource.View.Refresh();
                }
                else MessageBox.Show("Campuri goale");
            }
            catch (Exception)
            {
                MessageBox.Show("Campuri goale");
            }
        }


            private void btnEditStoc_Click(object sender, RoutedEventArgs e)
            {
          

            Stoc stoc = null;
            try
            {


                stoc = (Stoc)stocDataGrid.SelectedItem;
                stoc.Produs = produsTextBox.Text.Trim();
                stoc.Cantitate = decimal.Parse(cantitateTextBox.Text.Trim());
                stoc.Pret = decimal.Parse(pretTextBox.Text.Trim());
                
                ctx.SaveChanges();
                stocVSource.View.Refresh();


             }
             catch (Exception)
             {
                 MessageBox.Show("Acest camp nu poate fi editat");
             }
             }

              private void btnDeleteStoc_Click(object sender, RoutedEventArgs e)
            {


            Stoc stoc = null;
            try
            {
                stoc = (Stoc)stocDataGrid.SelectedItem;
                ctx.Stoc.Remove(stoc);
                ctx.SaveChanges();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            stocVSource.View.Refresh();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "." && IsNumber(e.Text) == false)
            {
                e.Handled = true;
            }
            else if (e.Text == ".")
            {
                if (((TextBox)sender).Text.IndexOf(e.Text) > -1)
                {
                    e.Handled = true;
                }
            }
        }
        private bool IsNumber(string Text)
        {
            int output;
            return int.TryParse(Text, out output);
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
