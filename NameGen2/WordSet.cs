using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen2
{
    class WordSet : IWordSet
    {
        private ImmutableDictionary<string, IndexRecord> dictionary;
        private ImmutableList<IndexRecord> list;
        private string partOfSpeech;
        private Random random = new Random();
        


        public WordSet(string partOfSpeech)
        {
            this.partOfSpeech = partOfSpeech;
        }

        public string IndexFilePath
        {
            get { return "index." + partOfSpeech; }
        }

        public string DataFilePath
        {
            get { return "data." + partOfSpeech; }
        }

        public string PartOfSpeech
        {
            get { return partOfSpeech; }
        }

        public void LoadWords()
        {
            var indexDictionary = loadIndex();
            dictionary = indexDictionary;
            list = ImmutableList.Create<IndexRecord>(dictionary.Values.ToArray<IndexRecord>());

            var dataDictionary = loadData();
        }

        private ImmutableDictionary<string, IndexRecord> loadIndex()
        {
            var dictionaryBuilder = ImmutableDictionary.CreateBuilder<string, IndexRecord>();

            using (var input = new StreamReader("./dict/" + IndexFilePath))
            {
                while (!input.EndOfStream && input.ReadLine().StartsWith("  ")) { }                

                while (!input.EndOfStream)
                {                    
                    var currentRecord = IndexRecord.Parse(input.ReadLine());
                    dictionaryBuilder.Add(currentRecord.Lemma, currentRecord);
                }                

                input.Close();
            }

            return dictionaryBuilder.ToImmutableDictionary<string, IndexRecord>();           
        }

        private ImmutableDictionary<long, DataRecord> loadData()
        {
            var dictionaryBuilder = ImmutableDictionary.CreateBuilder<long, DataRecord>();

            using (var input = new StreamReader("./dict/" + DataFilePath))
            {
                while (!input.EndOfStream && input.ReadLine().StartsWith("  ")) { }

                while (!input.EndOfStream)
                {                    
                    var currentRecord = DataRecord.Parse(input.ReadLine(), PartOfSpeech == "verb");   

                    dictionaryBuilder.Add(currentRecord.SynsetOffset, currentRecord);
                }

                input.Close();
            }

            return dictionaryBuilder.ToImmutableDictionary<long, DataRecord>();   
        }


        public Word GetRandomWord()
        {
            IndexRecord indexRecord = list[random.Next(list.Count)];

            return new Word(indexRecord.Lemma, partOfSpeech);
        }
    }
}
