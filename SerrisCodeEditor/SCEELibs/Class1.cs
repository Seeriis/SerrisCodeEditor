using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace SCEELibs
{
    public sealed class Class1
    {
        public async void sendMessage(string text)
        {
            await new MessageDialog(text).ShowAsync();
        }
    }
}
