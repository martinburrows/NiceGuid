using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NiceGuid.Generator
{
    public class GuidGenerator
    {
        private readonly ConcurrentDictionary<int, List<string>> _wordsByLength;
        private readonly Random _random;

        public GuidGenerator(ConcurrentDictionary<int, List<string>> wordsByLength)
        {
            _wordsByLength = wordsByLength;
            _random = new Random();
        }

        public string GetNiceGuid()
        {
            var segments = new[]
                           {
                               GetSegment(8),
                               GetSegment(4),
                               GetSegment(4),
                               GetSegment(4),
                               GetSegment(12)
                           };

            return string.Join("-", segments);
        }

        string GetSegment(int segmentLength)
        {
            var wordLengths = GetSegmentWordLengths(segmentLength);

            return wordLengths.Aggregate(string.Empty, (current, length) => current + GetRandomWord(length));
        }

        private IEnumerable<int> GetSegmentWordLengths(int totalLength)
        {
            var remainingLength = totalLength;

            while (remainingLength > 0)
            {
                var possibleLengths = GetRemainingPossibleWordLengths(remainingLength).ToList();
                var selected = possibleLengths.Count == 1 ? possibleLengths[0] : possibleLengths[_random.Next(possibleLengths.Count)];

                yield return selected;

                remainingLength -= selected;
            }
        }

        private static IEnumerable<int> GetRemainingPossibleWordLengths(int remainingLength)
        {
            const int minWordLength = 4;
            const int maxWordLength = 8;
            const int minNextWordLength = minWordLength*2;

            if (remainingLength <= maxWordLength)
                yield return remainingLength;

            if (remainingLength == minWordLength || remainingLength < minNextWordLength)
                yield break;

            yield return minWordLength;

            var maxVariable = remainingLength - minNextWordLength;

            if (maxVariable <= 0)
                yield break;

            foreach (var i in Enumerable.Range(minWordLength + 1, maxVariable))
                yield return i;
        }

        private string GetRandomWord(int length)
        {
            return _wordsByLength[length][_random.Next(_wordsByLength[length].Count)];
        }
    }
}
