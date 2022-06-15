namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ibi = new DEvaheb.IBIReader();

            //ibi.Read(@"D:\temp\real_scripts\_brig\poormunro.IBI");
            //ibi.Read(@"D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\test.IBI");
            //ibi.Read(@"D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\JAscripts\scripts\academy1\intro.IBI");
            ibi.Read(@"D:\temp\barrel_costa_loopbck.IBI");
        }
    }
}