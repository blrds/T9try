using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T9try.Models.T9.Base
{
    class Pair
    {
        public Pair(double popularity, int index)
        {
            Popularity = popularity;
            Index = index;
        }

        public Pair(Pair pair) {
            Popularity = pair.Popularity;
            Index = pair.Index;
        }

        public Pair() {
            Popularity = 0;
            Index = -1;
        }

        public double Popularity { get; set; }
        public int Index { get; set; }

        public static List<Pair> AddNewPair(Pair pair, List<Pair> pairs) {
            if (pairs.Where(x => x.Index == pair.Index).Any())
                pairs.Where(x => x.Index == pair.Index).First().Popularity += pair.Popularity;
            else pairs.Add(new Pair(pair));
            return pairs;
        } 
    }
}
