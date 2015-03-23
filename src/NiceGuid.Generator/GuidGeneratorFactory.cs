using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NiceGuid.Generator
{
    public class GuidGeneratorFactory : IGuidGeneratorFactory
    {
        private readonly ConcurrentDictionary<int, List<string>> _wordsByLength;

        public GuidGeneratorFactory(string wordsFile)
        {
            var allWords = File.ReadAllLines(wordsFile);

            _wordsByLength = new ConcurrentDictionary<int, List<string>>(Enumerable.Range(4, 5).ToDictionary(length => length, _ => new List<string>()));

            foreach (var word in allWords)
                _wordsByLength[word.Length].Add(word);
        }

        public GuidGenerator GetGuidGenerator()
        {
            return new GuidGenerator(_wordsByLength);
        }
    }

    public interface IGuidGeneratorFactory
    {
        GuidGenerator GetGuidGenerator();
    }
}