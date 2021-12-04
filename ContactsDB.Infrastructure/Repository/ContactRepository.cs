using ContactsDB.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ContactsDB.Infrastructure.Repository
{
    public class ContactRepository : GenericRepository<Contact>, IEnumerable<Contact>
    {
        public ContactRepository(ApplicationContext context) : base(context)
        {
            // Empty CTOR
        }
        
        private List<Contact> list = new List<Contact>();

        public IEnumerator<Contact> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Contact contact)
        {
            // Add new entity to DB by calling INSERT on base (generic repo) class            
            base.Insert(contact);
            base.SaveChanges();
        }
                
        
        public void SaveUpdatedContact() 
        {
            base.SaveChanges();
        }

        
        public void Remove(Contact contact)
        {          
            base.Delete(contact);
            base.SaveChanges();
        }
    }

}
    

