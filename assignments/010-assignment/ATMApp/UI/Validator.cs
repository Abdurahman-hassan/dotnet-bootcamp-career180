using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public static class Validator
    {
        public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput;
            while (!valid)
            {
                userInput = Utility.GetuserInput(prompt);
                try
                {
                    var convertor = TypeDescriptor.GetConverter(typeof(T));
                    if (convertor != null)
                    {
                        return (T)convertor.ConvertFromString(userInput);
                    }
                    else
                    {
                        return default;
                    }

                }
                catch (Exception ex)
                {
                    Utility.PrintMessage("Invalid input, try again", false);
                }
            }
            return default;
        }
    }
}
