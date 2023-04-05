
using Logger;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace AddressBook
{

    internal class LoadData
    {
        private readonly LogFile _logFile = new LogFile();
        private static int _userId = 0;
        private const string path = @"C:\Users\sajain\Desktop\ContactBook.xlsx";
        private readonly int _firstName, _lastName, _contactNumber, _email, _emergencyNumber, _gender;
        private readonly int _tempHouseNumber, _tempAreaName, _tempCity, _tempState, _tempCountry, _tempZipCode;
        private readonly int _permHouseNumber, _permAreaName, _permCity, _permState, _permCountry, _permZipCode;
        public LoadData()
        {
            _userId = 0;
            _firstName = 1;
            _lastName = 2;
            _contactNumber = 3;
            _email = 4;
            _emergencyNumber = 5;
            _gender = 6;
            _tempHouseNumber = 7;
            _tempAreaName = 8;
            _tempCity = 9;
            _tempState = 10;
            _tempCountry = 11;
            _tempZipCode = 12;
            _permHouseNumber = 13;
            _permAreaName = 14;
            _permCity = 15;
            _permState = 16;
            _permCountry = 17;
            _permZipCode = 18;
        }
        private string ValidateName(List<string> details, int name)
        {
            if (!Validate.IsValidName(details[name], out var error))
            {
                _logFile.EnterLog("Warning", $"{error}");
                return error;
            }
                
            else
                return details[name];
        }
        private string ValidateNumber(List<string> details, int number, int count)
        {
            if (!Validate.IsValidNumber(details[number], out var error, count))
            {
                _logFile.EnterLog("Warning", $"{error}");
                return error;
            }
            else
                return details[number];
        }
        private void ValidateData(List<string> details, ref int id)
        {
            try
            {
                if (details[0] == "" || details[0] == "0")
                    id = _userId;
                else
                    Convert.ToInt32(details[0]);
            }
            catch(Exception ex)
            {
                _logFile.EnterLog("Exception", $"{ex.StackTrace}");
            }

            details[_firstName] = ValidateName(details, _firstName);
            details[_lastName] = ValidateName(details, _lastName);
            details[_contactNumber] = ValidateNumber(details, _contactNumber, 10);
            details[_emergencyNumber] = ValidateNumber(details, _emergencyNumber, 10);

            details[_tempAreaName] = ValidateName(details, _tempAreaName);
            details[_tempCity] = ValidateName(details, _tempCity);
            details[_tempState] = ValidateName(details, _tempState);
            details[_tempCountry] = ValidateName(details, _tempCountry);
            details[_tempZipCode] = ValidateNumber(details, _tempZipCode, 5);

            details[_permAreaName] = ValidateName(details, _permAreaName);
            details[_permCity] = ValidateName(details, _permCity);
            details[_permState] = ValidateName(details, _permState);
            details[_permCountry] = ValidateName(details, _permCountry);
            details[_permZipCode] = ValidateNumber(details, _permZipCode, 5);

            if (!Validate.IsValidEmail(details[_email], out var warning))
            {
                _logFile.EnterLog("Warning", $"{warning}");
                details[_email] = warning;
            }
                

            if (!Validate.IsValidGender(details[_gender], out warning))
            {
                _logFile.EnterLog("Warning", $"{warning}");
                details[_gender] = warning;
            }
                
        }
        private void InsertDataIntoContactList(List<string> details)
        {
            int id = ++_userId;
            ValidateData(details, ref id);
            Address temporaryAddress = new Address(details[_tempHouseNumber], details[_tempAreaName], details[_tempCity], 
                                                    details[_tempState], details[_tempCountry], details[_tempZipCode]);

            Address permanentAddress = new Address(details[_permHouseNumber], details[_permAreaName], details[_permCity],
                                                    details[_permState], details[_permCountry], details[_permZipCode]);

            Contact contact = new Contact(id, details[_firstName], details[_lastName], details[_contactNumber], details[_email],
                                        details[_emergencyNumber], details[_gender], temporaryAddress, permanentAddress);

            AddressBook addressBook = new AddressBook();
            addressBook.SetContactList(contact);
        }
        private void ReadData(Excel.Range excelRange)
        {
            int rowCount = excelRange.Rows.Count;
            int columnCount = excelRange.Columns.Count;

            List<string> details = new List<string>();
            
            for (int i = 3; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    try
                    {
                        if (excelRange.Cells[i, j].Text == "")
                        {
                            details.Add("");
                        }
                        else if (excelRange.Cells[i, j] != null)
                        {
                            var value = excelRange.Cells[i, j].Value();
                            details.Add(value.ToString().Trim(' '));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logFile.EnterLog("Exception", $"{ex.StackTrace}");
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }

                InsertDataIntoContactList(details);
                details.Clear();
            }
        }
        public void LoadDataFromExcel()
        {
            try
            {
                Application excelApp = new Application();
                Workbook excelWB = excelApp.Workbooks.Open(path);
                _Worksheet excelWS = excelWB.Sheets[1];
                Excel.Range excelRange = excelWS.UsedRange;

                ReadData(excelRange);                

                AddressBook addressBook = new AddressBook();
                addressBook.UserId = _userId;

                Marshal.ReleaseComObject(excelWS);
                Marshal.ReleaseComObject(excelRange);
                excelWB.Close();
                Marshal.ReleaseComObject(excelWB);
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);

                _logFile.EnterLog("Information", $"{this.GetType()} DATA HAS BEEN LOADED SUCCEFULLY");

            }
            catch(Exception ex)
            {
                _logFile.EnterLog("Exception", $"{ex.StackTrace}");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
