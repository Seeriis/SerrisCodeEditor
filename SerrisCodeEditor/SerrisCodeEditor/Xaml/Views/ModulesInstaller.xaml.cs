using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Views
{

    public sealed partial class ModulesInstaller : Page
    {
        StorageFile ModuleFile;
        ModulesVerifyAssistant VerifyAssistant;

        public ModulesInstaller()
        {
            this.InitializeComponent();
        }

        private async void OpenModuleButton_Click(object sender, RoutedEventArgs e)
        {
            var opener = new FileOpenPicker();
            opener.ViewMode = PickerViewMode.Thumbnail;
            opener.SuggestedStartLocation = PickerLocationId.Downloads;
            opener.FileTypeFilter.Add(".zip");
            opener.FileTypeFilter.Add(".scea");

            StorageFile Module = await opener.PickSingleFileAsync();

            if(Module != null)
            {
                ModuleFile = Module;
                StorageApplicationPermissions.FutureAccessList.Add(Module);

                //ModuleFile = await Module.CopyAsync(ApplicationData.Current.TemporaryFolder);
                VerifyAssistant = new ModulesVerifyAssistant(Module);

                //Get package infos and show them !
                InfosModule infos = await VerifyAssistant.GetPackageInfosAsync();

                TitleModule.Text = infos.ModuleName;
                AuthorName.Text = infos.ModuleAuthor;
                DescriptionText.Text = infos.ModuleDescription;
                ModuleType.Text = infos.ModuleType.ToString();
                VersionNumber.Text = infos.ModuleVersion.GetVersionInString();

                SelectModuleGrid.Visibility = Visibility.Collapsed;
                VerifyModuleGrid.Visibility = Visibility.Visible;
            }
        }

        private async void InstallModuleButton_Click(object sender, RoutedEventArgs e)
        {
            if(ModuleFile != null)
            {
                PackageVerificationCode CodeResult = await VerifyAssistant.VerifyPackageAsync();

                if (CodeResult == PackageVerificationCode.Passed)
                {
                    if(await ModulesWriteManager.AddModuleAsync(ModuleFile))
                    {
                        ResultText.Text = "Module has been installed without any problem !";
                    }
                    else
                    {
                        ResultText.Text = "Module was not installed :(";
                    }
                }
                else
                {
                    ResultText.Text = "Error with the module: " + CodeResult.ToString();
                }

                VerifyModuleGrid.Visibility = Visibility.Collapsed;
                ResultInstallation.Visibility = Visibility.Visible;
            }
        }
    }

}
