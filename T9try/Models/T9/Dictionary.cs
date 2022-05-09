﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T9try.Models.T9.Base;

namespace T9try.Models.T9
{
    class Dictionary
    {
        public List<Word> Words { get; private set; } = new List<Word>();

        public Dictionary()
        {
        }

        /// <summary>
        /// предсказывает след слово
        /// </summary>
        /// <param name="prevWords">предыдущие н слов, где н-1 слово последнее перед новым</param>
        /// <returns></returns>
        public List<Pair> WhatsNext(string[] prevWords)
        {
            List<Pair> answer = new();
            int count = Math.Min(prevWords.Length, Settings.Setting.NextWords);
            for (int level = 0; level < count; level++)
            {
                var word = GetWord(prevWords[prevWords.Length - 1 - level]);
                if (word == null) continue;

                foreach (var a in word.Next[level])
                    answer = Pair.AddNewPair(new Pair(a.Popularity / (level + 1), a.Index), answer);

            }
            return answer;
        }

        public int GetIndex(string word)
        {
            int answer = -1;
            for (int i = 0; i < Words.Count; i++)
                if (Words[i].Construction == word)
                    return i;
            return answer;
        }

        public Word GetWord(string word)
        {
            try
            {
                return Words.Where(x => x.Construction == word).First();
            }
            catch (Exception e) { }
            return null;
        }

        public void Study(string file, bool reset = true)
        {
            if (reset) Words.Clear();
            string text = File.ReadAllText(file);
            StudyNew(text);
        }
        public void StudyNew(string text)
        {
            foreach (var a in Settings.Setting.SymbolsToIgnore)
                text = text.Replace(a, ' ');
            var lines = text.Split('.');
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                words = words.Where(x => x != " " && x != "").ToArray();
                if (words.Length == 0) continue;
                foreach (var a in words)
                    AddNewWord(a);
                string[] next = new string[Settings.Setting.NextWords];

                for (int i = 0; i < Settings.Setting.NextWords; i++)
                {
                    if (i + 1 >= words.Length) AddNext("", ref next);
                    AddNext(words[i + 1], ref next);

                }

                int wordIndex = 0;
                for (int i = 0; i < words.Length; i++)
                {
                    wordIndex = GetIndex(words[i]);
                    for (int j = 0; j < next.Length; j++)
                    {
                        Words[wordIndex].AddNext(j, GetIndex(next[j]));
                    }

                    if (i + Settings.Setting.NextWords + 1 >= words.Length) AddNext("", ref next);
                    else AddNext(words[i + Settings.Setting.NextWords + 1], ref next);
                }
            }
        }
        public static bool AddNext(string word, ref string[] next)
        {
            if (word == "") return false;
            for (int i = 0; i < next.Length - 1; i++)
            {
                next[i] = next[i + 1];
            }
            next[^1] = word;
            return true;
        }
        private bool AddNewWord(string word)
        {
            if (Words.Where(x => x.Construction == word).Any()) return false;
            Words.Add(new Word(word));
            return true;
        }
    }
}