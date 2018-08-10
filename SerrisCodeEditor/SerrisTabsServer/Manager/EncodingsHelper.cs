using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Manager
{
    public class EncodingType
    {
        public string EncodingName { get; set; }
        public int EncodingCodepage { get; set; }
    }

    public static class EncodingsHelper
    {

        public static EncodingType[] EncodingsAvailable = new EncodingType[] 
        {
            new EncodingType
            {
                EncodingName = "UTF-8 (default)",
                EncodingCodepage = Encoding.UTF8.CodePage
            },

            new EncodingType
            {
                EncodingName = "UTF-7",
                EncodingCodepage = Encoding.UTF7.CodePage
            },

            new EncodingType
            {
                EncodingName = "UTF-32",
                EncodingCodepage = Encoding.UTF32.CodePage
            },

            new EncodingType
            {
                EncodingName = "Unicode",
                EncodingCodepage = Encoding.Unicode.CodePage
            },

            new EncodingType
            {
                EncodingName = "ASCII",
                EncodingCodepage = Encoding.ASCII.CodePage
            },

            new EncodingType
            {
                EncodingName = "Big Endian Unicode",
                EncodingCodepage = Encoding.BigEndianUnicode.CodePage
            }
        };

    }
}
