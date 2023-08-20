using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordGenderApp
{
    [RequireComponent(typeof(Image))]
    public class ColoredBackground : MonoBehaviour
    {
        [SerializeField]
        private Image _imgBackground;

        private void Reset()
        {
            _imgBackground = GetComponent<Image>();
        }

        public void SetColor(Color color)
        {
            _imgBackground.color = color;
        }

        public IEnumerator AnimateChangeColor(Color endColor, float duration = 0.1f)
        {
            Color startCol = _imgBackground.color;
            float timer = 0f;
            while (timer < duration)
            {
                yield return null;
                _imgBackground.color = Color.Lerp(startCol, endColor, timer / duration);
            }
            _imgBackground.color = endColor;
        }

    }
}
