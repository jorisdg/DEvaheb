namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ibi = new DEvaheb.IBIReader();

            ibi.Read(@"D:\temp\real_scripts\_brig\poormunro.IBI");
            //ibi.Read(@"D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\test.IBI");
        }
    }
}