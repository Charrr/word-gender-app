using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordGenderApp
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GenderTag : MonoBehaviour
    {
        [SerializeField]
        private SwipeArea _swipeDirection;
        private WordCard _wordCard;
        public WordCard WordCard
        {
            get
            {
                if (!_wordCard)
                    _wordCard = GetComponentInParent<WordCard>();
                return _wordCard;
            }
        }

        private CanvasGroup _cg;

        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            UpdateGenderColors();
        }

        private void UpdateGenderColors()
        {
            float a;
            if (WordCard.CurrentArea == _swipeDirection)
            {
                a = WordCardManager.Instance.DetermineGenderTagAlpha(WordCard.transform.position);
                BackgroundColorController.Instance.SetColorPerSwipeArea(_swipeDirection, a);
            }
            else
            {
                a = 0;
            }

            _cg.alpha = a;
        }
    }
}
