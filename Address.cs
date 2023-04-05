
using Logger;

namespace AddressBook
{
    internal class Address
    {
        private string _houseNumber, _areaName, _state, _city, _country, _zipCode;
        private const int _zipCodeLength = 5;
        private readonly LogFile _logFile = new LogFile();
        public Address() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {

        }
        public Address(string houseNumber, string areaName, string city, string state, string country, string zipCode)
        {
            this._houseNumber = houseNumber;
            this._areaName = areaName;
            this._city = city;
            this._state = state;
            this._country = country;
            this._zipCode = zipCode;
        }
        public void PrintDetails()
        {
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", (_houseNumber==string.Empty)?"____":_houseNumber, _areaName, 
            _city, _state,_country, _zipCode);
        }
        private void StandardName(ref string name)
        {
            name = name.Substring(0,1).ToUpper() + name.Substring(1).ToLower();
        }
        private void EnterHouseNumber()
        {
            Console.Write("\n\tHouse Number: ");
            _houseNumber = Console.ReadLine() ?? string.Empty;
            _houseNumber = _houseNumber.Trim(' ');
        }
        private void EnterAreaName()
        {
            string error = string.Empty;
            do
            {
                Console.Write("\tAreaName: ");
                _areaName = Console.ReadLine() ?? string.Empty;
                _areaName = _areaName.Trim(' ');
                if (Validate.IsValidName(_areaName, out error))
                {
                    StandardName(ref _areaName);
                    break;
                }
                else
                {
                    _logFile.EnterLog("Warning", "Invalid Input");
                    Console.WriteLine(error);
                }

            } while (true);
        }
        private void EnterCity()
        {
            string error = string.Empty;
            do
            {
                Console.Write("\tCity: ");
                _city = Console.ReadLine() ?? string.Empty;
                _city = _city.Trim(' ');
                if (Validate.IsValidName(_city, out error))
                {
                    StandardName(ref _city);
                    break;
                }
                else
                {
                    _logFile.EnterLog("Warning", "Invalid Input");
                    Console.WriteLine(error);
                }

            } while (true);
        }
        private void EnterState()
        {
            string error = string.Empty;
            do
            {
                Console.Write("\tState: ");
                _state = Console.ReadLine() ?? string.Empty;
                _state = _state.Trim(' ');
                if (Validate.IsValidName(_state, out error))
                {
                    StandardName(ref _state);
                    break;
                }
                else
                {
                    _logFile.EnterLog("Warning", "Invalid Input");
                    Console.WriteLine(error);
                }

            } while (true);
        }
        private void EnterCountry()
        {
            string error = string.Empty;
            do
            {
                Console.Write("\tCountry: ");
                _country = Console.ReadLine() ?? string.Empty;
                _country = _country.Trim(' ');
                if (Validate.IsValidName(_country, out error))
                {
                    StandardName(ref _country);
                    break;
                }
                else
                {
                    _logFile.EnterLog("Warning", "Invalid Input");
                    Console.WriteLine(error);
                }
            } while (true);
        }
        private void EnterZipCode()
        {
            string error = string.Empty;
            do
            {
                Console.WriteLine("\n\tEnter 5digit Zip Code(xxxxx)");
                Console.Write("\tZip Code: ");
                _zipCode = Console.ReadLine() ?? string.Empty;
                _zipCode = _zipCode.Trim(' ');
                if (Validate.IsValidNumber(_zipCode, out error, _zipCodeLength))
                    break;
                else
                {
                    _logFile.EnterLog("Warning", "Invalid Input");
                    Console.WriteLine(error);
                }
            } while (true);
        }
        public Address EnterAddress()
        {
            Address address = new Address();

            address.EnterHouseNumber();
            address.EnterAreaName();
            address.EnterCity();
            address.EnterState();
            address.EnterCountry();
            address.EnterZipCode();

            return address;
        }
    }
}
