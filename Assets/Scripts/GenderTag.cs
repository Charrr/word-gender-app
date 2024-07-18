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

        private CanvasGroup _cg;

        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
            _cg.alpha = 0f;
        }

        public void UpdateAlpha(SwipeArea targetArea, float targetAlpha)
        {
            if (targetArea == _swipeDirection)
            {
                _cg.alpha = targetAlpha;
            }
            else
            {
                _cg.alpha = 0f;
            }
        }
    }
}
