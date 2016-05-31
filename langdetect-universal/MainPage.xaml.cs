using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace langdetect_universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        WordStorage wordStorage;
        LangDetect langDetect;
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size { Height = 480, Width = 375 };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            wordStorage = new WordStorage();
            langDetect = new LangDetect(wordStorage);
        }

        private async void detectLanguageBtn_Click(object sender, RoutedEventArgs e)
        {
            langDetect.Reset();
            Language lang = langDetect.Analyze(textBox.Text);
            MessageDialog msg = new MessageDialog(lang.Name);
            await msg.ShowAsync();
        }

        private async void selectStopFilesBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker fp = new FolderPicker();
            fp.FileTypeFilter.Add("*");
            StorageFolder folder = await fp.PickSingleFolderAsync();

            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                Language lang = new Language(file.DisplayName); 
                langDetect.AddLanguage(lang);

                IList<string> words = await FileIO.ReadLinesAsync(file, UnicodeEncoding.Utf8);
                foreach (string word in words)
                {
                    wordStorage.AddWord(word, lang);
                }
            }
        }
    }
}
