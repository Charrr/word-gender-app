using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WordGenderApp
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CorrectFeedback : Singleton<CorrectFeedback>
    {
        [SerializeField] private TMP_Text _txtCorrectWord;

        private CanvasGroup _cg;

        protected override void Awake()
        {
            base.Awake();
            _cg = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _cg.alpha = 0f;
        }

        public void DisplayCorrectWord(WordData word)
        {
            StartCoroutine(AnimateDisplayCorrectWord(word));
        }

        private IEnumerator AnimateDisplayCorrectWord(WordData word)
        {
            _cg.alpha = 1f;
            _txtCorrectWord.text = word.ToString();
            yield return new WaitForSeconds(1f);
            _cg.alpha = 0f;
        }
    } 
}
