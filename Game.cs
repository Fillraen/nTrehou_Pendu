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
        private readonly string[] words = System.IO.File.ReadAllLines("../../../ressource/words.txt"); // read the file words.txt or you can use a list of words
        private string word;
        private int tries = 6;      // number of tries-lifes
        private string letters = string.Empty; 
        private int time = 61; //how many second to guess
        public Game()
        {
            this.word = this.words[this.random.Next(this.words.Length)]; // choose a random word
            RemoveAccent(); // remove the accent of the word
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
            return this.word.All(this.letters.Contains); // return true if all the letters of the word are in the letters string
        }

        public string GetWord()
        {
            return this.word;
        }

        public string GetLetters()
        {
            return this.letters;
        }
        public void SetTries(int tries)
        {
            this.tries = tries;
        }
        
        public string GetImage()  
        {
            string image = "ressource/img/pendu/";
            
            image += this.tries switch 
            {
                6 => "0hanged.png",
                5 => "1hanged.png",
                4 => "2hanged.png",
                3 => "3hanged.png",
                2 => "4hanged.png",
                1 => "5hanged.png",
                0 => "6hanged.png",
                _ => "0hanged.png",
            };
            return image; // return the image path
        }
        
        public void RemoveAccent()
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
            
        } // remove the accent or special character of a word
        
        public void Reset()
        {
            this.word = this.words[this.random.Next(this.words.Length)]; // choose a new word
            RemoveAccent(); // remove the accent of the word
            this.tries = 6; // reset the number of tries
            this.letters = ""; // reset the letters
        }
        
        public void SetTime(int time)
        {
            this.time = time;
        }
        
        public int GetTime()
        {
            return this.time;
        }
    }
}