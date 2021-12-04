using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ContactsDB.Domain.Models;
using ContactsDB.Infrastructure.Repository;

namespace Contacts_DB_WPF_UI.ViewModels
{
    public class ZipViewModel : ViewModelBase, IDataErrorInfo
    {
        public RelayCommand SelectCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }
        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }

        private Zipcode model = new Zipcode();        
        private ApplicationContext applicationContext;
        private ZipcodeRepository zipRepo;
        private ObservableCollection<Zipcode> data;

        // TODO: DI the zipRepo into CTOR instead of newing it up inside CTOR
        public ZipViewModel()
        {
            applicationContext = new ApplicationContext();
            zipRepo = new ZipcodeRepository(applicationContext);
            zipRepo.RepositoryChanged += ModelChanged;
            Search();
            UpdateCommand = new RelayCommand(p => Update(), p => CanUpdate());
            SelectCommand = new RelayCommand(p => Search());
            ClearCommand = new RelayCommand(p => Clear());
            InsertCommand = new RelayCommand(p => Add(), p => CanAdd());
            RemoveCommand = new RelayCommand(p => Remove(), p => CanRemove());
        }

        public ObservableCollection<Zipcode> Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    OnPropertyChanged("Data");
                }
            }
        }

        public void ModelChanged(object sender, DbEventArgs e)
        {
            if (e.Operation != DbOperation.SELECT)
            {
                Clear();
            }
            Data = new ObservableCollection<Zipcode>(zipRepo);
        }

        public string Code
        {
            get 
            { 
                // TODO: figure out why model prop returns null when searching with select button in ZipWindow
                if (model == null) { model = new Zipcode(); }
                return model.Code; 
            }
            set
            {
                if (!model.Code.Equals(value))
                {
                    model.Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public string City
        {
            get 
            {
                // TODO: figure out why model prop returns null when searching with select button in ZipWindow
                if (model == null) { model = new Zipcode(); }
                return model.City; 
            }
            set
            {
                if (!model.City.Equals(value))
                {
                    model.City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        public Zipcode SelectedModel
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged("Code");
                OnPropertyChanged("City");
                OnPropertyChanged("SelectedModel");
            }
        }

        private void Clear()
        {
            SelectedModel = null;
            model = new Zipcode();

            // set up a fresh data source for the grid            
            Data = new ObservableCollection<Zipcode>();

            OnPropertyChanged("Code");
            OnPropertyChanged("City");
            OnPropertyChanged("SelectedModel");
        }

        public void Search()
        {          
            IEnumerable<Zipcode> searchResults = zipRepo.Select
                (zipcode => zipcode.Code.StartsWith(Code) && zipcode.City.StartsWith(City));
            
            // clear grid and set a new datasource
            Clear(); 
            foreach (Zipcode result in searchResults)
            {
                Data.Add(new Zipcode { Code = result.Code.ToString(), City = result.City.ToString() });
            }          
        }

        public void Update()
        {
            try
            {
                Zipcode updateZipcode = new Zipcode(Code, City);
                applicationContext.ChangeTracker.Clear();

                zipRepo.Update(updateZipcode);
                zipRepo.SaveChanges();                
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        private bool CanUpdate()
        {
            return model.IsValid;
        }

        public void Add()
        {
            try
            {
                Zipcode insertZipcode = new Zipcode(Code, City);                
                zipRepo.Insert(insertZipcode);
                zipRepo.SaveChanges();                
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        public bool CanAdd()
        {
            return model.IsValid;
        }

        public void Remove()
        {
            try
            {
                Zipcode removeZipcode = new Zipcode(Code, City);
                applicationContext.ChangeTracker.Clear();

                zipRepo.Remove(removeZipcode.Code);
                zipRepo.SaveChanges();
                Clear();               
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        private bool CanRemove()
        {
            return Code != null && Code.Length > 0;
        }

        string IDataErrorInfo.Error
        {
            get { return (model as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                try
                {
                    error = (model as IDataErrorInfo)[propertyName];
                }
                catch
                {
                }
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }
    }
}