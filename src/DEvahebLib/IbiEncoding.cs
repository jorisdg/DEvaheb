using System.Text;

namespace DEvahebLib
{
    /// <summary>
    /// Provides the Windows-1252 (ANSI) encoding used for string data in IBI binary files.
    /// </summary>
    public static class IbiEncoding
    {
        public static Encoding Windows1252 { get; }

        static IbiEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Windows1252 = Encoding.GetEncoding(1252);
        }
    }
}
