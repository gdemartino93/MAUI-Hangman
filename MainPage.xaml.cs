using System.ComponentModel;
using System.Diagnostics;

namespace MAUI_Hangman
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;

            GetRandomWord();
            CalculateWord(answer, charsGuessed);
        }
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
        private int errorsCount = 0;
        private bool isGameOver = false;
        private string message;
        private string gameStatus;
        private int maxAttempts = 6;
        private string image = "img0";
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

        public int ErrorsCount
        {
            get { return errorsCount; }
            set
            {
                errorsCount++;
                OnPropertyChanged();
            }
        }
        public bool IsGameOver
        {
            get { return isGameOver; }
            set
            {
                isGameOver = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public string GameStatus
        {
            get { return gameStatus; }

            set
            {
                if (gameStatus != value)
                {
                    gameStatus = value;
                    OnPropertyChanged(nameof(GameStatus));
                }
            }
        }
        public string Image { get => image; set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }
        #endregion

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

        private void HandleGuess(char letter)
        {
            if (charsGuessed.IndexOf(letter) == -1)
            {
                charsGuessed.Add(letter);
            }
            if (answer.IndexOf(letter) >= 0)
            {
                CalculateWord(answer, charsGuessed);
                CheckIfGameWon();
            }
            else if (errorsCount < 5)
            {
                errorsCount++;
                UpdateStatus();
                UpadateImageDueError();
            }
            else
            {
                IsGameOver = true;
                Message = "Hai perso!";
                ToggleButtonOnGameEnd(false);
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Errori: {errorsCount} di {maxAttempts}";
        }

        private void CheckIfGameWon()
        {
            if (!SpotLight.Contains('-'))
            {
                Message = "Hai Vinto!";
                ToggleButtonOnGameEnd(false);
            }
        }

        private void UpadateImageDueError()
        {
            Image = $"img{errorsCount}";
        }

        private void ToggleButtonOnGameEnd(bool toEnable)
        {
            foreach (var children in BtnContainers.Children)
            {
                Button? btn = children as Button;
                if (btn != null)
                {
                    btn.IsEnabled = toEnable;
                }
            }
        }

        #endregion

        private void Button_ClickedLetter(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn != null)
            {
                var letter = btn.Text;
                btn.IsEnabled = false;
                HandleGuess(letter[0]);
            }
        }

        private void Button_Clicked_Reset(object sender, EventArgs e)
        {
            errorsCount = 0;
            charsGuessed = new List<char>();
            UpadateImageDueError();
            UpdateStatus();
            Message = string.Empty;
            IsGameOver = false;
            GetRandomWord();
            CalculateWord(answer, charsGuessed);
            ToggleButtonOnGameEnd(true);
        }
    }
}
