using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using T9try.Infrastructure.Commands;
using T9try.Models.T9;
using T9try.ViewModels.Base;



namespace T9try.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        ~MainWindowViewModel() {
            Dictionary.Dispose();
        }
        public ICommand FileCommand { get; }
        private bool CanFileCommnadExecute(object p) => true;
        private void OnFileCommandExecuted(object p)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt";
            ofd.AddExtension = true;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    Dictionary.Study(ofd.FileName, Rewrite);
                }
                catch (Exception e) { }
            }
        }

        public ICommand StudyCommand { get; }
        private bool CanStudyCommnadExecute(object p) {
            try
            {
                return Text != "" && Text.Where(x => Char.IsLetterOrDigit(x)).Any();
            }
            catch (Exception e) {
                return false;
            }
        }
        private void OnStudyCommandExecuted(object p)
        {
            Dictionary.StudyNew(Text);
            if (Clean) Text = "";
        }
        Dictionary Dictionary = new Dictionary();
        public string Text { get; set; }

        public string Suggestion1 { get; set; }
        public string Suggestion2 { get; set; }
        public string Suggestion3 { get; set; }
        public bool Rewrite { get; set; }
        public bool Clean { get; set; }

        
        public MainWindowViewModel()
        {
            FileCommand = new LambdaCommand(OnFileCommandExecuted, CanFileCommnadExecute);
            StudyCommand = new LambdaCommand(OnStudyCommandExecuted, CanStudyCommnadExecute);
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text == "") return;
            if (Text.Last() == ' ')
            {
                var sentences = Text.Split('.');
                var words = sentences.Last().Split(' ');
                words = words.Where(x => x != "" && x != " ").ToArray();
                string[] sendInWords = new string[Settings.Setting.NextWords];
                string str = "";
                for (int i = 0; i < Settings.Setting.NextWords; i++)
                {
                    int index = words.Length - Settings.Setting.NextWords + i;

                    if (index >= 0) str = words[index];
                    else str = "";

                    Dictionary.AddNext(str, ref sendInWords);
                }
                var nextWords = Dictionary.WhatsNext(sendInWords);
                var a = nextWords.OrderByDescending(x => x.Popularity).ToArray();

                if (a.Length >= 1)
                    Suggestion1 = Dictionary.Words[a[0].Index].Construction;
                else Suggestion1 = "";
                OnPropertyChanged("Suggestion1");
                if (a.Length >= 2)
                    Suggestion2 = Dictionary.Words[a[1].Index].Construction;
                else Suggestion2 = "";
                OnPropertyChanged("Suggestion2");
                if (a.Length >= 3)
                    Suggestion3 = Dictionary.Words[a[2].Index].Construction;
                else Suggestion3 = "";
                OnPropertyChanged("Suggestion3");

            }
            else { }
        }
        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dictionary.Dispose();
        }
    }
}
