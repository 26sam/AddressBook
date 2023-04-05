
using Logger;

namespace AddressBook
{
    internal class Program
    {
        static void Main(string[] args)
        {

            LogFile logFile = new LogFile();
            logFile.EnterLog("Information", "Application has Started");

            AddressBook addressBook = new AddressBook();
            addressBook.IndexPage();
            
        }
    }
}