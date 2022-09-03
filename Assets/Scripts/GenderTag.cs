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
        private Datatypes.SwipeArea _swipeDirection;
        [SerializeField]
        private Transform _referenceLine;
        [SerializeField]
        private WordCard _wordCard;

        private Vector3 _defaultWordCardPos;
        private CanvasGroup _cg;
        private CanvasGroup _coloredBackground;

        private void Awake()
        {
            if (!_wordCard) _wordCard = GetComponentInParent<WordCard>();
            _defaultWordCardPos = _wordCard.transform.position;
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
            if (_wordCard.CurrentArea == _swipeDirection)
            {
                a = WordCardManager.Instance.DetermineGenderTagAlpha(_wordCard.transform.position);
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
