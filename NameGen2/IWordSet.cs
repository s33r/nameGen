using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen2
{
    interface IWordSet
    {

        string IndexFilePath { get; }
        string DataFilePath { get; }
        string PartOfSpeech { get; }

        void LoadWords();

        Word GetRandomWord();

        
    }
}
