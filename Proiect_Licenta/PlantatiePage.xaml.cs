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

   
    public partial class PlantatiePage : UserControl
    {

        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource plantatieVSource;
        CollectionViewSource proiecteVSource;

        
        public PlantatiePage()
        {
            InitializeComponent();
            DataContext = this;

        }


        decimal suprafata = 0;
        decimal procent;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            
            plantatieVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("plantatieViewSource")));
            plantatieVSource.Source = ctx.Plantatie.Local;
            ctx.Plantatie.Load();

            proiecteVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("proiecteViewSource")));
            proiecteVSource.Source = ctx.Proiecte.Local;
            ctx.Proiecte.Load();

            foreach (Plantatie item in ctx.Plantatie )
            {
                suprafata = suprafata + decimal.Parse(item.SuprafataCultivata);
            }

            Valoare.Content = suprafata;
            Progressbar.Value = double.Parse(suprafata.ToString());
            procent = (suprafata / 250) * 100;
            int x = (int)procent;
            txtProcente.Text = x.ToString() + "%";
            


        }

       
        

        private void btnAddPlanta_Click(object sender, RoutedEventArgs e)
        {

            Plantatie plantatie = null;
            try
            {
                if (plantaTextBox.Text.Trim().Length > 2)
                {
                    if (suprafata + decimal.Parse(suprafataCultivataTextBox.Text) <= 250)
                    {
                        plantatie = new Plantatie()
                        {
                            Planta = plantaTextBox.Text.Trim(),
                            Soi = soiTextBox.Text.Trim(),
                            SuprafataCultivata = suprafataCultivataTextBox.Text.Trim(),
                            Recoltare = recoltareTextBox.Text.Trim()

                        };
                        ctx.Plantatie.Add(plantatie);
                        suprafata = suprafata + decimal.Parse(suprafataCultivataTextBox.Text);
                        Valoare.Content = suprafata;
                        Progressbar.Value = double.Parse(suprafata.ToString());
                        procent = (suprafata / 250) * 100;
                        int x = (int)procent;
                        txtProcente.Text = x.ToString() + "%";

                        ctx.SaveChanges();
                        plantatieVSource.View.Refresh();
                    } else MessageBox.Show("Suprafata disponibila este depasita!");
                }
                else MessageBox.Show("Campuri goale");


            }
            catch (Exception)
            {
                MessageBox.Show("Campuri goale");
            }


        }

        private void btnEditPlanta_Click(object sender, RoutedEventArgs e)
        {
            Plantatie plantatie = null;
            try
            {  
                plantatie = (Plantatie)plantatieDataGrid.SelectedItem;
                if (plantatieDataGrid.SelectedItem != null)
                {
                    if (suprafata - decimal.Parse(plantatie.SuprafataCultivata) + decimal.Parse(suprafataCultivataTextBox.Text) <= 250)
                    {
                        suprafata = suprafata - decimal.Parse(plantatie.SuprafataCultivata);
                        plantatie.Planta = plantaTextBox.Text.Trim();
                        plantatie.Soi = soiTextBox.Text.Trim();
                        plantatie.SuprafataCultivata = suprafataCultivataTextBox.Text.Trim();
                        plantatie.Recoltare = recoltareTextBox.Text.Trim();

                        suprafata = suprafata + decimal.Parse(suprafataCultivataTextBox.Text);
                        Valoare.Content = suprafata;
                        Progressbar.Value = double.Parse(suprafata.ToString());
                        procent = (suprafata / 250) * 100;
                        int x = (int)procent;
                        txtProcente.Text = x.ToString() + "%";

                        ctx.SaveChanges();
                        plantatieVSource.View.Refresh();

                    }
                    else MessageBox.Show("Suprafata disponibila este depasita!");
                }
                else MessageBox.Show("Selectati un item din tabel");
            }
            catch (Exception)
            {
                MessageBox.Show("Acest camp nu poate fi editat");
            }

        }

        private void btnDeletePlanta_Click(object sender, RoutedEventArgs e)
        {
            if (plantatieDataGrid.SelectedItem != null)
            {
                Plantatie plantatie = null;
                try
                {
                    plantatie = (Plantatie)plantatieDataGrid.SelectedItem;
                    suprafata = suprafata - decimal.Parse(plantatie.SuprafataCultivata);
                    Valoare.Content = suprafata;
                    Progressbar.Value = double.Parse(suprafata.ToString());
                    procent = (suprafata / 250) * 100;
                    int x = (int)procent;
                    txtProcente.Text = x.ToString() + "%";
                    ctx.Plantatie.Remove(plantatie);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                plantatieVSource.View.Refresh();
            }
            else MessageBox.Show("Selectati un element din tabel!");
            
        }

        private void btnAddProiect_Click(object sender, RoutedEventArgs e)
        {
            if (dataInceperiiDatePicker.SelectedDate <= dataFinalizariiDatePicker.SelectedDate)
            {
                Proiecte proiecte = null;

                if (proiectTextBox.Text.Trim().Length > 2)
                {

                    proiecte = new Proiecte()
                    {
                        Proiect = proiectTextBox.Text.Trim(),
                        DataInceperii = dataInceperiiDatePicker.SelectedDate,
                        DataFinalizarii = dataFinalizariiDatePicker.SelectedDate
                    };
                    ctx.Proiecte.Add(proiecte);

                    ctx.SaveChanges();
                    proiecteVSource.View.Refresh();
                }
                else MessageBox.Show("Campuri goale");
            }
            else MessageBox.Show("Data inceperii este mai mare decat data finalizarii!");




        }

        private void btnEditProiect_Click(object sender, RoutedEventArgs e)
        {
            if (dataInceperiiDatePicker.SelectedDate <= dataFinalizariiDatePicker.SelectedDate)
            {
                Proiecte proiecte = null;
                try
                {

                    proiecte = (Proiecte)proiecteDataGrid.SelectedItem;
                    if (proiectTextBox.Text.Trim().Length > 2)
                    {
                        proiecte.Proiect = proiectTextBox.Text.Trim();
                        proiecte.DataInceperii = dataInceperiiDatePicker.SelectedDate;
                        proiecte.DataFinalizarii = dataFinalizariiDatePicker.SelectedDate;
                        ctx.SaveChanges();
                        proiecteVSource.View.Refresh();
                    }
                    else MessageBox.Show("Campuri libere");


                }
                catch (Exception)
                {
                    MessageBox.Show("Acest camp nu poate fi editat");
                } 
            }
            else MessageBox.Show("Data inceperii este mai mare decat data finalizarii!");
        }

        private void btnDeleteProiect_Click(object sender, RoutedEventArgs e)
        {
            if (proiecteDataGrid.SelectedItem != null)
            {
                Proiecte proiecte = null;
                try
                {
                    proiecte = (Proiecte)proiecteDataGrid.SelectedItem;
                    ctx.Proiecte.Remove(proiecte);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                proiecteVSource.View.Refresh();
            }
            else MessageBox.Show("Selectati un element din tabel!");
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



        
    }


}
