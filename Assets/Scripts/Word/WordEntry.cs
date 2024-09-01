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
        [SerializeField] private string _word;
        [SerializeField] private int _genderCode;
        [SerializeField] private int _occurrence;
        [SerializeField] private int _correctCount, _incorrectCount;
        [SerializeField] private bool _isFavorite, _shouldSkip;

        public Guid Id { get => _id; set => _id = value; }
        public string Word { get => _word; set => _word = value; }
        /// <summary>
        /// A 3-digit byte-code representaion of the gender.
        /// 000 - 0 - undefined
        /// 001 - 1 - das
        /// 010 - 2 - die
        /// 011 - 3 - die, das
        /// 100 - 4 - der
        /// 101 - 5 - der, das
        /// 110 - 6 - der, die
        /// 111 - 7 - der, die, das
        /// </summary>
        public int GenderCode { get => _genderCode; set => _genderCode = value; }
        public int Occurrence { get => _occurrence; set => _occurrence = value; }
        public int CorrectCount { get => _correctCount; set => _correctCount = value; }
        public int IncorrectCount { get => _incorrectCount; set => _incorrectCount = value; }
        public bool IsFavorite { get => _isFavorite; set => _isFavorite = value; }
        public bool ShouldSkip { get => _shouldSkip; set => _shouldSkip = value; }

        public bool IsNew => _occurrence == 0;
        public float CorrectRate => IsNew ? 0 : (float)_correctCount / _occurrence;
        public float IncorrectRate => IsNew ? 0 : (float)_incorrectCount / _occurrence;

        public WordEntry() { }

        public WordEntry(WordData wordData)
        {
            _id = new();
            _word = wordData.Word;
            _genderCode = wordData.GenderCode;
            _isFavorite = false;
            _shouldSkip = false;
            ResetCounts();
        }

        public void ResetCounts()
        {
            _occurrence = 0;
            _correctCount = 0;
            _incorrectCount = 0;
        }
    }
}

