using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCLanguageLibrary;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = (string)Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft Games\Age of Empires II: The Conquerors Expansion\1.0").GetValue("EXE Path");
            var iniPath = Path.Combine(path, @"games\wololokingdoms\language.ini");
            if (File.Exists(iniPath))
            {
                Console.WriteLine("File exists. Starting operation.");
                var ini = new AoCIniLibrary(path);
                foreach (KeyValuePair<string, string> entry in ini.strings)
                {
                    Console.WriteLine($"Key is: {entry.Key} data is : {entry.Value}");
                }
                ini.Save(@"E:\User\Visual C++ Projects\AoCIniLibrary\mystring.ini");
            }

            //var dllPath = Path.Combine(path, @"language_x1_p1.dll");
            //if (File.Exists(dllPath))
            //{
            //    Console.WriteLine("File exists. Starting operation.");
            //    var dll = new AocDllLibrary(dllPath);
            //    foreach (KeyValuePair<string, string> entry in dll.strings)
            //    {
            //        Console.WriteLine($"Key is: {entry.Key} data is : {entry.Value}");
            //    }
            //}

            //var txtPath = @"G:\SteamLibrary\steamapps\common\Age2HD\resources\en\strings\key-value\key-value-strings-utf8.txt";         // That's my steam directory file location. You can set one yourself too. Just didn't want to do the hassle of finding Steam directory location from registry and other stuffs.
            //if (File.Exists(txtPath))
            //{
            //    Console.WriteLine("File exists. Starting operation.");
            //    var dll = new AocTxtLibrary(txtPath);
            //    foreach (KeyValuePair<string, string> entry in dll.strings)
            //    {
            //        Console.WriteLine($"Key is: {entry.Key} data is : {entry.Value}");
            //        if (entry.Key == entry.Value)
            //            Console.Write("Empty value? ");
            //    }

            //    dll.Save(@"E:\User\Visual C++ Projects\AoCIniLibrary\mystring.txt");
            //}
            Console.ReadKey();
        }
    }
}
