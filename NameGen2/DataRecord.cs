using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen2
{
    class DataRecord
    {
        private DataRecord() { }

        public long SynsetOffset { get; private set; }
        public int LexicalFileNumber { get; private set; }
        public char PartOfSpeech { get; private set; }
        
        public int SynsetWordCount { get; private set; }
        private List<LexicalWord> words;
        public IEnumerable<LexicalWord> Words
        {
            get
            {
                return words;
            }
        }


        public string Word { get; private set; }
        public int LexicalId { get; private set; }

        public int PointerCount { get; private set; }
        private List<Pointer> pointers;
        public IEnumerable<Pointer> Pointers
        {
            get
            {
                return pointers;
            }
        }

        public int FrameCount { get; private set; }
        private List<Frame> frames;
        public IEnumerable<Frame> Frames
        {
            get
            {
                return frames;
            }
        }

        public string Gloss { get; private set; }

        private static Queue<string> tokenizeRecord(string record)
        {
            string[] tokens = record.Trim().Split(new char[] { ' ' });

            return new Queue<string>(tokens);
        }

        private static Queue<string> parseSynsets(Queue<string> tokens, DataRecord dataRecord)
        {
            dataRecord.SynsetWordCount = Int32.Parse(tokens.Dequeue(), System.Globalization.NumberStyles.AllowHexSpecifier);
            dataRecord.words = new List<LexicalWord>();

            for (int j = 0; j < dataRecord.SynsetWordCount; j++)
            {
                string wordText = tokens.Dequeue();
                int id = Int32.Parse(tokens.Dequeue(), System.Globalization.NumberStyles.AllowHexSpecifier);
                
                LexicalWord lexicalWord = new LexicalWord(wordText, id);
                
                dataRecord.words.Add(lexicalWord);
            }

            return tokens;
        }

        private static Queue<string> parsePointers(Queue<string> tokens, DataRecord dataRecord)
        {
            dataRecord.PointerCount = Int32.Parse(tokens.Dequeue());
            dataRecord.pointers = new List<Pointer>();

            for (int j = 0; j < dataRecord.PointerCount; j++)
            {
                dataRecord.pointers.Add(Pointer.Parse(tokens));
            }

            return tokens;
        }

        private static Queue<string> parseFrames(Queue<string> tokens, DataRecord dataRecord)
        {
            dataRecord.FrameCount = Int32.Parse(tokens.Dequeue());

            for (var j = 0; j < dataRecord.FrameCount; j++)
            {
                string divider = tokens.Dequeue();
                var frameNumber = Int32.Parse(tokens.Dequeue());
                var wordIndex = Int32.Parse(tokens.Dequeue(), System.Globalization.NumberStyles.AllowHexSpecifier);

                Frame frame = new Frame(frameNumber, wordIndex);
                
                dataRecord.frames.Add(frame);
            }

            return tokens;
        }

        private static Queue<string> parseGloss(Queue<string> tokens, DataRecord dataRecord)
        {
            dataRecord.Gloss = "";

            tokens.Dequeue();

            while (tokens.Count > 0)
            {
                dataRecord.Gloss += tokens.Dequeue() + " ";
            }

            dataRecord.Gloss = dataRecord.Gloss.Trim();

            return tokens;
        }

        public static DataRecord Parse(string record, bool hasFrames)
        {
            DataRecord dataRecord = new DataRecord();
            Queue<string> tokens = tokenizeRecord(record);

            dataRecord.SynsetOffset = Int64.Parse(tokens.Dequeue());
            dataRecord.LexicalFileNumber = Int32.Parse(tokens.Dequeue());
            dataRecord.PartOfSpeech = tokens.Dequeue()[0];

            parseSynsets(tokens, dataRecord);
            parsePointers(tokens, dataRecord);           

            dataRecord.frames = new List<Frame>();
            if (hasFrames)
            {
                parseFrames(tokens, dataRecord);
            }

            parseGloss(tokens, dataRecord);

            return dataRecord;
        }


        public class Frame
        {
            public int FrameNumber { get; private set; }
            public int WordIndex { get; private set; }

            public Frame(int frameNumber, int wordIndex)
            {
                FrameNumber = frameNumber;
                WordIndex = wordIndex;
            }
        }

        public class LexicalWord
        {
            public int LexicalId { get; private set; }
            public string Word { get; private set; }

            public LexicalWord(string word, int lexicalId)
            {
                Word = word;
                LexicalId = lexicalId;
            }
        }

        public class Pointer
        {
            private Pointer() { }

            public char PointerSymbol { get; private set; }
            public long SynsetOffset { get; private set; }
            public char PartOfSpeech { get; private set; }
            public int Source { get; private set; }
            public int Target { get; private set; }


            public static Pointer Parse(Queue<string> parsedRecord)
            {
                Pointer result = new Pointer();
                result.PointerSymbol = parsedRecord.Dequeue()[0];
                result.SynsetOffset = Int64.Parse(parsedRecord.Dequeue());
                result.PartOfSpeech = parsedRecord.Dequeue()[0];
                
                string sourceTarget = parsedRecord.Dequeue();
                result.Source = Int32.Parse(sourceTarget.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                result.Target = Int32.Parse(sourceTarget.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

                return result;
            }

            public override string ToString()
            {
                return string.Format("({0}){1}[{2}] {3} -> {4}", PartOfSpeech, PointerSymbol, SynsetOffset, Source, Target);

            }

        }

        
    }
}
