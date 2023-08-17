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
        private CanvasGroup _coloredBackground;

        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _coloredBackground = WordCardManager.Instance.ColoredBackgroundDict[_swipeDirection];
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
            }
            else
            {
                a = 0;
            }

            _coloredBackground.alpha = a;
            _cg.alpha = a;
        }
    }
}
