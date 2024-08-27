using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class ResultColoredBackground : ColoredBackground
    {
        [SerializeField]
        private Color _correctColor, _incorrectColor;

        private void Start()
        {
            SetAlpha(0f);
        }

        public void DipToCorrectColor(float waitDuration = 0.5f, float fadeDuration = 0.1f)
        {
            StartCoroutine(AnimateDipToColorAndBack(_correctColor, waitDuration, fadeDuration));
        }

        public void DipToIncorrectColor(float waitDuration = 0.5f, float fadeDuration = 0.1f)
        {
            StartCoroutine(AnimateDipToColorAndBack(_incorrectColor, waitDuration, fadeDuration));
        }

        private IEnumerator AnimateDipToColorAndBack(Color targetColor, float waitDuration = 0.5f, float fadeDuration = 0.1f)
        {
            SetColor(targetColor);
            yield return StartCoroutine(AnimateChangeAlpha(1f, fadeDuration));
            yield return new WaitForSeconds(waitDuration);
            yield return StartCoroutine(AnimateChangeAlpha(0f, fadeDuration));
        }
    }
}