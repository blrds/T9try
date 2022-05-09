using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T9try.Models.T9
{
    class Settings
    {
        public static Settings Setting = new();

        public readonly int NextWords;
        public readonly List<char> SymbolsToIgnore;
        private Settings() {
            NextWords = 3;
            SymbolsToIgnore = new List<char>
            {
                ',',
                '(',
                ')',
                '[',
                ']',
                '\n',
                '\r',
                '\t'
            };
        }
    }
}
