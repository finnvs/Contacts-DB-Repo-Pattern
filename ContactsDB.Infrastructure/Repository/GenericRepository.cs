using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

namespace ContactsDB.Infrastructure.Repository
{
    // Fra oprindelig kodebase
    // Definerer typen af hhv. den databaseoperation, der er udført og af hvilket repository.
    public enum DbOperation { SELECT, INSERT, UPDATE, DELETE };
    public enum DbModeltype { Contact, Zipcodes }

    // EventArgs for en databaseoperation.
    public class DbEventArgs : EventArgs
    {
        public DbOperation Operation { get; private set; }  // databaseoperation
        public DbModeltype Modeltype { get; private set; }  // repository

        public DbEventArgs(DbOperation operation, DbModeltype modeltype)
        {
            Operation = operation;
            Modeltype = modeltype;
        }
    }

    // Exception type defineret for programmets repositories.
    public class DbException : Exception
    {
        public DbException(string message)
          : base(message)
        {
        }
    }

    public delegate void DbEventHandler(object sender, DbEventArgs e);
    // oprindelig kodebase slut

    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        
        protected ApplicationContext context;

        public GenericRepository(ApplicationContext applicationContext)
        {
            this.context = applicationContext;
        }

        public virtual T Delete(T entity)
        {
            return context.Remove(entity).Entity;
        }

        public virtual T Insert(T entity)
        {
            return context
            .Add(entity)
            .Entity;
        }

        public virtual IEnumerable<T> Select(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>()
            .AsQueryable()
            .Where(predicate).ToList();
        }

        public virtual T Update(T entity)
        {
            return context.Update(entity)
            .Entity;
        }

        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: Float a message back to the UI project
                String ErrMsg = ex.Message + " " + ex.InnerException;
                // Log and display message
                // LogDisplay(ErrMsg);
                //MessageBox.Show("Your attempted operation has been logged and HR has been notified.", "Illegal Operation.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Fra oprindelig kodebase (i princippet unødvendigt, da vi ikke laver SQL params og da vi bruger EF changetrackeren)
        public event DbEventHandler RepositoryChanged;
        public void OnChanged(DbOperation opr, DbModeltype mt)
        {
            if (RepositoryChanged != null) RepositoryChanged(this, new DbEventArgs(opr, mt));
        }

        protected SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }
        // oprindelig kodebase slut
    }
}
