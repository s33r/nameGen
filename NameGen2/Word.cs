using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen2
{
    class Word
    {
        public string Name { get; private set; }
        public string FriendlyName { get; private set; }
        public string PartOfSpeech { get; private set; }

        public Word(string name, string partOfSpeech)
        {
            Name = name;
            PartOfSpeech = partOfSpeech;
            FriendlyName = Name.Replace('_', ' ');
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", PartOfSpeech, Name);
        }

    }
}
