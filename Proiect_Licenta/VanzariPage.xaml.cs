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
    
    public partial class VanzariPage : UserControl
    {
        decimal? total=0;
       
        
        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource stocVSource;
        public VanzariPage()
        {
            InitializeComponent();
            DataContext = this;
            
        }

        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            stocVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stocViewSource")));
            stocVSource.Source = ctx.Stoc.Local;
            ctx.Stoc.Load();
            lbltotal.Content = total + " RON";


        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            Stoc stoc = null;
            try
            {
                stoc = (Stoc)stocDataGrid.SelectedItem;
                if (stoc != null)
                {   if (decimal.Parse(txtCantitate.Text) > stoc.Cantitate)
                        MessageBox.Show("Cantitatea introdusa depaseste stocul disponibil");

                    else
                    {
                        lstVanzare.Items.Add(stoc.Pret * decimal.Parse(txtCantitate.Text) + " RON    " + stoc.Produs + "   -" + txtCantitate.Text + "  kg/litri");
                        total = total + (stoc.Pret * decimal.Parse(txtCantitate.Text));
                        lbltotal.Content = total + " RON";
                    }
                    

                    
                    
                }
                else MessageBox.Show("Selectati un produs");

                ctx.SaveChanges();
                stocVSource.View.Refresh();
            }
            catch (Exception )
            {
                MessageBox.Show("Selectati canitatea!");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
            stocVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stocViewSource")));
            stocVSource.Source = ctx.Stoc.Local;
            ctx.Stoc.Load();
            stocVSource.View.Refresh();
        }

        private void btnVanzare_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (string s in lstVanzare.Items)
            {
                string numprodus = s.Substring(s.IndexOf("RON") + 7, s.IndexOf("-") - s.IndexOf("RON") -10);

                Stoc stoc = (from d in ctx.Stoc
                             where d.Produs.Equals(numprodus)
                             select d).First();

                stoc.Cantitate = stoc.Cantitate - decimal.Parse(s.Substring(s.IndexOf("-") + 1, s.IndexOf("kg") - s.IndexOf("-") - 3));

                
                ctx.SaveChanges();
                stocVSource.View.Refresh();

            }
            lstVanzare.Items.Clear();

            total = 0;
            lbltotal.Content = total + " RON";
        }

        private void btnSterge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstVanzare.SelectedItem != null)
                {
                    string s = lstVanzare.SelectedItem.ToString();
                    total = total - decimal.Parse((s.Substring(0, s.IndexOf("RON"))));
                    lbltotal.Content = total + " RON";
                    lstVanzare.Items.RemoveAt(lstVanzare.Items.IndexOf(lstVanzare.SelectedItem));
                    lstVanzare.UnselectAll();
                }
                else MessageBox.Show("Selectati un element din lista");
            }
            catch (Exception)
            {

                MessageBox.Show("Selectati un element din lista");
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }
    }
}

