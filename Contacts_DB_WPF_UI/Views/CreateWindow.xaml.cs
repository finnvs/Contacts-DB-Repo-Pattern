using System;
using System.Windows;
using ContactsDB.Domain.Models;
using Contacts_DB_WPF_UI.ViewModels;

namespace Contacts_DB_WPF_UI.Views
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        private ContactViewModel model = new ContactViewModel(new ZipContactVM(new Contact(), new Zipcode()), MainViewModel.contactRepo);

        public CreateWindow()
        {
            InitializeComponent();
            model.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            model.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            DataContext = model;
        }
    }
}
