using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WordGenderApp
{
    public class WordLoader
    {
#if UNITY_EDITOR
        private static string s_LoadPath = Path.Combine(Application.dataPath, "Data");
#else
        private static string s_LoadPath = Path.Combine(Application.persistentDataPath, "Data");
#endif
        public static List<WordData> LoadWords()
        {
            var fileName = "WordBank.txt";
            var filePath = Path.Combine(s_LoadPath, fileName);
            return LoadWordsFromFile(filePath);
        }

        private static List<WordData> LoadWordsFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var content = File.ReadAllLines(filePath);
                var words = new List<WordData>();
                foreach(var line in content)
                {
                    var data = ParseStringToWordData(line);
                    if (data.HasValue)
                    {
                        words.Add(data.Value);
                    }
                }
                return words;
            }
            else
            {
                Debug.LogError($"The file at the given path does not exist: {filePath}");
                return new List<WordData>();
            }
        }

        private static WordData? ParseStringToWordData(string content)
        {
            char sep = ' ';
            var temp = content.Split(sep);

            if (temp.Length == 2)
            {
                return new WordData(gender: temp[0], word: temp[1]);
            }
            else
            {
                Debug.LogError($"Cannot parse \"{content}\" to WordData!");
                return null;
            }
        }
    }
}
