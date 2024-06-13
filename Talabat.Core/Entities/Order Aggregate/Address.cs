using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Address //it's not table but will be mapping with order entity it will be order address not the user address
    {
        public Address() //EF should see empty parameterless ctor when adding migrations why?  // cuz EF will use this ctor to create the table

        {
            
        }

        public Address(string firstName , string lastName , string street , string city , string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; } 

        public string City { get; set; } 
        public string Country { get; set; } 



    }
}
