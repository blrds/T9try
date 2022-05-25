using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T9try.Models.T9.Base;

namespace T9try.Models.T9
{
    class LetterPopularity
    {
        public char Letter { get; set; } = '\0';

        public double Popularity { get; set; } = 0;

        public LetterPopularity()
        {

        }

        public LetterPopularity(char c, double p)
        {
            Letter = c;
            Popularity = p;
        }
    }
    class Letters
    {
        //словарь ключ-настоящая буква, значение - словарь отдаленностей следующих букв от настоящей
        //                                  ключ-дальность от настоящей буквы до следующей, значение - словарь букв
        //                                                                                              ключ-буква, значение - ее попоулярность на данной позиции после настоящей буквы
        public Dictionary<char, Dictionary<int, Dictionary<char, int>>> Chars { get; set; } = new Dictionary<char, Dictionary<int, Dictionary<char, int>>>();

        public bool AddLetter(char currentChar, char nextChar, int level)
        {
            if (!Char.IsLetter(nextChar)) return false;
            if (!Char.IsLetter(currentChar)) return false;
            if (level <= 0) return false;
            char nc = char.ToLower(nextChar);
            if (Chars.ContainsKey(Char.ToLower(currentChar)))
            {
                var cc = Chars[Char.ToLower(currentChar)];
                if (cc.ContainsKey(level))
                {
                    var l = cc[level];
                    if (l.ContainsKey(nc))
                        l[nc]++;
                    else
                        l.Add(nc, 1);
                }
                else
                {
                    cc.Add(level, new Dictionary<char, int>());
                    cc[level].Add(nc, 1);
                }
            }
            else
            {
                var cc = new Dictionary<int, Dictionary<char, int>>();
                Chars.Add(Char.ToLower(currentChar), cc);
                cc.Add(level, new Dictionary<char, int>());
                cc[level].Add(nc, 1);
            }
            return true;
        }
        public void LearnWord(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                char currentLetter = word[i];
                for (int j = i + 1; j < word.Length; j++)
                {
                    AddLetter(currentLetter, word[j], j - i);
                }
            }
        }
        private int GetAllMoves(char c, int level)
        {
            int answer = 0;
            foreach (var item in Chars[c][level])
            {
                answer += item.Value;
            }
            return answer;
        }
        public List<LetterPopularity>ToLetterPopularityList(List<KeyValuePair<char, int>> list, int delimeter)
        {
            var answer = new List<LetterPopularity>();
            foreach (var a in list)
                answer.Add(new LetterPopularity(a.Key, (a.Value+0.0f)/delimeter));
            return answer;
        }
        public List<LetterPopularity> GetNextLetters(string preCombo)
        {
            var answer = new List<LetterPopularity>();
            int index = 0;
            List<LetterPopularity> lastChars = new List<LetterPopularity>();
            for (index = preCombo.Length-1; index >=0; index--)
            {
                if (Chars.ContainsKey(preCombo[index]))
                    if (Chars[preCombo[index]].ContainsKey(preCombo.Length - index))
                    {
                        lastChars = ToLetterPopularityList(Chars[preCombo[index]][preCombo.Length - index].ToList(),GetAllMoves(preCombo[index], preCombo.Length - index));
                        break;
                    }
            }
            for (; index >= 0; index--)
            {
                if (!Char.IsLetter(preCombo[index])) continue;
                char currentChar = Char.ToLower(preCombo[index]);
                if (lastChars.Count == 0) break;
                List<LetterPopularity> currentChars = new List<LetterPopularity>();
                if (Chars.ContainsKey(currentChar))
                    if (Chars[currentChar].ContainsKey(preCombo.Length - index))
                        currentChars = ToLetterPopularityList(Chars[currentChar][preCombo.Length - index].ToList(), GetAllMoves(currentChar, preCombo.Length - index));
                List<LetterPopularity> nextLastChars = new List<LetterPopularity>();
                foreach(var c in lastChars)
                {
                    if (currentChars.Any(x => x.Letter == c.Letter))
                        nextLastChars.Add(new LetterPopularity(c.Letter, c.Popularity * currentChars.First(x => x.Letter == c.Letter).Popularity));
                }
                lastChars = nextLastChars;
            }
            answer = lastChars;
            answer.OrderByDescending(x=>x.Popularity);
            return answer;
        }

    }
}
