using System;
using System.Windows;
using Contacts_DB_WPF_UI.ViewModels;
using ContactsDB.Domain.Models;
using ContactsDB.Infrastructure.Repository;

namespace Contacts_DB_WPF_UI.Views
{
    /// <summary>
    /// Interaction logic for ZipWindow.xaml
    /// </summary>
    public partial class ZipWindow : Window
    {
        private ZipViewModel model = new ZipViewModel();       

        public ZipWindow()
        {
            InitializeComponent();
            model.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            model.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            DataContext = model;
        }
    }
}
