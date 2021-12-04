using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Contacts_DB_WPF_UI.ViewModels;
using ContactsDB.Domain.Models;

namespace Contacts_DB_WPF_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel model = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
            model.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    DataGridRow row = sender as DataGridRow;                    
                    ZipContactVM zipContactVM = (ZipContactVM)row.Item;
                    model.UpdateContact(zipContactVM);
                }
                catch (Exception ex)
                {
                    model.OnWarning(ex.Message);
                }
            }
        }
    }
}
