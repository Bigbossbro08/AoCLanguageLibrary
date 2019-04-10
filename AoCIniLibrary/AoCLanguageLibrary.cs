using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using Vestris.ResourceLib;

namespace AoCLanguageLibrary
{
    public abstract class AoCLanguageLibrary
    {
        public Dictionary<string, string> strings = new Dictionary<string, string>();                                                   // I actually wanted to use <int, string> pair. HD edition has some keys doesn't use like that. For that reason using <string, string> pair. You can parse into int if you want.  Exclude HD string keys as well or setting HD keys to some other format.
    }

    public class AocTxtLibrary : AoCLanguageLibrary
    {
        public AocTxtLibrary()
        {

        }

        public AocTxtLibrary(string fileName)
        {
            Load(fileName);
        }

        public void Load(string fileName)
        {
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = string.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    var d = s.Split(null, 2);
                    if (s.Length == 0) continue;
                    if (s.StartsWith("\\")) continue;
                    if (s.StartsWith("/")) continue;
                    
                    d[1] = d[1].TrimStart(new char[] { '"' });
                    d[1] = d[1].TrimEnd(new char[] { '"' });
                    string key = d[0];
                    if (key.Length == 0)
                    {
                        continue;
                    }
                    string value = d[1];
                    if (strings.ContainsKey(key))
                    {
                        
                        Console.WriteLine($"Same key id is {key} string is: {value}");
                        strings.Remove(key);
                    }
                    strings.Add(key, value);
                }
            }

            strings.OrderBy(x => x.Key);
        }

        public void Save(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(File.Create(fileName)))
            {
                string s = string.Empty;
                foreach (KeyValuePair<string, string> entry in strings)
                {
                    sw.Write($"{entry.Key} ");
                    sw.Write("\"");
                    sw.Write($"{entry.Value}");
                    sw.Write("\"");
                    sw.WriteLine();
                }
            }
        }
    }

    public class AocDllLibrary : AoCLanguageLibrary
    {
        public AocDllLibrary()
        {

        }

        public AocDllLibrary(string dll)
        {
            Load(dll);
        }

        public void Load(string dll)
        {
            var ri = new ResourceInfo();
            ri.Load(dll);

            var languages = ri[Kernel32.ResourceTypes.RT_STRING].GroupBy(r => r.Language);

            foreach (var language in languages)
            {
                var fileName = Path.GetFileNameWithoutExtension(dll);
                var ci = language.Key == 0 ? System.Globalization.CultureInfo.InvariantCulture : System.Globalization.CultureInfo.GetCultureInfo(language.Key);
                var culture = ci.Name;
                var extension = Path.GetExtension(dll);

                foreach (StringResource resource in language)
                {
                    foreach (var str in resource.Strings)
                    {
                        string key = Convert.ToString(str.Key);
                        string value = str.Value;
                        value = value.Replace("\n", @"\n");
                        value = value.Replace("\r", @"\r");
                        if (strings.ContainsKey(key))
                        {
                            Console.WriteLine($"Same key id is {key}");
                            strings.Remove(key);
                        }
                        strings.Add(key, value);
                    }
                }
            }

            strings.OrderBy(x => x.Key);
        }

        public void Save(string path)
        {
            using (var ri = new ResourceInfo())
            {
                var resources = new List<Resource>();

                foreach (KeyValuePair<string, string> entry in strings)
                {
                    var sr = new StringResource(ushort.Parse(entry.Key));
                    if (int.TryParse(entry.Key, out int key))
                    {
                        sr[(ushort)key] = entry.Value;
                    }
                    else
                    {
                        Console.WriteLine("String key detected may not be supported in dll");
                    }
                    resources.Add(sr);
                }
                
                if (!path.EndsWith(".dll"))
                {
                    path += ".dll";
                }
                Resource.Save(path, resources);
            }
        }
    }

    public class AoCIniLibrary : AoCLanguageLibrary
    {
        public AoCIniLibrary()
        {

        }

        public AoCIniLibrary(string fileName)
        {
            Load(fileName);
        }

        public void Save(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(File.Create(fileName)))
            {
                string s = string.Empty;
                foreach (KeyValuePair<string, string> entry in strings)
                {
                    if (!int.TryParse(entry.Key, out int key))
                    {
                        Console.WriteLine("Warning string key detected may not be supported by the game");
                    }
                    sw.WriteLine($"{entry.Key}={entry.Value}");
                }
            }
        }

        public void Save(string fileName, bool skipStringKeys)
        {
            using (StreamWriter sw = new StreamWriter(File.Create(fileName)))
            {
                string s = string.Empty;
                foreach (KeyValuePair<string, string> entry in strings)
                {
                    if (skipStringKeys == true)
                    {
                        if (int.TryParse(entry.Key, out int key))
                        {
                            sw.WriteLine($"{entry.Key}={entry.Value}");
                        }
                        else
                        {
                            Console.WriteLine($"Skipping the key {entry.Key}");
                        }
                    }
                    else
                    {
                        if (int.TryParse(entry.Key, out int key))
                        {
                            Console.WriteLine("Warning string key detected may not be supported by the game");
                        }
                        sw.WriteLine($"{entry.Key}={entry.Value}");
                    }
                }
            }
        }

        public void Load(string fileName)
        {
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = string.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    //s = s.Trim();
                    var d = s.Split('=');
                    if (s.Length == 0) continue;
                    string key = d[0];
                    string value = d[1];
                    if (strings.ContainsKey(key))
                    {
                        Console.WriteLine($"Same key id is {key}");
                        strings.Remove(key);
                    }
                    strings.Add(key, value);
                }
            }
            strings.OrderBy(x => x.Key);
        }
    }
}
