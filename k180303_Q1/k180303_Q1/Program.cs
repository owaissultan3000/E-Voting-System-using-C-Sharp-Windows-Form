using System;
using System.Configuration;
using System.IO;


namespace k180303_Q1
{
    class PasswordEncryptor
    {
        protected String userName;
        protected String password;
        protected String stationID;

        public PasswordEncryptor(String _userName,String _password,String _stationID)
        {
            userName = _userName;
            password = _password;
            stationID = _stationID;
        }

        protected void StoreData()
        {
            //File Location
            String path = ConfigurationManager.AppSettings["FilePath"];
            path += @"\cList.data";


            var fileData = userName + ',' + password + ',' + stationID + Environment.NewLine;

            try
            {
                if (File.Exists(path))
                {
                    File.AppendAllText(path, fileData);
                }
                else
                {
                    System.IO.File.WriteAllText(path, fileData);
                }

                //store output in file 
                string readText = File.ReadAllText(path);
                Console.WriteLine(readText);
                return;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e+" File Not Found!!");
                return;
            }
        }

        static void Main(string[] args)
        {
            
            if (args.Length == 3)
            {
                string root = @"C:\k180303OutputFiles\Q1";

                // If directory does not exist, create it. 
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                //store command line argument in variables
                String userName = args[0];
                String password = args[1];
                String stationID = args[2];

                //encryption of password in base64
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
                password = System.Convert.ToBase64String(plainTextBytes);

                PasswordEncryptor passwordEncryptor = new PasswordEncryptor(userName, password, stationID);
                passwordEncryptor.StoreData();
             }
            else
            {
                Console.WriteLine("Command Line Argument Is Missing !!");
                return;
            }
                     
        }
    }
}
