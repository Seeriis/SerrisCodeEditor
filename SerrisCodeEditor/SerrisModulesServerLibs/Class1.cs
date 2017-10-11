using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace SerrisModulesServerLibs
{
    public sealed class Lol
    {
        public async void sendMessage(string text)
        {
            await new MessageDialog(text).ShowAsync();
        }
    }
}
