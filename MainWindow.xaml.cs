using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace nTrehou_Pendu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // aficher les tiret

        private List<Label> Labels { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Labels = new List<Label>();
            startNewGame();

        }
        Game Hanged = new Game();
        char letterTry = ' ';
        string Letters = "";
        char[] Alphabet = new char[]
        {
            'a', 'b', 'c', 'd', 'e',
            'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n','o',
            'p', 'q', 'r', 's', 't',
            'u', 'v', 'w','x', 'y',
            'z'
        };

        public void startNewGame()
        {
            Letters = Hanged.GetLetters();
            if (Letters != string.Empty)
            {
                Hanged.Reset();
            }
            tb_LetterTry.Content = ""; ; // reset all the letter try

            Uri img = new Uri(Hanged.GetImage(), UriKind.Relative); // set the img to the first one
            imgTries.Source = new BitmapImage(img);
            // reset all the labels
            Labels.Clear();
            WordPanel.Children.Clear();


            allBtnAlphabet(true); // enable all the button
            CreateCharactereLbl(Hanged.GetWord().Length);
        }
        private void onClick_restart(object sender, RoutedEventArgs e)
        {
            startNewGame();
        }
        private void onClick_Letter(object sender, RoutedEventArgs e)
        {
            Button letter = sender as Button;
            letterTry = letter.Content.ToString()[0];
            tryLetter(letterTry);

        }
        private void Keypress(object sender, KeyEventArgs e)
        {
            if (!Hanged.IsLost() || !Hanged.IsWon())
            {

            }
            letterTry = e.Key.ToString().ToLower()[0]; // verifier si j'ai gagner ou perdu bloquer ici
            tryLetter(letterTry);
        }
        private void tryLetter(char letter)
        {
            letterTry = letter.ToString().ToLower()[0];
            Hanged.Try(letterTry);
            tb_LetterTry.Content = Hanged.GetLetters();
            revealGoodLetter(letterTry);


            Button btn = GetBtn(letterTry);
            btn.IsEnabled = false;
            Uri img = new Uri(Hanged.GetImage(), UriKind.Relative);
            imgTries.Source = new BitmapImage(img);

            if (Hanged.IsLost())
            {
                MessageBox.Show("Dommage !" + " Le mot était : " + Hanged.GetWord());
                startNewGame();
            }
            else if (Hanged.IsWon())
            {
                MessageBox.Show("Bravo !" + " Le mot était : " + Hanged.GetWord());
                startNewGame();
            }
        }

        private void revealGoodLetter(char letter)
        {
            int index = Hanged.GetWord().IndexOf(letter);
            while (index != -1)
            {
                Labels[index].Content = letter;
                index = Hanged.GetWord().IndexOf(letter, index + 1);
            }
        } // reveal the good letter in the word

        private Button GetBtn(char c)
        {
            string btn_Name = "btn_" + c;
            Button btn = (Button)System.Windows.Application.Current.MainWindow.FindName(btn_Name);
            return btn;
        } // Fin a button on my window from his name
        #region Create Game field
        private void CreateCharactereLbl(int lenght)
        {
            Random rnd = new Random();
            for (int i = 0; i < lenght; i++)
            {
                Label label = new Label();
                label.RenderTransform = new RotateTransform(rnd.Next(-7, 7));

                label.Name = "Character" + i.ToString();
                if (Hanged.GetWord()[i] == '-')
                {
                    label.Content = "_";
                }
                else
                {
                    label.Content = "?";
                }
                Labels.Add(label);
                WordPanel.Children.Add(label);

            }
        }
        private void allBtnAlphabet(bool status)
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
        } // get all the Alphabet button enabled or disabled
        #endregion
    }
}