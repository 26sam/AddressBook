
using AddressBook;
using Logger;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AddressBook
{
    internal class AddressBook
    {
        private readonly static List<Contact> _contactList = new List<Contact>();
        private readonly static List<Contact> _deletedContact = new List<Contact>();
        private readonly static int noOfUndoContacts = 4;
        private static int _userId;

        private readonly LogFile _logFile = new LogFile();
        public AddressBook()
        {
            _userId = 0;
        }
        public int UserId
        {
            private get { return _userId; }
            set { _userId = value; }
        }
        public void SetContactList(Contact contact)
        {
            _logFile.EnterLog("Information", $"Contact has been successfully registered.");
            _contactList.Add(contact);
        }
        public int SelectOptionFromMenu()
        {
            Console.Clear();
            DisplayChoices();

            int userInput;
            while (true)
            {
                try
                {
                    Console.Write("\nEnter your choice: ");
                    userInput = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch(Exception ex)
                {
                    _logFile.EnterLog("Exception", $"{ex.StackTrace}");
                    Console.WriteLine("Invalid Input try again.\n Press any key to Continue....");
                }
            }
            return userInput;
        }
        private bool IfWrongSelection()
        {
            Console.Write("\tIf chosen by mistake press 0 to the menu or press any key: ");
            char userInput = Console.ReadKey().KeyChar;

            return (userInput == '0') ? true : false;
        }
        private static void DisplayChoices()
        {
            Console.WriteLine("\t\tPress 0 to return to the main menu.....");
            Console.WriteLine("\nChoose any one option\n ");
            Console.WriteLine("1. Enter new contact");
            Console.WriteLine("2. Edit the existing contact");
            Console.WriteLine("3. Remove the existing contact");
            Console.WriteLine("4. Display a contact");
            Console.WriteLine("5. Undo remove contact");
            Console.WriteLine("6. Quit\n");
        }
        private Contact? GetContact(int userInput)
        {
            int userId, chances = 3;
            Contact? contact = null;
            
            do
            {
                PrintDetails();
                Console.WriteLine($"You have {chances} chances left......");
                Console.Write("Choose User Id: ");
                try
                {
                    userId = Convert.ToInt32(Console.ReadLine());
                    if (userId == 0)
                        return null;
                    contact = _contactList.Find(cont => cont.UserId == userId);
                    if (contact == null)
                    {
                        _logFile.EnterLog("Warning", "No such option is present");
                        Console.WriteLine("No such Id exists. Try Again!!!!");
                        --chances;
                    }
                    else
                        break;
                }
                catch (Exception ex)
                {
                    --chances;
                    if (ex.StackTrace != null)
                        _logFile.EnterLog("Exception", ex.StackTrace);

                    Console.WriteLine(ex.Message + "Try Again!!!");
                }

                Task.Delay(1500).Wait();
                Console.Clear();
                DisplayChoices();

                if (chances == 0)
                {
                    Console.WriteLine("You have used up all your chances......");
                    break;
                }

                Console.WriteLine($"Enter your Choice: {userInput}");
            } while (true);

            return contact;
        }
        private void AddContact()
        {
            Contact person = new Contact();
            Console.WriteLine("\n Enter your Details");
            person.EnterDetails(++_userId);
            _contactList.Add(person);
            _logFile.EnterLog("Information", $"{this.GetType()}\n\t{person.FirstName} {person.LastName} details has been successfully registered.");

            Console.WriteLine($"{person.FirstName} {person.LastName} details has been successfully registered.");
        }
        private void EditContact(int userInput)
        {
            Contact? editContact = GetContact(userInput);

            if (editContact != null)
                editContact.EditDetails();
            else
                _logFile.EnterLog("Information", "Redirecting to the Menu");
        }
        private void DeleteContact(int userInput)
        {
            if (_contactList.Count == 0)
            {
                _logFile.EnterLog("Information", "No Contact is present to delete. Redirecting to the main menu.");
                Console.WriteLine("No Contact is present. Redirecting to the main menu.");
                Task.Delay(2000).Wait();
                return;
            }

            Contact? removeContact = GetContact(userInput);

            if (removeContact != null)
            {
                if (noOfUndoContacts != 0 && _deletedContact.Count == noOfUndoContacts)
                {
                    _deletedContact.RemoveAt(0);
                }
                _deletedContact.Add(removeContact);
                _contactList.Remove(removeContact);
                _logFile.EnterLog("Information", $"{removeContact.FirstName} {removeContact.LastName} Contact has been successfully removed");
                Console.WriteLine($"\t{removeContact.FirstName} {removeContact.LastName} has been successfully removed from the contact.");
            }
            else
                _logFile.EnterLog("Information", "Redirecting to the menu");
        }
        private void DisplayContact(int userInput)
        {
            if (_contactList.Count == 0)
            {
                _logFile.EnterLog("Warning", "Cannot display contact of empty list");
                Console.WriteLine("No Contact is present. Redirecting to the main menu.....");
                Task.Delay(2000).Wait();
                return;
            }
            Contact? displayContact = GetContact(userInput);
            if (displayContact != null)
                displayContact.PrintDetails();
            else
                _logFile.EnterLog("Information", "Redirecting to the menu");
        }
        private void UndoContact()
        {
            Console.Write("\tIf chosen by mistake press 0 to the menu or press any key: ");
            char userInput = Console.ReadKey().KeyChar;

            if (userInput == '0')
                return;
            if (noOfUndoContacts == 0)
            {
                _logFile.EnterLog("Warning", "There are no more contacts to undo.");
                Console.WriteLine("There are no contacts to undo.");
                Task.Delay(1500).Wait();
                return;
            }
            if (!IsUndoContactPossible())
            {
                _logFile.EnterLog("Warning", "There are no more contacts to undo.");
                Console.WriteLine("There are no contacts to undo");
                Task.Delay(1500).Wait();
                return;
            }
        }
        private bool IsUndoContactPossible()
        {
            int noOfContacts = _deletedContact.Count;

            if (noOfContacts == 0)
                return false;

            Contact restoredContact = _deletedContact[noOfContacts-1];
            _deletedContact.RemoveAt(noOfContacts - 1);
            _contactList.Add(restoredContact);
            _logFile.EnterLog("Information", $"{restoredContact.FirstName} {restoredContact.LastName} contact has been restored.");
            Console.WriteLine($"{restoredContact.FirstName} {restoredContact.LastName} contact has been restored.");

            return true;
        }
        private bool Exit()
        {
            Console.Write("\n\tIf you want to quit press Q else press any key: ");

            char quit = Console.ReadKey().KeyChar;

            if (quit == 'q' || quit == 'Q')
            {
                _logFile.EnterLog("Information", $"Application has been closed");
                return true;
            }
            else
                return false;
        }
        private void PrintDetails()
        {
           
            Console.WriteLine("\t{0} {1} {2}\n", ("ID").PadRight(8), ("NAME").PadRight(22), ("EMAIL ID").PadRight(9));
            string result = "", fullname = "";
            foreach (var contact in _contactList)
            {
                fullname = contact.FirstName + " " + contact.LastName;
                result = String.Format("\t{0} {1} {2}", contact.UserId.ToString().PadRight(8), fullname.PadRight(22), contact.EmailAddress.PadRight(9));
                Console.WriteLine(result);
            }
        }
        public void IndexPage()
        {

            //BindData();
            LoadData loadData = new LoadData();
            loadData.LoadDataFromExcel();
            while (true)
            {
                int userInput = SelectOptionFromMenu();
                bool exit = false;
                switch ((IndexOptions)userInput)
                {

                    case IndexOptions.MainMenu:
                        if (IfWrongSelection())
                            break;
                        Console.WriteLine("You are already at main menu...");
                        break;
                        
                    case IndexOptions.NewContact:
                        if (IfWrongSelection())
                            break;
                        AddContact();
                        if (Exit()) 
                            exit = true;
                        break;

                    case IndexOptions.EditContact:
                        if (IfWrongSelection())
                            break;
                        EditContact(userInput);
                        if (Exit()) 
                            exit = true;
                        break;

                    case IndexOptions.RemoveContact:
                        if (IfWrongSelection())
                            break;
                        DeleteContact(userInput);
                        if (Exit()) 
                            exit = true;
                        break;

                    case IndexOptions.DisplayContact:
                        if (IfWrongSelection())
                            break;
                        DisplayContact(userInput);
                        if (Exit()) 
                            exit = true;
                        break;

                    case IndexOptions.UndoContact:
                        if (IfWrongSelection())
                            break;
                        UndoContact();
                        if (Exit()) 
                            exit = true;
                        break;

                    case IndexOptions.Quit:
                        exit = true;
                        break;

                    default:
                        _logFile.EnterLog("Warning", "InValid Input");
                        Console.WriteLine("\tInvalid Input Try Again!!!!!!!");
                        if (Exit())
                            exit = true;
                        break;
                }
                if(exit)
                {
                    _logFile.EnterLog("Information", "Terminating Application");
                    Console.WriteLine("\nTerminating Application");
                    break;
                }
            }
        }
    }
}
