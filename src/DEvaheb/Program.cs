using System.Diagnostics;

namespace ConsoleApp
{
    // TO TRY:
    //   - if's $ signs for literals, are they necessary? does ibize compile without, and does the runtime work if so?

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("************** THIS IS AN ALPHA VERSION **************");
                Console.WriteLine("***                                                ***");
                Console.WriteLine("***  contact interface@codecrib.com for questions  ***");
                Console.WriteLine("***                                                ***");
                Console.WriteLine("******************************************************");
                Console.WriteLine();
                Console.WriteLine("DEvaheb v2.0 *ALPHA*");
                Console.WriteLine();

                Console.WriteLine("DEvaheb.exe \"path_to_compiled_icarus\" [-output \"path_for_new_source_file\"] [-open \"path_to_editor_to_open\"]");
                Console.WriteLine();
                Console.WriteLine("if output file path is ommitted, IBI file name and path are reused, but with extension .icarus");
                Console.WriteLine("if option third parameter is specified, the generated source file is opened");

                Console.WriteLine();
                Console.WriteLine("Example: DEvaheb.exe \"C:\\Temp\\real_scripts\\intro.IBI\" -open \"notepad\"");
                Console.WriteLine();
            }
            else
            {
                string sourceFile = args[0];
                string targetFile = Path.ChangeExtension(sourceFile, "icarus");
                string editor = string.Empty;

                if (!File.Exists(sourceFile))
                {
                    Console.WriteLine($"ERROR! File \"{sourceFile}\" doesn't exist");
                    return;
                }

                if (args.Length > 1)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        if (args[i].Equals("-output", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (i + 1 < args.Length)
                            {
                                i++;
                                targetFile = args[i];
                            }
                            else
                            {
                                Console.WriteLine("ERROR! Expected file path after \"output\" parameter");
                                return;
                            }
                        }
                        else if (args[i].Equals("-open", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (i + 1 < args.Length)
                            {
                                i++;
                                editor = args[i];
                            }
                            else
                            {
                                Console.WriteLine("ERROR! Expected file path after \"open\" parameter");
                                return;
                            }
                        }
                    }
                }

                var ibi = new DEvahebLib.IBIReader();

                //ibi.Read(@"D:\temp\real_scripts\_brig\poormunro.IBI");
                //ibi.Read(@"D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\test.IBI");
                //ibi.Read(@"D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\JAscripts\scripts\academy1\intro.IBI");
                //ibi.Read(@"D:\temp\barrel_costa_loopbck.IBI");
                //var output = ibi.Read(@"D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\EFScripts\PAK3\_holodeck_garden\boothby_workloop.IBI");

                var output = ibi.Read(sourceFile);

                File.WriteAllText(targetFile, output);

                if (!string.IsNullOrWhiteSpace(editor))
                {
                    Process.Start(editor, $"\"{targetFile}\"");
                }
            }
        }
    }
}