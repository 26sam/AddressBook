
using Logger;

namespace AddressBook
{
    internal class Contact
    {
        private string? _firstName, _number, _email, _lastName, _gender, _emergencyNumber;
        private const int _contactNumberLength = 10;
        private readonly LogFile _logFile = new LogFile();
        internal string FirstName { get; private set; }
        internal string LastName { get; private set; }
        internal string EmailAddress { get; private set; }
        internal int UserId { get; set; }
        internal Address? TemporaryAddress { get; private set; }
        internal Address? PermanentAddress { get; private set; }
        public Contact() : this(0,string.Empty, string.Empty, string.Empty, 
            string.Empty, string.Empty, string.Empty, null, null)
        {
            
        }

        public Contact(int userId,string firstName, string lastName, string number, string email, string emergencyNumber, 
            string gender, Address? temporaryAddress, Address? permanentAddress)
        {
            this.UserId = userId;
            this._firstName = firstName;
            this._lastName = lastName;
            this._number = number;
            this._email = email;
            this._emergencyNumber = emergencyNumber;
            this._gender = gender;
            this.EmailAddress = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.TemporaryAddress = temporaryAddress;
            this.PermanentAddress = permanentAddress;
        }

        private void StandardName(ref string name)
        {
            name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
        }
        private void EnterFirstName()
        {
            string error = string.Empty;

            do
            {
                Console.Write("\tFirst Name: ");
                _firstName = Console.ReadLine() ?? string.Empty;
                _firstName = _firstName.Trim(' ');

                if (Validate.IsValidName(_firstName, out error))
                {
                    StandardName(ref _firstName);
                    FirstName = _firstName;
                    break;
                }
                else
                {
                    _logFile.EnterLog("Warning", $"{error}");
                    Console.WriteLine(error);
                }
            } while (true);
        }
        private void EnterLastName()
        {
            string error = string.Empty;

            do
            {
                Console.Write("\tLastName: ");
                _lastName = Console.ReadLine() ?? string.Empty;
                _lastName = _lastName.Trim(' ');

                if (Validate.IsValidName(_lastName, out error))
                {
                    StandardName(ref _lastName);
                    this.LastName = _lastName;
                    break;
                }
                else
                {
                    _logFile.EnterLog("Warning", $"{error}");
                    Console.WriteLine(error);
                }
                    

            } while (true);
        }
        private void EnterMobileNumber()
        {
            string error = string.Empty;
            do
            {
                Console.WriteLine("\n\tEnter 10 digit number (+91)(xxxxxxxxxx)");
                Console.Write("\tMobile Number: ");
                _number = Console.ReadLine() ?? string.Empty;
                _number = _number.Trim(' ');

                if (Validate.IsValidNumber(_number, out error, _contactNumberLength))
                    break;
                else
                {
                    _logFile.EnterLog("Warning", $"{error}");
                    Console.WriteLine(error);
                }

            } while (true);
        }
        private void EnterEmergencyNumber()
        {
            string error = string.Empty;
            do
            {
                Console.WriteLine("\n\tEnter 10 digit number (+91)(xxxxxxxxxx)");
                Console.Write("\tEmergency Number: ");
                _emergencyNumber = Console.ReadLine() ?? string.Empty;
                _emergencyNumber = _emergencyNumber.Trim(' ');

                if (Validate.IsValidNumber(_emergencyNumber, out error, _contactNumberLength))
                    break;
                else
                {
                    _logFile.EnterLog("Warning", $"{error}");
                    Console.WriteLine(error);
                }

            } while (true);
        } 
        private void EnterGender()
        {
            string error = string.Empty;
            do
            {
                Console.Write("\n\tGender(M/F)): ");
                _gender = Console.ReadLine() ?? string.Empty;
                _gender = _gender.ToUpper();
                if (Validate.IsValidGender(_gender, out error))
                    break;
                else
                {
                    _logFile.EnterLog("Warning", $"{error}");
                    Console.WriteLine(error);
                }
            } while (true);
        }
        private void EnterEmailAddress()
        {
            string error = string.Empty;
            do
            {
                Console.Write("\tEmail Address: ");
                _email = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(_email))
                {
                    error = "\tInvalid email";
                    _logFile.EnterLog("Warning", $"{error}");
                    Console.WriteLine(error);
                    continue;
                }
                StandardName(ref _email);
                if (Validate.IsValidEmail(_email, out error))
                {
                    EmailAddress = _email;
                    break;
                }
                else
                {
                    _logFile.EnterLog("Warning", $"{error}");
                    Console.WriteLine(error);
                }

            } while (true);
            
        }
        private void EnterTemporaryAddress()
        {
            Address temporaryAddress = new Address();
            do
            {
                Console.Write("\tIf you have temporary Adress? If yes press (y) and if not press (n): ");
                var userInput = Console.ReadKey().KeyChar;
                Console.WriteLine("\n");
                if (userInput == 'Y' || userInput == 'y')
                {
                    Console.WriteLine("\tEnter Temporary Address: ");
                    TemporaryAddress = temporaryAddress.EnterAddress();
                    break;
                }
                else if (userInput == 'N' || userInput == 'n')
                {
                    return;
                }
                else
                {
                    _logFile.EnterLog("Warning", "Invalid Input");
                    Console.WriteLine("\tWrong Input. Try Again\n");
                }

            } while(true);
        }
        private void EnterPermanentAddress()
        {
            Address permanentAddress = new Address();
            Console.Write("\tEnter Permanent Address: ");
            PermanentAddress = permanentAddress.EnterAddress();
        }
        private void DisplayOptions()
        {
            Console.WriteLine("What do you want to edit ?\n");
            Console.WriteLine("1. FirstName");
            Console.WriteLine("2. LastName");
            Console.WriteLine("3. Mobile Number");
            Console.WriteLine("4. Emergency Number");
            Console.WriteLine("5. Email Address");
            Console.WriteLine("6. Gender");
            Console.WriteLine("7. Temporary Address");
            Console.WriteLine("8. Permanenet Address");
        }
        public void EnterDetails(int userId)
        {
            UserId = userId;
            EnterFirstName();
            EnterLastName();
            EnterMobileNumber();
            EnterEmergencyNumber();
            EnterEmailAddress();
            EnterGender();
            EnterTemporaryAddress();
            EnterPermanentAddress();
            _logFile.EnterLog("Information", $"In {this.GetType()}\ncontact's details have been successfully registered");
        }
        public void PrintDetails()
        {
            Console.WriteLine("\tName: {0} {1}", _firstName, _lastName);
            Console.WriteLine("\tContact Number: {0}", _number);
            Console.WriteLine("\tEmergency Number: {0}", _emergencyNumber);
            Console.WriteLine("\tEmail: {0}", _email);
            Console.WriteLine("\tGender: {0}", (_gender == "M")?"Male":"Female");

            if (TemporaryAddress != null)
            {
                Console.Write("\tTemporary Address: ");
                TemporaryAddress.PrintDetails();
            }
            if(PermanentAddress == null) 
            {
                Console.WriteLine("\tNo Permanent Address present.\n");
            }
            else
            {
                Console.Write("\tPermanent Address: ");
                PermanentAddress.PrintDetails();
            }
            _logFile.EnterLog("Information", $"{this.GetType()}All the details of the contact have been successfully printed.");
        }
        public void EditDetails()
        {
            do
            {
                int userInput;
                do
                {
                    DisplayOptions();
                    Console.Write("Enter your Choice: ");
                    try
                    {
                        userInput = Convert.ToInt32(Console.ReadLine());
                        if (userInput == 0)
                            return;
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logFile.EnterLog("Exception", $"{ex.StackTrace}");
                        Console.WriteLine(ex.Message + "Try Again!!!");
                    }
                } while (true);

                switch ((Properties)userInput)
                {
                    case Properties.Firstname:
                        this.EnterFirstName();
                        break;

                    case Properties.Lastname:
                        this.EnterLastName();
                        break;

                    case Properties.Gender:
                        this.EnterGender();
                        break;

                    case Properties.MobileNumber:
                        this.EnterMobileNumber();
                        break;

                    case Properties.EmailAddress:
                        this.EnterEmailAddress();
                        break;

                    case Properties.EmergencyNumber:
                        this.EnterEmergencyNumber();
                        break;

                    case Properties.TemporaryAddress:
                        this.EnterTemporaryAddress();
                        break;

                    case Properties.PermanentAddress:
                        this.EnterPermanentAddress();
                        break;

                    default:
                        _logFile.EnterLog("Warning", "Invalid Input.");
                        Console.WriteLine("\n\tInvalid Input, try again!!");
                        break;
                }

                Console.WriteLine("To Change something else press Y/y otherwise press any key: ");
                var input = Console.ReadLine() ?? string.Empty;
                if (input.ToLower() != "y")
                    break;
            } while (true);

            _logFile.EnterLog("Information", $"{this.GetType()}Contact has been successfully edited");
            Console.WriteLine("Contact has been successfully edited");
        }
    }
}
