using ContactsDB.Domain.Models;
using ContactsDB.Infrastructure.Repository;

namespace Contacts_DB_WPF_UI.ViewModels
{
    public class ZipContactVM
    {
        public Zipcode Zipcode { get; set; }

        public Contact Contact { get; set; }

        public ZipContactVM()
        {

        }

        public ZipContactVM(Contact contact, Zipcode zipcode)
        {
            this.Contact = contact;
            this.Zipcode = zipcode;
        }
    }
}
