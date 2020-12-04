using ModuleHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ModuleHelper.Utils
{
    public static class MusicalMathHelper
    {
        public static double CalculateFrequency(int n, int startOctave)
        {
            var freq = 440.0 * Math.Pow(2.0, (n - 49.0) / 12.0);

            return freq * startOctave;
        }

        public static int Modulo(int x, int m)
        {
            return (x % m + m) % m;
        }

        public static bool CheckIfKeyIsInScale(bool isUsingScales, int keyNumber, IEnumerable<Note> musicalScaleNotes)
        {
            if (!isUsingScales)
            {
                return true;
            }
            else
            {
                return musicalScaleNotes.Any(note => Modulo(keyNumber, 12) == (int)note);
            }
        }

        public static List<int> CalculateKeyDifferences(List<int> keys)
        {
            keys.Sort();

            List<int> keyDifferences = new List<int>();

            for (int i = 0; i < keys.Count(); i++)
            {
                var difference = keys[i] - keys[0];

                keyDifferences.Add(difference);
            }
            return keyDifferences;
        }
    }
}