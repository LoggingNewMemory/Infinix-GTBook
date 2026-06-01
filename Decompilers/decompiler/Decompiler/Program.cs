using System;
using System.IO;
using System.Linq;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;

class Program
{
    static void Main(string[] args)
    {
        string inputDir = args.Length > 0 ? args[0] : "../extracted_bundle";
        string outputDir = args.Length > 1 ? args[1] : "../decompiled_source";

        // Key application assemblies to decompile
        string[] keyAssemblies = new[]
        {
            "ControlCenter.dll",
            "BydCentral.Core.dll",
            "BYDWmi2.dll",
            "CustomControlLibrary.dll",
        };

        Directory.CreateDirectory(outputDir);

        foreach (var assemblyName in keyAssemblies)
        {
            string assemblyPath = Path.Combine(inputDir, assemblyName);
            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"WARNING: {assemblyName} not found at {assemblyPath}");
                continue;
            }

            Console.WriteLine($"\n{"="u8.Length}");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine($"Decompiling: {assemblyName}");
            Console.WriteLine(new string('=', 80));

            try
            {
                var settings = new DecompilerSettings(LanguageVersion.CSharp10_0)
                {
                    ThrowOnAssemblyResolveErrors = false,
                    RemoveDeadCode = false,
                    RemoveDeadStores = false,
                };

                var decompiler = new CSharpDecompiler(assemblyPath, settings);

                // Create output directory for this assembly
                string assemblyOutputDir = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(assemblyName));
                Directory.CreateDirectory(assemblyOutputDir);

                // 1. Decompile entire assembly to a single file
                string fullSource = decompiler.DecompileWholeModuleAsString();
                string fullOutputPath = Path.Combine(assemblyOutputDir, $"{Path.GetFileNameWithoutExtension(assemblyName)}_full.cs");
                File.WriteAllText(fullOutputPath, fullSource);
                Console.WriteLine($"  Full source: {fullOutputPath} ({new FileInfo(fullOutputPath).Length / 1024}KB)");

                // 2. Decompile each type to a separate file
                var types = decompiler.TypeSystem.MainModule.TypeDefinitions
                    .Where(t => !t.Name.StartsWith("<") && t.Name != "<Module>")
                    .ToList();

                Console.WriteLine($"  Found {types.Count} types to decompile individually");

                foreach (var type in types)
                {
                    try
                    {
                        string typeSource = decompiler.DecompileTypeAsString(type.FullTypeName);
                        
                        // Create namespace directory structure
                        string nsDir = string.IsNullOrEmpty(type.Namespace) ? "_global" : type.Namespace.Replace('.', Path.DirectorySeparatorChar);
                        string typeDir = Path.Combine(assemblyOutputDir, nsDir);
                        Directory.CreateDirectory(typeDir);

                        string safeTypeName = type.Name;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeTypeName = safeTypeName.Replace(c, '_');

                        string typeOutputPath = Path.Combine(typeDir, $"{safeTypeName}.cs");
                        File.WriteAllText(typeOutputPath, typeSource);
                        
                        // Count methods
                        int methodCount = type.Methods.Count();
                        int propertyCount = type.Properties.Count();
                        int fieldCount = type.Fields.Count();
                        
                        Console.WriteLine($"    [{type.Kind}] {type.FullName} - {methodCount} methods, {propertyCount} props, {fieldCount} fields");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    ERROR decompiling {type.FullName}: {ex.Message}");
                    }
                }

                // 3. Generate a summary/index file
                string indexPath = Path.Combine(assemblyOutputDir, "INDEX.md");
                using (var writer = new StreamWriter(indexPath))
                {
                    writer.WriteLine($"# {assemblyName} - Decompiled Source Index");
                    writer.WriteLine();
                    writer.WriteLine($"- **Assembly**: {assemblyName}");
                    writer.WriteLine($"- **Types**: {types.Count}");
                    writer.WriteLine($"- **Total Methods**: {types.Sum(t => t.Methods.Count())}");
                    writer.WriteLine();

                    var namespaces = types.GroupBy(t => string.IsNullOrEmpty(t.Namespace) ? "<global>" : t.Namespace);
                    foreach (var ns in namespaces.OrderBy(g => g.Key))
                    {
                        writer.WriteLine($"## Namespace: `{ns.Key}`");
                        writer.WriteLine();
                        foreach (var type in ns.OrderBy(t => t.Name))
                        {
                            writer.WriteLine($"- **{type.Kind}** `{type.Name}` - {type.Methods.Count()} methods, {type.Properties.Count()} properties");
                            foreach (var method in type.Methods.Where(m => !m.Name.StartsWith("<") && m.Name != ".ctor" && m.Name != ".cctor"))
                            {
                                writer.WriteLine($"  - `{method.Name}({string.Join(", ", method.Parameters.Select(p => $"{p.Type.Name} {p.Name}"))})`");
                            }
                        }
                        writer.WriteLine();
                    }
                }
                Console.WriteLine($"  Index: {indexPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  FATAL ERROR: {ex.Message}");
                Console.WriteLine($"  {ex.StackTrace}");
            }
        }

        Console.WriteLine("\n" + new string('=', 80));
        Console.WriteLine("Decompilation complete!");
        Console.WriteLine($"Output directory: {Path.GetFullPath(outputDir)}");
    }
}
