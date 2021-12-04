using System;
using System.Collections.ObjectModel;
using ContactsDB.Domain.Models;
using ContactsDB.Infrastructure.Repository;
using Contacts_DB_WPF_UI.Views;
using System.Collections.Generic;

namespace Contacts_DB_WPF_UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand ZipCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        // TODO: DI contact repo instead
        public static ContactRepository contactRepo;
        private ZipcodeRepository zipRepo;
        private ObservableCollection<Contact> contacts;
        private ObservableCollection<ZipContactVM> extendedContacts;
        private ApplicationContext applicationContext;

        private string phone = "";
        private string fname = "";
        private string lname = "";
        private string addr = "";
        private string code = "";
        private string city = "";
        private string email = "";
        private string title = "";

        // TODO: DI contact repo instead
        public MainViewModel()
        {
            applicationContext = new ApplicationContext();
            contactRepo = new ContactRepository(applicationContext);
            zipRepo = new ZipcodeRepository(applicationContext);
            SearchCommand = new RelayCommand(p => Search(), p => CanSearch());
            CreateCommand = new RelayCommand(p => new CreateWindow().ShowDialog());
            ZipCommand = new RelayCommand(p => new ZipWindow().ShowDialog());
            ClearCommand = new RelayCommand(p => Clear());
            contacts = new ObservableCollection<Contact>(contactRepo);
            extendedContacts = new ObservableCollection<ZipContactVM>();            
            contactRepo.RepositoryChanged += Refresh;
            
            // Sæt grid op med søgning på postnr 9000 Aalborg            
            Code = "9000";
            Search();
        }

        private void Refresh(object sender, DbEventArgs e)
        {
            Contacts = new ObservableCollection<Contact>(contactRepo);
            extendedContacts = new ObservableCollection<ZipContactVM>();
        }

        public ObservableCollection<Contact> Contacts
        {
            get { return contacts; }
            set
            {
                if (!contacts.Equals(value))
                {
                    contacts = value;
                    OnPropertyChanged("Contacts");
                }
            }
        }

        public ObservableCollection<ZipContactVM> ExtendedContacts
        {
            get { return extendedContacts; }
            set
            {
                if (!extendedContacts.Equals(value))
                {
                    extendedContacts = value;
                    OnPropertyChanged("ExtendedContacts");
                }
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (!phone.Equals(value))
                {
                    phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }

        public string Fname
        {
            get { return fname; }
            set
            {
                if (!fname.Equals(value))
                {
                    fname = value;
                    OnPropertyChanged("Fname");
                }
            }
        }

        public string Lname
        {
            get { return lname; }
            set
            {
                if (!lname.Equals(value))
                {
                    lname = value;
                    OnPropertyChanged("Lname");
                }
            }
        }

        public string Addr
        {
            get { return addr; }
            set
            {
                if (!addr.Equals(value))
                {
                    addr = value;
                    OnPropertyChanged("Addr");
                }
            }
        }

        public string Code
        {
            get { return code; }
            set
            {
                if (!code.Equals(value))
                {
                    code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (!city.Equals(value))
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (!email.Equals(value))
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (!title.Equals(value))
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private void Clear()
        {
            Phone = "";
            Fname = "";
            Lname = "";
            Addr = "";
            Code = "";
            City = "";
            Email = "";
            Title = "";

            extendedContacts = new ObservableCollection<ZipContactVM>();
            ExtendedContacts = extendedContacts;
            OnPropertyChanged("ExtendedContacts");
        }

        private void Search()
        {
            try
            {                
                IEnumerable<Contact> searchResults = contactRepo.Select
                (contact => contact.Phone.StartsWith(Phone)
                    && contact.Firstname.StartsWith(Fname) 
                    && contact.Lastname.StartsWith(Lname) 
                    && contact.Address.StartsWith(Addr)
                    && contact.Zipcode.StartsWith(Code)
                    && contact.Email.StartsWith(Email)
                    && contact.Title.StartsWith(Title));

                // Clear grid og sæt ny datasource 'extendedContacts' i View File
                Clear();
                // ViewModel 'ZipContactVM' bygges op og sendes med ud til viewet
                foreach (Contact result in searchResults)
                {
                    extendedContacts.Add(new ZipContactVM(result, zipRepo.ReturnZipCode(result.Zipcode)));
                }
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        public void UpdateContact(ZipContactVM contact)
        {
            UpdateWindow dlg = new UpdateWindow(contact);
            dlg.ShowDialog();
        }

        private bool CanSearch()
        {
            return phone.Length > 0 || fname.Length > 0 ||
                      lname.Length > 0 || addr.Length > 0 ||
                      code.Length > 0 || city.Length > 0 ||
                      title.Length > 0;
        }
    }
}