using System.ComponentModel;

namespace MAUI_Hangman
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region Fiels
        List<string> words = new List<string>()
         {
            "python",
            "javascript",
            "maui",
            "csharp",
            "mongodb",
            "sql",
            "xaml",
            "word",
            "excel",
            "powerpoint",
            "code",
            "hotreload",
            "snippets"
         };
        string answer = string.Empty;
        private string spotLight;
        public List<char> charsGuessed = new();
        private List<char> letters = new();

        #endregion

        #region UIProperties
        public string SpotLight
        {
            get { return spotLight; }
            set
            {
                spotLight = value;
                OnPropertyChanged();
            }
        }
        public List<char> Letters
        {
            get { return letters; }
            set
            {
                letters = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;
                       
            GetRandomWord();
            CalculateWord(answer, charsGuessed);
        }

        #region Game Engine
        private void GetRandomWord()
        {
            Random random = new Random();
            int randomNumber = random.Next(words.Count);
            answer = words[randomNumber];
        }

        private void CalculateWord(string answer, List<char> charsGuesses)
        {
            //answer hello
            // charguessed : l
            /*
             * h _
             * e _
             * l l
             * l l
             * o _
             */
            var tmp = answer.Select(c => charsGuesses.IndexOf(c) >= 0 ? c : '-').ToArray();
            SpotLight = string.Join(" ", tmp);
        }
        #endregion
    }
}
