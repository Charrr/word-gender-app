using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    [CreateAssetMenu]
    public class WordBank : ScriptableObject
    {
        public List<WordEntry> WordEntries = new();
    }
}
