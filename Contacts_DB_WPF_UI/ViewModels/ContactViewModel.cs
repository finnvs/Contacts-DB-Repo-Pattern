using System;
using System.Windows.Input;
using System.ComponentModel;
using ContactsDB.Domain.Models;
using ContactsDB.Infrastructure.Repository;

namespace Contacts_DB_WPF_UI.ViewModels
{
    public class ContactViewModel : ViewModelBase, IDataErrorInfo
    {
        public RelayCommand OkCommand { get; private set; }
        public RelayCommand ModCommand { get; private set; }
        public RelayCommand RemCommand { get; private set; }
        protected ZipContactVM model;
        protected ContactRepository repository;

        public ContactViewModel(ZipContactVM model, ContactRepository repository)
        {
            this.model = model;
            this.repository = repository;
            OkCommand = new RelayCommand(p => Add(), p => CanUpdate());            
            ModCommand = new RelayCommand(p => Update(), p => CanUpdate());
            RemCommand = new RelayCommand(p => Remove());
        }

        public ZipContactVM Model
        {
            get { return model; }
        }

        public string Phone
        {
            get { return model.Contact.Phone; }
            set
            {
                if (!model.Contact.Phone.Equals(value))
                {
                    model.Contact.Phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }

        public string Firstname
        {
            get { return model.Contact.Firstname; }
            set
            {
                if (!model.Contact.Firstname.Equals(value))
                {
                    model.Contact.Firstname = value;
                    OnPropertyChanged("Firstname");
                }
            }
        }

        public string Lastname
        {
            get { return model.Contact.Lastname; }
            set
            {
                if (!model.Contact.Lastname.Equals(value))
                {
                    model.Contact.Lastname = value;
                    OnPropertyChanged("Lastname");
                }
            }
        }

        public string Address
        {
            get { return model.Contact.Address; }
            set
            {
                if (!model.Contact.Address.Equals(value))
                {
                    model.Contact.Address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        public string Zipcode
        {
            get { return model.Contact.Zipcode; }
            set
            {
                if (!model.Contact.Zipcode.Equals(value))
                {
                    model.Contact.Zipcode = value;
                    OnPropertyChanged("Zipcode");
                }
            }
        }

        public string City
        {
            get { return model.Zipcode.City; }
            set
            {
                if (!model.Zipcode.City.Equals(value))
                {
                    model.Zipcode.City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        public string Email
        {
            get { return model.Contact.Email; }
            set
            {
                if (!model.Contact.Email.Equals(value))
                {
                    model.Contact.Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Title
        {
            get { return model.Contact.Title; }
            set
            {
                if (!model.Contact.Title.Equals(value))
                {
                    model.Contact.Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public void Clear()
        {
            model = new ZipContactVM();
            OnPropertyChanged("Phone");
            OnPropertyChanged("Lastname");
            OnPropertyChanged("Firstname");
            OnPropertyChanged("Address");
            OnPropertyChanged("Zipcode");
            OnPropertyChanged("City");
            OnPropertyChanged("Email");
            OnPropertyChanged("Title");
        }

        public void Update()
        {
            if (IsValid)
            {
                try
                {
                    repository.Update(model.Contact);
                    repository.SaveUpdatedContact();
                    OnClose();
                }
                catch (Exception ex)
                {
                    OnWarning(ex.Message);
                }
            }
        }

        public void Remove()
        {
            try
            {
                repository.Remove(model.Contact);
                OnClose();
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        public void Add()
        {
            if (IsValid)
            {
                try
                {
                    repository.Add(model.Contact);
                    OnClose();                    
                }
                catch (Exception ex)
                {
                    OnWarning(ex.Message);
                }
            }
        }

        string IDataErrorInfo.Error
        {
            get { return (model.Contact as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                try
                {
                    error = (model.Contact as IDataErrorInfo)[propertyName];
                }
                catch
                {
                }
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        private bool IsValid
        {
            get { return model.Contact.IsValid; }
        }

        private bool CanUpdate()
        {
            return IsValid;
        }
    }
}
