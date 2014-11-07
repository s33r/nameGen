using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen2
{
    class IndexRecord
    {
        public string Lemma { get; private set; }
        public char PartOfSpeech { get; private set; }
        
        
        public int SenseCount { get; private set; }
        public int TagSenseCount { get; private set; }


        public int SynsetCount { get; private set; }
        private List<long> synsetOffsets;
        public List<long> SynsetOffsets
        {
            get
            {
                return synsetOffsets;
            }

        }


        public int PointerCount { get; private set; }
        private List<string> pointers;
        public IEnumerable<string> Pointers
        {
            get
            {
                return pointers;
            }
        }

        public static IndexRecord Parse(string record)
        {
            IndexRecord result = new IndexRecord();            
            Queue<string> parsedRecord = new Queue<string>(record.Split(new char[] { ' ' }));

            result.Lemma = parsedRecord.Dequeue();
            result.PartOfSpeech = parsedRecord.Dequeue()[0];
            result.SynsetCount = Int32.Parse(parsedRecord.Dequeue());
            result.PointerCount = Int32.Parse(parsedRecord.Dequeue());            

            result.pointers = new List<string>();
            for (int j = 0; j < result.PointerCount; j++)
            {
                result.pointers.Add(parsedRecord.Dequeue());
            }

            result.SenseCount = Int32.Parse(parsedRecord.Dequeue());
            result.TagSenseCount = Int32.Parse(parsedRecord.Dequeue());

            result.synsetOffsets = new List<long>();
            for (int j = 0; j < result.SynsetCount; j++)
            {
                result.SynsetOffsets.Add(Int64.Parse(parsedRecord.Dequeue()));
            }            

            return result;
        }

    }
}
