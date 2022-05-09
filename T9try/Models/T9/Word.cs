using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T9try.Models.T9.Base;

namespace T9try.Models.T9
{
    class Word
    {
        public Word(string construction, int nextCounter=3)
        {
            Construction = construction;
            for (int i = 0; i < nextCounter; i++)
                Next.Add(new List<Pair>());
        }

        public string Construction { get; private set; }
        /// <summary>
        /// 0-ближайшее
        /// </summary>
        public List<List<Pair>> Next { get; private set; } = new List<List<Pair>>();

        /// <summary>
        /// Добавляет новую  пару индекс-популярность 
        /// </summary>
        /// <param name="level">глубина от слова(0-близость)</param>
        /// <param name="index">индекс слова</param>
        /// <returns>true если слово добавилось/изменилась популярность</returns>
        public bool AddNext(int level, int index)
        {
            if (level > Next.Count - 1) return false;
            if (Next[level].Where(x => x.Index == index).Any())
                Next[level].Where(x => x.Index == index).First().Popularity++;
            else
                Next[level].Add(new Pair(1, index));
            return true;
        }
    }
}
