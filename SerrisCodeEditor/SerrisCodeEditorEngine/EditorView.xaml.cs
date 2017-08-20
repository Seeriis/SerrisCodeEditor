using SerrisCodeEditorEngine.Items;
using SerrisTabsServer.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditorEngine
{
    public sealed partial class EditorView : UserControl
    {
        public EditorView()
        {
            this.InitializeComponent();
            InitializeEditor();
        }




        /*======================
         ------PARAMETERS-------
        =======================*/




        public string Code
        {
            get
            {
                while (!Initialized)
                {
                    if (Initialized)
                        break;
                }
                return GetCode();
            }
            set
            {
                IsLoading(true);
                while (!Initialized)
                {
                    if (Initialized)
                        break;
                }

                new Languages().GetActualLanguage(CodeLanguage, editor_view);
                SetCode(value);
            }
        }
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(EditorView), null);

        public string CodeLanguage
        {
            get { return (string)GetValue(CodeLanguageProperty); }
            set { SetValue(CodeLanguageProperty, value); }
        }
        public static readonly DependencyProperty CodeLanguageProperty = DependencyProperty.Register("CodeLanguage", typeof(string), typeof(EditorView), null);

        public Brush BackgroundWait
        {
            get { return (Brush)GetValue(BackgroundWaitProperty); }
            set { SetValue(BackgroundWaitProperty, value); }
        }
        public static readonly DependencyProperty BackgroundWaitProperty = DependencyProperty.Register("BackgroundWait", typeof(Brush), typeof(EditorView), null);

        public Brush ForegroundWait
        {
            get { return (Brush)GetValue(ForegroundWaitProperty); }
            set { SetValue(ForegroundWaitProperty, value); }
        }
        public static readonly DependencyProperty ForegroundWaitProperty = DependencyProperty.Register("ForegroundWait", typeof(Brush), typeof(EditorView), null);

        public bool isReadOnly
        {
            get { return (bool)GetValue(isReadOnlyProperty); }
            set { SetValue(isReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty isReadOnlyProperty = DependencyProperty.Register("isReadOnly", typeof(bool), typeof(EditorView), null);

        private bool Initialized = false;




        /*=====================
         ------FUNCTIONS-------
        =======================*/




        //Convert C# String to JS String (HttpUtility doesn't exist in WinRT) - this function has been fund here: http://joonhachu.blogspot.fr/2010/01/c-javascript-encoder.html
        public static string JavaScriptEncode(string s)
        {
            if (s == null || s.Length == 0)
            {
                return string.Empty;
            }
            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);


            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>') || (c == '\''))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else
                {
                    if (c < ' ')
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            return sb.ToString();
        }


        public void InitializeEditor()
        {
            editor_view.Navigate(new Uri("ms-appx-web:///SerrisCodeEditorEngine/Pages/editor.html"));
            editor_view.LoadCompleted += (a, b) => { };
            Initialized = true;
        }

        private void IsLoading(bool enabled)
        {
            if(enabled)
            {
                LoadingScreen.Visibility = Visibility.Visible; loading_ring.IsActive = true;
            }
            else
            {
                LoadingScreen.Visibility = Visibility.Collapsed; loading_ring.IsActive = false;
            }
        }

        private async void SetCode(string code)
        {
            if(Initialized)
            {
                await editor_view.InvokeScriptAsync("eval", new string[] { @"editor.session.setValue('" + JavaScriptEncode(code) + "', -1);" });

                IsLoading(false);
            }
        }

        private string GetCode()
        {
            if (Initialized)
                return editor_view.InvokeScriptAsync("eval", new string[] { @"editor.getValue()" }).GetResults();
            else
                return "";
        }

        private void editor_view_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {

        }

        private void editor_view_ScriptNotify(object sender, NotifyEventArgs e)
        {

        }
    }
}
