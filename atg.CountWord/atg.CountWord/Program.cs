using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atg.CountWord
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "fileInput.arff";
            string outputFile = "fileOutput.arff";
            ProcessFile(inputFile, outputFile, true);
        }

        private static void ProcessFile(string inputFile, string outputFile, bool firstSentenceEnglish)
        {
            if(!File.Exists(inputFile))
            {
                Console.WriteLine("El fichero no existe");
                return;
            }

            File.WriteAllText(outputFile, GetHeaderOutputFile());

            var lines = File.ReadAllLines(inputFile).ToList();

            bool english = firstSentenceEnglish;
            foreach(var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var words = GetWords(line);

                var avgLetters = GetAvgLetters(words);
                var numVowels = GetNumVowels(line);
                var numCharactersLongestWord = GetNumCharactersInLongestWord(words);

                File.AppendAllText(outputFile,Environment.NewLine);
                File.AppendAllText(outputFile, avgLetters.ToString().Replace(",",".") + "," + numVowels + "," + numCharactersLongestWord + "," + (english?"1":"0"));
                english = !english;
            }
        }

        private static string GetHeaderOutputFile()
        {
            string cabecera = "@RELATION language_sentences" + Environment.NewLine +
                              "@ATTRIBUTE avg_characters  REAL" + Environment.NewLine +
                              "@ATTRIBUTE num_vowels numeric" + Environment.NewLine +
                              "@ATTRIBUTE num_characters_longest_word  numeric" + Environment.NewLine +
                              "@ATTRIBUTE english {1, 0}" + Environment.NewLine +
                              Environment.NewLine +
                              Environment.NewLine +
                              "@DATA";

            return cabecera;
        }

        private static double GetAvgLetters(List<string> words)
        {            
            var avg = words.Average(w => w.Length);
            return Math.Round(avg,2);    
        }

        private static int GetNumVowels(string line)
        {
            //var vowels = "aeiou";
            var vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
            var numVowels = line.Count(c=> vowels.Contains(Char.ToLower(c)));
            return numVowels;
        }

        private static int GetNumCharactersInLongestWord(List<string> words)
        {            
            var longestWord = words.Max(w => w.Length);
            return longestWord;
        }

        private static List<string> GetWords(string line)
        {
            var separators = new char[] { ' ', ',', ';', '?', '.', '!' };
            return line.Split(separators).ToList();
        }
    }
}
