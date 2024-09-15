using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WordGenderApp
{
    public class WordListImporter : Singleton<WordListImporter>
    {
        private string _loadPath => Path.Combine(Application.dataPath, "Data/WordList.txt");

        [ContextMenu("Load Words From Text File To Database")]
        public void LoadWordsToDatabase()
        {
            var entriesFromList = LoadWordsFromFile(_loadPath);
            WordBankManager.Instance.AddWordEntries(entriesFromList);
        }

        private List<WordEntry> LoadWordsFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] content = File.ReadAllLines(filePath);
                var words = new List<WordEntry>();
                foreach (var line in content)
                {
                    string[] strs = line.Split(' ');
                    if (strs.Length == 2)
                        words.Add(new WordEntry(word: strs[1], gender: strs[0]));
                }
                return words;
            }
            else
            {
                Debug.LogError($"The file at the given path does not exist: {filePath}");
                return new List<WordEntry>();
            }
        }
    }
}