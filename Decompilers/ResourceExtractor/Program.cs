using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections;

class Program
{
    static void Main(string[] args)
    {
        string dllPath = "/home/yamada/CODE/device_infinix_gtbook/extracted_bundle/ControlCenter.dll";
        string outDir = "/home/yamada/CODE/device_infinix_gtbook/NewControlCenter/assets/images";
        Directory.CreateDirectory(outDir);

        Assembly asm = Assembly.LoadFile(dllPath);
        foreach (string name in asm.GetManifestResourceNames())
        {
            Console.WriteLine("Manifest Resource: " + name);
            if (name.EndsWith(".resources"))
            {
                using (Stream stream = asm.GetManifestResourceStream(name))
                using (ResourceReader reader = new ResourceReader(stream))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        string key = (string)entry.Key;
                        Console.WriteLine("  Entry: " + key);
                        if (key.EndsWith(".png") || key.EndsWith(".jpg") || key.EndsWith(".ico"))
                        {
                            object value = entry.Value;
                            if (value is Stream s)
                            {
                                string outPath = Path.Combine(outDir, key.Replace("/", "_").Replace("\\", "_"));
                                using (FileStream fs = new FileStream(outPath, FileMode.Create))
                                {
                                    s.CopyTo(fs);
                                }
                                Console.WriteLine("    -> Extracted: " + outPath);
                            }
                        }
                    }
                }
            }
        }
    }
}
