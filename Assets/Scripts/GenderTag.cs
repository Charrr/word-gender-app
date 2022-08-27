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
            if (!_coloredBackground)
            {
                Debug.Log("No on Start", this);
            }
        }

        private void Update()
        {
            float a = GetLerpValue();

            if (_coloredBackground)
                _coloredBackground.alpha = a;
            _cg.alpha = a;
        }

        private float GetLerpValue()
        {
            float t = 0f;
            switch (_swipeDirection)
            {
                case Datatypes.SwipeArea.Left:
                case Datatypes.SwipeArea.Right:
                    float x_start = _defaultWordCardPos.x;
                    float x_end = _referenceLine.position.x;
                    t = Mathf.InverseLerp(x_start, x_end, _wordCard.transform.position.x);
                    break;
                case Datatypes.SwipeArea.Top:
                case Datatypes.SwipeArea.Bottom:
                    float y_start = _defaultWordCardPos.y;
                    float y_end = _referenceLine.position.y;
                    t = Mathf.InverseLerp(y_start, y_end, _wordCard.transform.position.y);
                    break;
            }
            // Scale the factor non-linearly to avoid tag showing too early.
            return Mathf.Clamp(2 * t - 1f, 0f, 1f);
        }
    }
}
