using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class WordBankManager : Singleton<WordBankManager>
    {
        [SerializeField] private WordBank _wordBank;
        public List<WordEntry> Entries => _wordBank.WordEntries;

        public void AddWordEntry(WordEntry entry)
        {
            if (Entries.Find(x => x.Word == entry.Word) == null)
                Entries.Add(entry);
        }

        public void RemoveWordEntry(WordEntry entry)
        {
            if (Entries.Contains(entry))
                Entries.Remove(entry);
        }
    }
}