﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NiceGuid.Generator
{
    public interface IGuidGenerator
    {
        List<Segment> GetNiceGuidSegments();
    }

    public class GuidGenerator : IGuidGenerator
    {
        private readonly ConcurrentDictionary<int, List<string>> _wordsByLength = new ConcurrentDictionary<int, List<string>>();
        private readonly Random _random = new Random();

        public GuidGenerator(string dictionaryFilePath)
        {
            var dictionary = File.ReadAllLines(dictionaryFilePath);

            foreach (var word in dictionary)
                _wordsByLength.GetOrAdd(word.Length, new List<string>()).Add(word);
        }

        public List<Segment> GetNiceGuidSegments()
        {
            var segments = new List<Segment>();

            segments.AddRange(GetSegments(8));
            segments.Add(Segment.Separator);
            segments.AddRange(GetSegments(4));
            segments.Add(Segment.Separator);
            segments.AddRange(GetSegments(4));
            segments.Add(Segment.Separator);
            segments.AddRange(GetSegments(4));
            segments.Add(Segment.Separator);
            segments.AddRange(GetSegments(12));

            return segments;
        }

        IEnumerable<Segment> GetSegments(int totalLength)
        {
            var wordLengths = GetSegmentWordLengths(totalLength);

            return wordLengths.Select(GenerateSegment);
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

        Segment GenerateSegment(int length)
        {
            return new Segment(GetRandomWord(length));
        }

        private string GetRandomWord(int length)
        {
            return _wordsByLength[length][_random.Next(_wordsByLength[length].Count)];
        }
    }
}
