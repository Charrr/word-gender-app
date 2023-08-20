using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class BackgroundColorController : Singleton<BackgroundColorController>
    {
        [SerializeField]
        private Color _leftColor, _rightColor, _topColor, _bottomColor;
        [SerializeField]
        private Color _defaultColor, _correctColor, _incorrectColor;
        private Dictionary<SwipeArea, Color> _swipeColorMap;

        [SerializeField]
        private ColoredBackground _background;

        protected override void Awake()
        {
            base.Awake();
            if (!_background) _background = FindObjectOfType<ColoredBackground>();
            _swipeColorMap = new Dictionary<SwipeArea, Color>() {
                { SwipeArea.Bottom, _bottomColor },
                { SwipeArea.Left, _leftColor },
                { SwipeArea.Right, _rightColor },
                { SwipeArea.Top, _topColor },
            };
        }

        private void Start()
        {
            ResetColor();
        }

        public void SetColorPerSwipeArea(SwipeArea targetArea, float lerp = 1f)
        {
            _background.SetColor(Color.Lerp(_defaultColor, _swipeColorMap[targetArea], lerp));
        }

        [ContextMenu("Reset Color")]
        public void ResetColor()
        {
            _background.SetColor(_defaultColor);
        }
    }
}
