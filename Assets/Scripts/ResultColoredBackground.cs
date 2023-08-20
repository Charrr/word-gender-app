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
    }
}