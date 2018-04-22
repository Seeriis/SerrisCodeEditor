using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisCodeEditor.Xaml.Views;
using SerrisModulesServer;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Theme;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor
{
    /// <summary>
    /// Fournit un comportement spécifique à l'application afin de compléter la classe Application par défaut.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initialise l'objet d'application de singleton.  Il s'agit de la première ligne du code créé
        /// à être exécutée. Elle correspond donc à l'équivalent logique de main() ou WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoqué lorsque l'application est lancée normalement par l'utilisateur final.  D'autres points d'entrée
        /// seront utilisés par exemple au moment du lancement de l'application pour l'ouverture d'un fichier spécifique.
        /// </summary>
        /// <param name="e">Détails concernant la requête et le processus de lancement.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            //Serris Modules Server !
            SMSInitialize.InitializeSMSJson();
            GlobalVariables.CurrentTheme = await new ThemeReader(ModulesAccessManager.GetCurrentThemeID()).GetThemeBrushesContent();

            Messenger.Default.Register<SMSNotification>(this, async (notification) =>
            {
                try
                {
                    switch (notification.Type)
                    {
                        case TypeUpdateModule.CurrentThemeUpdated:
                            GlobalVariables.CurrentTheme = await new ThemeReader(notification.ID).GetThemeBrushesContent();
                            Messenger.Default.Send(new EditorViewNotification { ID = 0, type = EditorViewNotificationType.UpdateUI });
                            break;
                    }
                }
                catch { }
            });

            Messenger.Default.Register<TempContentNotification>(this, (notification) =>
            {
                try
                {
                    switch(notification.type)
                    {
                        case TempContentType.currentIDs when !notification.answerNotification:
                            Messenger.Default.Send(new TempContentNotification { answerNotification = true, type = TempContentType.currentIDs, content = GlobalVariables.CurrentIDs });
                            break;

                        case TempContentType.currentDevice when !notification.answerNotification:
                            Messenger.Default.Send(new TempContentNotification { answerNotification = true, type = TempContentType.currentDevice, content = GlobalVariables.CurrentDevice });
                            break;

                        case TempContentType.currentTheme when !notification.answerNotification:
                            Messenger.Default.Send(new TempContentNotification { answerNotification = true, type = TempContentType.currentTheme, content = GlobalVariables.CurrentTheme });
                            break;
                    }
                }
                catch { }
            });

            // Ne répétez pas l'initialisation de l'application lorsque la fenêtre comporte déjà du contenu,
            // assurez-vous juste que la fenêtre est active
            if (rootFrame == null)
            {
                // Créez un Frame utilisable comme contexte de navigation et naviguez jusqu'à la première page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: chargez l'état de l'application précédemment suspendue
                }

                // Placez le frame dans la fenêtre active
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quand la pile de navigation n'est pas restaurée, accédez à la première page,
                    // puis configurez la nouvelle page en transmettant les informations requises en tant que
                    // paramètre
                    rootFrame.Navigate(typeof(EditorView), e.Arguments);
                }
                // Vérifiez que la fenêtre actuelle est active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Appelé lorsque la navigation vers une page donnée échoue
        /// </summary>
        /// <param name="sender">Frame à l'origine de l'échec de navigation.</param>
        /// <param name="e">Détails relatifs à l'échec de navigation</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Appelé lorsque l'exécution de l'application est suspendue.  L'état de l'application est enregistré
        /// sans savoir si l'application pourra se fermer ou reprendre sans endommager
        /// le contenu de la mémoire.
        /// </summary>
        /// <param name="sender">Source de la requête de suspension.</param>
        /// <param name="e">Détails de la requête de suspension.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            //TODO: enregistrez l'état de l'application et arrêtez toute activité en arrière-plan
            deferral.Complete();
        }
    }
}
