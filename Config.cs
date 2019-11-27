using System;
using System.IO;
using Microsoft.Win32;

namespace CopyWorkItemFromTFStoHTMLandAttachWinReg
{
    public abstract class Config
    {
        /// <summary>
        /// Create/Edit/Read the Config File.
        /// </summary>

        // string array for read the config file
        public static string[] config = new string[3];
        // temp variable for read value from Config Form
        public static string tempDomainName = null;
        public static string tempPassword = null;
        public static string tempPathToTasks = null;

        // read the config file to memory
        public static bool readConfigFile()
        {

            //opening the subkey  
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TFStoHTMLandAttach");

            // check if the config file exist and if it more than 0 bytes
            if (key == null)
                return false;

            // chek config if it has less or more than 3 string
            if (key.ValueCount != 3)
                return false;

            // check the string from string array if it's empty or not
            if (key.GetValue("Login") == null || key.GetValue("Pass") == null || key.GetValue("Path") == null)
                return false;

            // check if the saved password can't be decrypted
            if (Cipher.Decrypt(key.GetValue("Pass").ToString(), Program.Key) == "$$$")
                return false;

            config[0] = key.GetValue("Login").ToString();
            config[1] = key.GetValue("Pass").ToString();
            config[2] = key.GetValue("Path").ToString();

            return true;
        }
        // function for create and editing the config file
        public static void editConfigFile()
        {
            //accessing the CurrentUser root element  
            //and adding "OurSettings" subkey to the "SOFTWARE" subkey  
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TFStoHTMLandAttach");

            if (tempDomainName.CompareTo("") == 0)
                key.SetValue("Login", key.GetValue("Login"));
            else
                key.SetValue("Login", tempDomainName.ToLower());

            // encrypt the password for writing into config file
            key.SetValue("Pass", Cipher.Encrypt(tempPassword, Program.Key));

            if (tempPathToTasks.CompareTo("") == 0)
                key.SetValue("Path", key.GetValue("Path"));
            else
                key.SetValue("Path", tempPathToTasks.ToLower());
        }
    }
}
