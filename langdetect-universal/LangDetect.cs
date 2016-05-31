using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
namespace langdetect_universal
{
    public class Language
    {
        public Language(string name)
        {
            Name = name;
            Occurances = 0;
        }
        public string Name { get; }
        public int Occurances { get; set; }
    }
    public class WordStorage
    {
        private Dictionary<string, List<Language>> words = new Dictionary<string, List<Language>>();

        public void AddWord(string word, Language l)
        {
            List<Language> languageList;
            if (words.TryGetValue(word, out languageList))
            {
                // word is already in the dictionary
                languageList.Add(l);
            }
            else
            {
                languageList = new List<Language>();
                languageList.Add(l);
                words.Add(word, languageList);
            }
        }   
        
        /// <summary>
        /// Tries to find the given word in the dictionary. 
        /// </summary>
        /// <param name="word">The word to search for</param>
        /// <returns>The list of languages the word is found in, or null if the word is not in the dictionary</returns>
        public List<Language> FindWord(string word)
        {
            List<Language> languageList;
            if (words.TryGetValue(word, out languageList))
            {
                return languageList;
            }
            else
            {
                return null;
            }
        }
    }

    public class LangDetect
    {
        private WordStorage words;
        private List<Language> languages = new List<Language>();
        public LangDetect(WordStorage w)
        {
            words = w;
        }

        /// <summary>
        /// Adds a language to the detector
        /// </summary>
        /// <param name="l">The language to add</param>
        public void AddLanguage (Language l)
        {
            languages.Add(l);
        }

        /// <summary>
        /// Return the language with the most words in the given text
        /// </summary>
        /// <param name="text">The text to analyze</param>
        /// <returns>The language with the most words in the given text</returns>
        public Language Analyze(string text)
        {
            string[] inputWords = text.Split(' ');
            foreach (string word in inputWords)
            {
                ProcessWord(word);
            }

            return languages.MaxBy(x => x.Occurances);
        }

        /// <summary>
        /// Updates the occurances for each language the word appears in
        /// </summary>
        /// <param name="word">The word to process</param>
        private void ProcessWord(string word)
        {
            List<Language> languageList;
            if ((languageList = words.FindWord(word)) != null)
            {
                languageList.ForEach(x => x.Occurances++);
            }
        }

        public void Reset()
        {
            languages.ForEach(x => x.Occurances = 0);
        }
    }
}
