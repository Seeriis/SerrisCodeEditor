using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace SerrisTabsServer.Manager
{
    public class EncodingType
    {
        public string EncodingName { get; set; }
        public int EncodingCodepage { get; set; }
        public bool EncodingBOM { get; set; }
    }

    public static class EncodingsHelper
    {

        public static string EncodingName(int Codepage, bool bom)
        {
            foreach(EncodingType type in EncodingsAvailable)
            {
                if(type.EncodingCodepage == Codepage && type.EncodingBOM == bom)
                {
                    return type.EncodingName;
                }
            }

            return EncodingsAvailable[0].EncodingName;
        }

        public static EncodingType[] EncodingsAvailable = new EncodingType[]
        {
            new EncodingType
            {
                EncodingName = new ResourceLoader().GetString("encodings-utf8nobom"),
                EncodingCodepage = Encoding.UTF8.CodePage,
                EncodingBOM = false
            },

            new EncodingType
            {
                EncodingName = new ResourceLoader().GetString("encodings-utf8bom"),
                EncodingCodepage = Encoding.UTF8.CodePage,
                EncodingBOM = true
            },

            new EncodingType
            {
                EncodingName = "UTF-7",
                EncodingCodepage = Encoding.UTF7.CodePage,
                EncodingBOM = true
            },

            new EncodingType
            {
                EncodingName = "UTF-32",
                EncodingCodepage = Encoding.UTF32.CodePage,
                EncodingBOM = true
            },

            new EncodingType
            {
                EncodingName = "Unicode",
                EncodingCodepage = Encoding.Unicode.CodePage,
                EncodingBOM = true
            },

            new EncodingType
            {
                EncodingName = "ASCII",
                EncodingCodepage = Encoding.ASCII.CodePage,
                EncodingBOM = true
            },

            new EncodingType
            {
                EncodingName = "Big Endian Unicode",
                EncodingCodepage = Encoding.BigEndianUnicode.CodePage,
                EncodingBOM = true
            }
        };

    }
}
