using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen2
{
    class Program
    {
        static void Main(string[] args)
        {
            WordSet nouns = new WordSet("noun");
            WordSet adjectives = new WordSet("adj");
            WordSet adverbs = new WordSet("adv");
            WordSet verbs = new WordSet("verb");

            Console.WriteLine("Loading word lists...");

            nouns.LoadWords();
            adjectives.LoadWords();
            adverbs.LoadWords();
            verbs.LoadWords();

            Console.WriteLine("... Done.");         

            for (char currentKey = ' '; Char.ToLower(currentKey) != 'q'; currentKey = Console.ReadKey(true).KeyChar)
            {
                Console.Clear();
                Console.WriteLine(new String('=', 79));
                Console.WriteLine("Aaron's phrase generator");
                Console.WriteLine("[q] Quit");
                Console.WriteLine("[1] Noun Phrase (adjective noun)");
                Console.WriteLine("[2] Verb Phrase (adverbs verbs)");
                Console.WriteLine(new String('=', 79));

                if (currentKey == '1')
                {
                    var noun = nouns.GetRandomWord().FriendlyName;
                    var adjective = adjectives.GetRandomWord().FriendlyName;

                    Console.WriteLine("{0} {1}", adjective, noun);
                }
                else if (currentKey == '2')
                {
                    var verb = verbs.GetRandomWord().FriendlyName;
                    var adverb = adverbs.GetRandomWord().FriendlyName;

                    Console.WriteLine("{0} {1}", adverb, verb);
                }
            }
        }
    }
}
