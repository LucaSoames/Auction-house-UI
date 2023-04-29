using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Advert
    {
        public string Name { get ; set; }

        private string name;
        public string Description { get; set; }

        private string description;
        public string Price { get; set; }

        private string price;

        public string Bid { get; set; }

        private string bid;    

        public Advert(string nameInput, string descriptionInput, string priceInput, string topBidEmail) //Advert constructor
        {
            Name = nameInput;
            Description = descriptionInput;
            Price = priceInput;
            Bid = topBidEmail; // linking variable between a bid and advert.
        }
    }
}
