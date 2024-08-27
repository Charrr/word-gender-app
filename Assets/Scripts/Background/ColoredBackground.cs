using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordGenderApp
{
    [RequireComponent(typeof(Image), typeof(CanvasGroup))]
    public class ColoredBackground : MonoBehaviour
    {
        [SerializeField]
        protected Image m_ImgBackground;
        [SerializeField]
        protected CanvasGroup m_CanvasGroup;

        private void Reset()
        {
            m_ImgBackground = GetComponent<Image>();
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetColor(Color color)
        {
            m_ImgBackground.color = color;
        }

        public void SetAlpha(float alpha)
        {
            m_CanvasGroup.alpha = alpha;
        }

        public void FadeOut(float duration = 0.1f)
        {
            StartCoroutine(AnimateChangeAlpha(0f, duration));
        }

        public IEnumerator AnimateChangeColor(Color endColor, float duration = 0.1f)
        {
            Color startCol = m_ImgBackground.color;
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                m_ImgBackground.color = Color.Lerp(startCol, endColor, timer / duration);
                yield return null;
            }
            m_ImgBackground.color = endColor;
        }

        public IEnumerator AnimateChangeAlpha(float endAlpha, float duration = 0.1f)
        {
            float startAlpha = m_CanvasGroup.alpha;
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                m_CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
                yield return null;
            }
            m_CanvasGroup.alpha = endAlpha;
        }
    }
}
