using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class House
    {
        private int unitNumber;

        public int UnitNumber { get; set; }

        private int streetNumber;

        public int StreetNumber { get; set; }

        private string streetName;

        public string StreetName { get; set; }

        private string streetSuffix;

        public string StreetSuffix { get; set; }

        private string city;

        public string City { get; set; }

        private string state;

        public string State { get; set; }

        private int postCode;

        public int PostCode { get; set; }

        public House(int unitNumberInput, int streetNumberInput, string streetNameInput, string streetSuffixInput, string cityInput, string stateInput, int postCodeInput)
        {
            //house class constructor
            UnitNumber = unitNumberInput;
            StreetNumber = streetNumberInput;
            StreetName = streetNameInput;
            StreetSuffix = streetSuffixInput;
            City = cityInput;
            State = stateInput;
            PostCode = postCodeInput;

        }
    }
}
