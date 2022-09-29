using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace nTrehou_Pendu
{
    internal class Game
    {
        private readonly Random random = new Random();
        private readonly string[] words = System.IO.File.ReadAllLines("../../../ressource/words.txt");
        private string word;
        private int tries = 6;
        private string letters = string.Empty;
        public Game()
        {
            this.word = this.words[this.random.Next(this.words.Length)];
            removeAccent();
        }

        public void Try(char letter)
        {
            if (this.letters.Contains(letter))
            {
                return;
            }

            this.letters += letter + " ";

            if (!this.word.Contains(letter))
            {
                this.tries--;
            }

        }
        public bool IsLost()
        {
            return this.tries <= 0;
        }

        public bool IsWon()
        {
            return this.word.All(this.letters.Contains);
        }

        public string GetWord()
        {
            return this.word;
        }

        public string GetLetters()
        {
            return this.letters;
        }

        public int GetTries()
        {
            return this.tries;
        }
        public void Reset()
        {
            this.word = this.words[this.random.Next(this.words.Length)];
            removeAccent();
            this.tries = 6;
            this.letters = "";
        }
        public string GetImage()
        {
            string image = "ressource/img/pendu/";
            switch (this.tries)
            {
                case 6:
                    image += "0hanged.png";
                    break;
                case 5:
                    image += "1hanged.png";
                    break;
                case 4:
                    image += "2hanged.png";
                    break;
                case 3:
                    image += "3hanged.png";
                    break;
                case 2:
                    image += "4hanged.png";
                    break;
                case 1:
                    image += "5hanged.png";
                    break;
                case 0:
                    image += "6hanged.png";
                    break;
            }
            return image;
        }
        public void removeAccent()
        {
            this.word = this.word.Replace("é", "e");
            this.word = this.word.Replace("è", "e");
            this.word = this.word.Replace("ê", "e");
            this.word = this.word.Replace("à", "a");
            this.word = this.word.Replace("â", "a");
            this.word = this.word.Replace("î", "i");
            this.word = this.word.Replace("ï", "i");
            this.word = this.word.Replace("ô", "o");
            this.word = this.word.Replace("ö", "o");
            this.word = this.word.Replace("ù", "u");
            this.word = this.word.Replace("û", "u");
            this.word = this.word.Replace("ü", "u");
            this.word = this.word.Replace("ç", "c");
            this.word = this.word.Replace("œ", "oe");
            this.word = this.word.Replace("æ", "ae");
        }
    }
}