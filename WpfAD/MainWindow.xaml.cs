using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfAD.Models;
using WpfAD.services;

namespace WpfAD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IFileManager _fileManager = new FileManager();
        private ObservableCollection<ContactPerson> contacts; // Det sk finnas en lista
        private string _filePath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\File.Json";
        public MainWindow()
        {
            InitializeComponent();
            contacts = new ObservableCollection<ContactPerson>(); //Skapar en instanciering av lista
            lv_Contacts.ItemsSource = contacts; // Källan som som ska hämta uppgifterna ifrån är contacts lista
        }

        private void tbn_Add_Click(object sender, RoutedEventArgs e) //Lägg till metod
        {
            var contact = contacts.FirstOrDefault(x => x.Email == tb_Email.Text);
            if(contact == null)
            {
                contacts.Add(new ContactPerson
                {
                    FirstName = tb_FirstName.Text,
                    LastName = tb_LastName.Text,
                    Email = tb_Email.Text,
                    StreetName = tb_StreetName.Text,
                    PostalCode = tb_PostalCode.Text,
                    City = tb_City.Text,
                });  // Kontakter stoppas in direkt i listan utan behov till variabler

                _fileManager.Save(_filePath, JsonConvert.SerializeObject(contact));
            }
            
            else
            {
                MessageBox.Show("Det finns redan en kontakt med samma e-postadress.");
            }
;

            ClearFields();
        } 
        private void ClearFields()
        {
            tb_FirstName.Text = "";
            tb_LastName.Text = "";
            tb_Email.Text = "";
            tb_StreetName.Text = "";
            tb_PostalCode.Text = "";
            tb_City.Text = "";
        }

        private void tbn_Remove_Click(object sender, RoutedEventArgs e) // Borttagning fuktion
        {
            var button = sender as Button;
            var contact = (ContactPerson)button!.DataContext;

            contacts.Remove(contact);

            ClearFields();
        }

        private void lv_Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var contact = (ContactPerson)lv_Contacts.SelectedItems[0]!;

                tb_FirstName.Text = contact.FirstName;
                tb_LastName.Text = contact.LastName;
                tb_Email.Text = contact.Email;
                tb_StreetName.Text = contact.StreetName;
                tb_PostalCode.Text = contact.PostalCode;
                tb_City.Text = contact.City; 
            }
            catch { }                    
        }
        

        private void tbn_Update_Click(object sender, RoutedEventArgs e)
        {
           var contact = (ContactPerson)lv_Contacts.SelectedItems[0]!;
            var index = lv_Contacts.Items.IndexOf(contact);
            try
            {
                contacts[index].FirstName = tb_FirstName.Text;
                contacts[index].LastName = tb_LastName.Text;
                contacts[index].Email = tb_Email.Text;
                contacts[index].StreetName = tb_StreetName.Text;
                contacts[index].PostalCode = tb_PostalCode.Text;
                contacts[index].City = tb_City.Text;
            }
            catch { }
            lv_Contacts.Items.Refresh();
            ClearFields();
            _fileManager.Save(_filePath, JsonConvert.SerializeObject(contact));
            try { contacts = JsonConvert.DeserializeObject<ObservableCollection<ContactPerson>>(_fileManager.Read(_filePath)); } //försöker att läsa in listan från en Json-fil
            catch { }

        }
    }
}
