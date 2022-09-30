using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace nTrehou_Pendu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Label> Labels { get; set; } 
        public MainWindow()
        {
            InitializeComponent();
            Labels = new List<Label>(); 
            
            //Create a timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);               // interval 1 second
            timer.Tick += Timer_Tick; 
            timer.Start(); 
            
            StartNewGame(); 
        } 
        Game Hanged = new Game();
        int timeTimer = 61;                                         // 61 annd not 60 because the timer start at 0
        char letterTry = ' ';                                       // letter try by the player
        string Letters = "";                                        // all the letters try by the player
        char[] Alphabet = new char[] 
        {
            'a', 'b', 'c', 'd', 'e',
            'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n','o',
            'p', 'q', 'r', 's', 't',
            'u', 'v', 'w','x', 'y',
            'z'
        };

        #region Create Game field
        private void CreateCharactereLbl(int lenght)
        {
            Random rnd = new Random();
            for (int i = 0; i < lenght; i++)
            {
                Label label = new Label(); 
                label.RenderTransform = new RotateTransform(rnd.Next(-7, 7));   // random rotation -7° to 7°

                label.Name = "Character" + i.ToString();
                if (Hanged.GetWord()[i] == '-')                                 // To display the - in the word
                {
                    label.Content = "_";
                }
                else
                {
                    label.Content = "?";
                }
                Labels.Add(label);
                WordPanel.Children.Add(label);                                  // add the label to the panel

            }
        }
        private void AllBtnAlphabet(bool status)
        {
            if (status) 
            {
                foreach (char i in Alphabet)
                {
                    Button btnLetter = GetBtn(i);
                    btnLetter.IsEnabled = true;
                }
            }
            else
            {
                foreach (char i in Alphabet)
                {
                    Button btnLetter = GetBtn(i);
                    btnLetter.IsEnabled = false;
                }
            }
        }                 // get all the Alphabet button enabled or disabled
        #endregion
        
        #region start a new game
        public void StartNewGame()
        {
            Letters = Hanged.GetLetters();                          // get the letters try by the player
            if (Letters != string.Empty)
            {
                Hanged.Reset();                                     // reset the game
            }
            tb_Timer.Content = 60.ToString();                       // reset the timer
            Hanged.SetTime(timeTimer);                              // set the time to the game
            
            tb_LetterTry.Content = ""; ;                            // reset all the letter try
            
            Uri img = new Uri(Hanged.GetImage(), UriKind.Relative); // reset img state
            imgTries.Source = new BitmapImage(img);                 // reset img state

            Labels.Clear();                                         // clear the list of labels
            WordPanel.Children.Clear();                             // clear the panel of labels
            AllBtnAlphabet(true);                                   // enable all the button
            CreateCharactereLbl(Hanged.GetWord().Length);

        }
        #endregion

        #region Manage input
        private void Keypress(object sender, KeyEventArgs e)        
        {
            if (e.Key >= Key.A && e.Key <= Key.Z)                   // if the key is a letter
            {
                letterTry = e.Key.ToString().ToLower()[0];          // get the letter
                CheckLetter(letterTry);
            }
        }// manage the key press
        private void OnClick_Letter(object sender, RoutedEventArgs e)
        {
            Button letter = sender as Button;
            letterTry = letter.Content.ToString()[0];
            CheckLetter(letterTry);
        }// manage the click on a character button
        private void OnClick_restart(object sender, RoutedEventArgs e) => StartNewGame(); // manage the click on the restart button
        #endregion

        #region fonctionnement du jeu
        private void CheckLetter(char letter)
        {
            letterTry = letter.ToString().ToLower()[0]; 
            Hanged.Try(letterTry); 
            if (Hanged.GetWord().Contains(letterTry))
            {
                RevealGoodLetter(letterTry);
            }
            else
            {
                Uri img = new Uri(Hanged.GetImage(), UriKind.Relative);
                imgTries.Source = new BitmapImage(img);
            }
            tb_LetterTry.Content = Hanged.GetLetters();
            Button btn = GetBtn(letterTry);
            btn.IsEnabled = false;


            if (Hanged.IsLost())
            {
                MessageBox.Show("Dommage !" + " Le mot était : " + Hanged.GetWord());
                StartNewGame();
            }
            else if (Hanged.IsWon())
            {
                MessageBox.Show("Bravo !" + " Le mot était : " + Hanged.GetWord() + " || Vous avez reussi en :" + (61 - Hanged.GetTime()) + " secondes");
                StartNewGame();
            }
        }                   // check if the letter is in the word
        private void RevealGoodLetter(char letter)
        {
            int index = Hanged.GetWord().IndexOf(letter);               // get the index of the letter in the word
            while (index != -1)
            {
                Labels[index].Content = letter; 
                index = Hanged.GetWord().IndexOf(letter, index + 1);    // search the next letter
            }
        }              // reveal the good letter in the word
        #endregion

        #region Utils
        private Button GetBtn(char c)
        {
            string btn_Name = "btn_" + c;
            Button btn = (Button)System.Windows.Application.Current.MainWindow.FindName(btn_Name);
            return btn;
        }                           // Fin a button on my window from his name

        #endregion

        #region timer
        void Timer_Tick(object sender, EventArgs e)
        {
            int time = Hanged.GetTime();
            if (time > 0)
            {
                Hanged.SetTime(time - 1);
                tb_Timer.Content = Hanged.GetTime();
            }
            else
            {
                MessageBox.Show("Dommage !" + " Le mot était : " + Hanged.GetWord());
                StartNewGame();
            }
        }             // timer tick
        #endregion
    }
}
