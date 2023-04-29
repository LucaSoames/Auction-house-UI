using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Assignment2
{

    class Testing
    {
        // ----- testing code ----------------------------------------------//
        const string fileName = "AuctionHouseDatabase.txt";

        //Name testing
        public static bool IsTextString(string userInput) //check if null or empty
        {
            if (userInput == null)
            {
                return false;
            }
            else if (userInput.Length > 0)
            {
                return true;
                
            }
            else
            {
                return false;
            }
        }

        public static bool IsInCsv(string foo) //check if email address is already in txt file
        {
            foreach (string line in File.ReadLines(Path.GetFullPath(fileName)))
            {
                if (line.Contains(foo))
                {
                    return true;
                    break;
                }
            }
            return false;
        }

        public static bool IsEmailValid(string email)
        {

            if (email.Contains('@')) //check contains @
            {
                string[] components = email.Split('@'); //split and check if there is a both a prefix and suffix
                if (components.Length > 2)
                {
                    return false;
                }
                string prefixCh = "_-."; //possible special chars
                char[] prefixChArray = prefixCh.ToCharArray();
                if (components[0] == "")
                {
                    return false;
                } 
                else if (Regex.IsMatch(components[0], @"^[a-zA-Z0-9_.-]+$")) //regex for letters numbers and allowed special chars
                {
                    if (Regex.IsMatch(components[1], @"^[a-zA-Z0-9.-]+$")) {
                        if (components[1].Contains('.')) //check for 1 dot
                        {
                            char[] componentsCh = components[1].ToCharArray();
                            if (componentsCh[0] == '.') // . cannot be first or last char
                            {
                                return false;
                            }
                            else if (componentsCh[componentsCh.Length-1] == '.')
                            {
                                return false;
                            }
                            else
                            {
                                string[] suffix = components[1].Split('.');
                                if (Regex.IsMatch(suffix[suffix.Length-1], @"^[a-zA-Z]+$")) //check if last part of suffix is only letters
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;  
                                }
                            }
                        }
                        else return false;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }


        }

        public static bool IsPasswordValid(string password)
        {
            if (password.Length < 8) // length must be atleast 8 chars
                return false;
            else if (!password.Any(char.IsUpper)) // must contain 1 upper and lower case
                return false;
            else if (!password.Any(char.IsLower))
                return false;
            else if (password.Contains(" ")) //cannot contain space
                return false;
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\""; //must contain one special char
            char[] specialChArray = specialCh.ToCharArray();
            foreach (char ch in specialChArray)
            {
                if (password.Contains(ch))
                    return true;
            }
            return false;
        }


        public static bool IsCurrency(string userInput)
        {
            if (userInput.ToCharArray()[0] == '$') //check if $ precedes 
            {
                string[] userInputs;
                userInput = userInput.Remove(0,1);
                if (userInput.Contains('.')) //must contain .
                {
                    userInputs = userInput.Split('.');
                    int i;
                    int i2;
                    if (int.TryParse(userInputs[0], out i)) //try parsing
                    {
                        if (int.TryParse(userInputs[1], out i2) & userInputs[1].Length == 2) //must contain two ints after .
                        {
                            return true;
                        }
                        else return false;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }

            }
            else return false;
        }

        public static bool Is1HourTimeSpan(DateTime Start, DateTime End) //check 1 hour time span based on two datetime values
        {
            TimeSpan interval = new TimeSpan(1, 0, 0);
            TimeSpan ts = End - Start;

            if (ts.TotalSeconds < interval.TotalSeconds)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsFirstSignIn(Client client)
        {
            if (client.House.PostCode == 0) //postcode default value is 0 
            {
                return true;
            }
            else return false;
        }

        public static string DeliveryType(Bid bid) // check what type of delivery method it is based on delivery info 
        {
            string s = "-"; //default collectinfo
            if (bid.CollectInfo[0] == s)
            {
                string returnString = $"Deliver to {bid.House.UnitNumber}/{bid.House.StreetNumber} {bid.House.StreetName} {bid.House.City} {bid.House.State} {bid.House.PostCode}";
                return returnString;
            }
            else
            {
                string returnString = $"Collect from {bid.CollectInfo[0]} to {bid.CollectInfo[1]}";
                return returnString;
            }
        }

    }
}
