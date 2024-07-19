using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    [Serializable]
    public class WordEntry
    {
        [SerializeField] private Guid _id;
        [SerializeField] private WordData _word;
        [SerializeField] private int _occurrence;
        [SerializeField] private int _correctCount, _incorrectCount;
        [SerializeField] private bool _isFavorite, _shouldSkip;

        public Guid Id { get => _id; set => _id = value; }
        public WordData Word { get => _word; set => _word = value; }
        public int Occurrence { get => _occurrence; set => _occurrence = value; }
        public int CorrectCount { get => _correctCount; set => _correctCount = value; }
        public int IncorrectCount { get => _incorrectCount; set => _incorrectCount = value; }
        public bool IsFavorite { get => _isFavorite; set => _isFavorite = value; }
        public bool ShouldSkip { get => _shouldSkip; set => _shouldSkip = value; }

        public bool IsNew => _occurrence == 0;
        public float CorrectRate => IsNew ? 0 : (float)_correctCount / _occurrence;
        public float IncorrectRate => IsNew ? 0 : (float)_incorrectCount / _occurrence;

        public void ResetCounts()
        {
            _occurrence = 0;
            _correctCount = 0;
            _incorrectCount = 0;
        }
    }
}

