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
    
    public partial class MainWindow : Window
    {
        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource IstoricVanzariVSource;
        public MainWindow()
        {
            
            InitializeComponent();
            btnPlantatie.IsChecked = true;
            ctx.Logare.Load();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            IstoricVanzariVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("IstoricVanzariViewSource")));
            IstoricVanzariVSource.Source = ctx.IstoricVanzari.Local;
            ctx.IstoricVanzari.Load();
        }


        public void SetActiveUserControl(UserControl control)
        {
            plantatie.Visibility = Visibility.Collapsed;
            vanzari.Visibility = Visibility.Collapsed;
            parole.Visibility = Visibility.Collapsed;
            
            angajati.Visibility = Visibility.Collapsed;

            control.Visibility = Visibility.Visible;
        }

        private void btnPlantatie_Checked(object sender, RoutedEventArgs e)
        {
            SetActiveUserControl(plantatie);
        }

        private void btnVanzari_Checked(object sender, RoutedEventArgs e)
        {
            SetActiveUserControl(vanzari);

        }

       

        private void btnAngajati_Checked(object sender, RoutedEventArgs e)
        {
            SetActiveUserControl(angajati);
        }

        private void plantatie_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnParole_Checked(object sender, RoutedEventArgs e)
        {
            parole.Visibility = Visibility.Visible;
            IstoricVanzariVSource.Source = ctx.IstoricVanzari.Local;
            ctx.IstoricVanzari.Load();
            IstoricVanzariVSource.View.Refresh();
        }

        private void btnLogare_Click(object sender, RoutedEventArgs e)
        {
           
            btnPlantatie.IsChecked = true;
           
            if (cmbCont.SelectedItem != null)
            {

                int id = 1;
                if (cmbCont.Text == "Angajat")  id = 2;


                if (loginTextBox.Password == ctx.Logare.Find(id).Parola)
                {
                    if (id == 2)
                    {
                        btnAngajati.IsEnabled = false;
                        btnParole.IsEnabled = false;
                    }

                    gridLogare.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("    Parola Incorecta!    ");
                    loginTextBox.Password = "";

                }
                loginTextBox.Password = "";
            }
            else MessageBox.Show("  Selectati tipul de cont! ");

        }

        private void imgLogin_Click(object sender, MouseButtonEventArgs e)
        {
            gridLogare.Visibility = Visibility.Visible;

            parole.Visibility = Visibility.Collapsed;
            btnAngajati.IsEnabled = true;
            btnParole.IsEnabled = true;

        }

        private void loginTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            btnPlantatie.IsChecked = true;
            if (e.Key == Key.Enter)
            {
                if (cmbCont.SelectedItem != null)
                {

                    int id = 1;
                    if (cmbCont.Text == "Angajat") id = 2;


                    if (loginTextBox.Password == ctx.Logare.Find(id).Parola)
                    {
                        if (id == 2)
                        {
                            btnAngajati.IsEnabled = false;
                            btnParole.IsEnabled = false;
                        }

                        gridLogare.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("    Parola Incorecta!    ");
                        loginTextBox.Password = "";

                    }
                    loginTextBox.Password = "";
                }
                else MessageBox.Show("  Selectati tipul de cont! ");

            }
        }

        private void gridLogare_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
            DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
            ctx.Logare.Load();
            
            
        }
        private void btnSchimba_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCont.SelectedItem != null)
            {
                int id = 1;
                if (cmbCont.Text == "Angajat") id = 2;
                if (parolaCurenta.Password == ctx.Logare.Find(id).Parola && confirmaParola.Password == parolaNoua.Password)
                {
                    ctx.Logare.Find(id).Parola = parolaNoua.Password;
                    ctx.SaveChanges();
                    MessageBox.Show("  Parola a fost schimbata cu succes!  ");
                    parolaCurenta.Password = "";
                    parolaNoua.Password = "";
                    confirmaParola.Password = "";
                }
                else MessageBox.Show("    Parola incorecta!    ");
            }
            else MessageBox.Show("  Selectati tipul de cont! ");
        }

        private void imgExit_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btnStergeVanzare_Click(object sender, RoutedEventArgs e)
        {  if (istoricDataGrid.SelectedItem != null)
            {
                IstoricVanzari IstoricVanzari = null;
                try
                {
                    IstoricVanzari = (IstoricVanzari)istoricDataGrid.SelectedItem;

                    ctx.IstoricVanzari.Remove(IstoricVanzari);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                IstoricVanzariVSource.View.Refresh();
            }
            else MessageBox.Show("Selectati un element din tabel!");
        }
    }
}
