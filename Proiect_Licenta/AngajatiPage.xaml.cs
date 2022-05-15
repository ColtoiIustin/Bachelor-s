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
using System.Net.Mail;

namespace Proiect_Licenta
{
 
    public partial class AngajatiPage : UserControl
    {

        DatabaseEntitiesModel ctx = new DatabaseEntitiesModel();
        CollectionViewSource angajatiVSource;

        public AngajatiPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            angajatiVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("angajatiViewSource")));
            angajatiVSource.Source = ctx.Angajati.Local;
            ctx.Angajati.Load();

        }

        private void btnAddAngajat_Click(object sender, RoutedEventArgs e)
        {
            Angajati angajati = null;

            try
            {   if (numeTextBox.Text.Trim().Length > 2)
                {
                    angajati = new Angajati()
                    {
                        Nume = numeTextBox.Text.Trim(),
                        Prenume = prenumeTextBox.Text.Trim(),
                        Email = emailTextBox.Text.Trim()


                    };
                    ctx.Angajati.Add(angajati);
                    ctx.SaveChanges();
                    angajatiVSource.View.Refresh();
                }
                else MessageBox.Show("Campuri goale");

            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eroare");
            }
        }

        private void btnEditAngajat_Click(object sender, RoutedEventArgs e)
        {
            Angajati angajati = null;
            try
            {
                if (angajatiDataGrid.SelectedItem != null)

                {
                    angajati = (Angajati)angajatiDataGrid.SelectedItem;
                    angajati.Nume = numeTextBox.Text.Trim();
                    angajati.Prenume = prenumeTextBox.Text.Trim();
                    angajati.Email = emailTextBox.Text.Trim();

                    ctx.SaveChanges();
                    angajatiVSource.View.Refresh();

                }
                else MessageBox.Show("Selectati un item din tabel");
            }
            catch (Exception)
            {
                MessageBox.Show("Acest camp nu poate fi editat");
            }
        }

        private void btnDeleteAngajat_Click(object sender, RoutedEventArgs e)
        {
            if (angajatiDataGrid.SelectedItem != null)
            {
                Angajati angajati = null;
                try
                {
                    angajati = (Angajati)angajatiDataGrid.SelectedItem;
                    ctx.Angajati.Remove(angajati);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                angajatiVSource.View.Refresh();
            }
            else MessageBox.Show("Selectati un camp din tabel!");
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            bool gasit = true;
            Angajati angajati = null;
            try
            {
                angajati = (Angajati)angajatiDataGrid.SelectedItem;
                if (angajati == null) MessageBox.Show("Selectati un angajat");

                foreach (string s in lstAngajati.Items)
                {
                    if (s.Substring(0, s.IndexOf("-") - 3) == angajati.Nume && s.Substring(s.IndexOf("-") + 4, s.Length - s.IndexOf("-") - 4) == angajati.Prenume)
                        gasit = false;
                }
                
                if (gasit==true && angajati != null)
                {
                    lstAngajati.Items.Add(angajati.Nume  + "   -   " + angajati.Prenume);
                   
                }
                if(gasit==false) MessageBox.Show("Acest angajat este deja prezent in lista");
               

               
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eroare!");
            }
        }

        private void btnSterge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAngajati.SelectedItem != null)
                {

                    lstAngajati.Items.RemoveAt(lstAngajati.Items.IndexOf(lstAngajati.SelectedItem));
                    lstAngajati.UnselectAll();
                }

                else
                {
                    MessageBox.Show("Selectati un element din lista");
                    lstAngajati.SelectedIndex = 0;
                }

                    
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        // Trimitere mail
        private void btnTrimite_Click(object sender, RoutedEventArgs e)
        {

            if (lstAngajati.Items.Count >=1)
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                foreach (string s in lstAngajati.Items)
                {
                    string nume = s.Substring(0, s.IndexOf("-") - 3);
                    string prenume = s.Substring(s.IndexOf("-") + 4, s.Length - s.IndexOf("-") - 4);

                    Angajati angajati = (from d in ctx.Angajati
                                         where d.Nume.Equals(nume) && d.Prenume.Equals(prenume)
                                         select d).First();
                    mail.From = new MailAddress("isiasiasii@gmail.com");
                    mail.To.Add(angajati.Email);
                    string richText = new TextRange(mailTextBox.Document.ContentStart, mailTextBox.Document.ContentEnd).Text;
                    mail.Subject = "Mesaj important!";
                    mail.Body = richText;
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("isiasiasii@gmail.com", "asiisiisia");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);


                }
                lstAngajati.Items.Clear();
            }
            else MessageBox.Show("Adaugati destinatari!");
                      
        }

       
    }
}
