using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsDB.Domain.Models
{
    // Modelklasse for en adresse.
    // Contact implementerer datavalidering, så en adresse skal opfylde:
    //   Phone      skal være en streng bestående af 8 cifre
    //   Firstname  må ikke være null
    //   Lastname   må ikke være blank
    //   Address    må ikke være null
    //   Zipcode    skal være en streng bestående af 4 cifre
    //   Email      skal være blank eller også en streng, som kan være en lovlig email adresse
    //   Title      må ikke være null
    // Telefonnummer opfattes som nøgle, og sammenligning og ordning sker alene ud fra værdien af Phone.
    public class Contact : IDataErrorInfo, IComparable<Contact>
    {
    [Key]
    public string Phone { get; set; }         // telefonnummer
    public string Firstname { get; set; }     // fornavn
    public string Lastname { get; set; }      // efternavn
    public string Address { get; set; }       // adresse
    [ForeignKey("Zipcode")]
    public string Zipcode { get; set; }       // postnummer
    // public string City { get; set; }          // bynavn
    public string Email { get; set; }         // email adresse
    public string Title { get; set; }         // stillingsbetegnelse

    // Opretter et objekt ved at sætte alle felter til blank.
    public Contact()
    {
      Phone = "";
      Firstname = "";
      Lastname = "";
      Address = "";
      Zipcode = "";
      // City = "";
      Email = "";
      Title = "";
    }

    // Opretter et objekt, hvor alle felter initialiseres med parametre.
    // Konstruktøren garanterer ikke, at objektet er lovligt.
    public Contact(string phone, string firstname, string lastname, string address, string zipcode, string city, string email, string title)
    {
      Phone = phone;
      Firstname = firstname;
      Lastname = lastname;
      Address = address;
      Zipcode = zipcode;
      // City = city;
      Email = email;
      Title = title;
    }

    // Implementerer sammenligning alene på telefonnummer.
    public override bool Equals(object obj)
    {
      try
      {
        Contact contact = (Contact)obj;
        return Phone.Equals(contact.Phone);
      }
      catch
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return Phone.GetHashCode();
    }

    // Implementerer ordning af adresse alene ud fra telefonnummeret
    public int CompareTo(Contact contact)
    {
      return Phone.CompareTo(contact.Phone);
    }

    // Validering af objektet.
    // Arrayet angiver hvilke properties, der skal valideres.
    private static readonly string[] validatedProperties = { "Phone", "Firstname", "Lastname", "Address", "Zipcode", "Email", "Title" };

    public bool IsValid
    {
      get
      {
        foreach (string property in validatedProperties) if (GetError(property) != null) return false;
        return true;
      }
    }

    string IDataErrorInfo.Error
    {
      get { return IsValid ? null : "Illegal Contact object"; }
    }

    string IDataErrorInfo.this[string propertyName]
    {
      get { return Validate(propertyName); }
    }

    private string GetError(string name)
    {
      foreach (string property in validatedProperties) if (property.Equals(name)) return Validate(name);
      return null;
    }

    private string Validate(string name)
    {
      switch (name)
      {
        case "Phone": return ValidatePhone();
        case "Firstname": return ValidateFirstname();
        case "Lastname": return ValidateLastname();
        case "Address": return ValidateAddress();
        case "Zipcode": return ValidateZipcode();
        case "Email": return ValidateEmail();
        case "Title": return ValidateTitle();
      }
      return null;
    }

    // Valideringsmetoder til de enkelte properties.
    private string ValidatePhone()
    {
      if (Phone.Length != 8) return "Phone must be a number of 8 digits";
      foreach (char c in Phone) if (c < '0' || c > '9') return "Phone must be a number of 8 digits";
      return null;
    }

    private string ValidateFirstname()
    {
      if (Firstname == null) return "Firstname can not be null";
      return null;
    }

    private string ValidateLastname()
    {
      if (Lastname == null || Lastname.Length == 0) return "Lastname can not be empty";
      return null;
    }

    private string ValidateAddress()
    {
      if (Address == null) return "Address can not be null";
      return null;
    }

    private string ValidateZipcode()
    {
      if (Zipcode.Length != 4) return "Zipcode must be a number of 4 digits";
      foreach (char c in Zipcode) if (c < '0' || c > '9') return "Zipcode must be a number of 4 digits";
      return null;
    }

    private string ValidateEmail()
    {
      if (Email == null) return "Email can not be null";
      if (Email.Length == 0) return null;
      return IsValidEmail(Email) ? null : "Email must be legal";
    }

    private static bool IsValidEmail(string email)
    {
      Regex rx = new Regex(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
      return rx.IsMatch(email);
    }

    private string ValidateTitle()
    {
      if (Title == null) return "Title can not be null";
      return null;
    }
  }
}
