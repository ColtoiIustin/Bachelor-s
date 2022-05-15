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
using Aspose.Words;
using Aspose.Words.Reporting;


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
            gridStoc.Visibility = Visibility.Hidden;

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

        public class Sender
        {
            public string Lista { get; set; }
            public string PretTotal { get; set; }
            public string DataFactura { get; set; }

            public Sender(string _lista, string _prettotal,string _datafactura)
            {
                Lista = _lista;
                PretTotal = _prettotal;
                DataFactura = _datafactura;
                
            }
        }
        string produse;
        private void btnVanzare_Click(object sender, RoutedEventArgs e)
        {
            

            foreach (string s in lstVanzare.Items)
            {
                string numprodus = s.Substring(s.IndexOf("RON") + 7, s.IndexOf("-") - s.IndexOf("RON") -10);

                Stoc stoc = (from d in ctx.Stoc
                             where d.Produs.Equals(numprodus)
                             select d).First();
                

                stoc.Cantitate = stoc.Cantitate - decimal.Parse(s.Substring(s.IndexOf("-") + 1, s.IndexOf("kg") - s.IndexOf("-") - 3));

                produse +=s + Environment.NewLine;
                

                ctx.SaveChanges();
                stocVSource.View.Refresh();

            }
            lstVanzare.Items.Clear();



            if (checkFactura.IsChecked == true)
            {   DateTime datafactura = DateTime.Now;
                Document doc = new Document("Template.docx");
                Sender senderr = new Sender(produse, total.ToString(),datafactura.ToString());
                ReportingEngine engine = new ReportingEngine();
                engine.BuildReport(doc, senderr, "sender");
                string day = (DateTime.Now.Day).ToString();
                string month = (DateTime.Now.Month).ToString();
                string year = (DateTime.Now.Year).ToString();
                string hour = (DateTime.Now.Hour).ToString();
                string minute = (DateTime.Now.Minute).ToString();

                doc.Save("Factura " + day + "." + month + "." + year + "  " + hour + "-" + minute + ".docx"); 
            }
            produse = null;
            



            IstoricVanzari IstoricVanzari = null;
            IstoricVanzari = new IstoricVanzari()
            {
                DataVanzare = DateTime.Now,
                Pret = total,                
            };
            ctx.IstoricVanzari.Add(IstoricVanzari);
            ctx.SaveChanges();
            

            total = 0;
            lbltotal.Content = total + " RON";

            ctx.SaveChanges();
            stocVSource.View.Refresh();
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
                else
                {
                    MessageBox.Show("Selectati un element din lista");
                    lstVanzare.SelectedIndex = 0;
                }
                }
            catch (Exception)
            {

                MessageBox.Show("Selectati un element din lista");
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }


        // functie ce permite doar introducerea de cifre si un singur punct intr-un TextBox:
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

        private void btnAddStoc_Click(object sender, RoutedEventArgs e)
        {

            Stoc stoc = null;

            if (double.Parse(cantitateTextBox.Text.Trim()) <= 999.99)
            {
                if (double.Parse(pretTextBox.Text.Trim()) <= 999.99)
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
                else MessageBox.Show("Pret incorect!"); 
            }
            else MessageBox.Show("Cantitate incorecta!");

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

            if (stocDataGrid.SelectedItem != null)
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

            else MessageBox.Show("Selectati un element din tabel!");
        }
        
        private void btnModificareStoc_Click(object sender, RoutedEventArgs e)
        {

            if (gridStoc.Visibility == Visibility.Hidden)
                gridStoc.Visibility = Visibility.Visible;
            else gridStoc.Visibility = Visibility.Hidden;

            lstVanzare.Items.Clear();
            total = 0;
            lbltotal.Content = total + " RON";

        }

        
    }
}

