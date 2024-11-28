using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace ATM;

class Program
{
    static void Main(string[] args)
    {
        Atm atm = new ();
        atm.Run();
    }
}