using System;
using System.Windows;
using ContactsDB.Domain.Models;
using Contacts_DB_WPF_UI.ViewModels;

namespace Contacts_DB_WPF_UI.Views
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private ContactViewModel model;

        public UpdateWindow(ZipContactVM zipContactVM)
        {
            model = new ContactViewModel(zipContactVM, MainViewModel.contactRepo);
            InitializeComponent();
            model.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            model.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            DataContext = model;
        }
    }
}
