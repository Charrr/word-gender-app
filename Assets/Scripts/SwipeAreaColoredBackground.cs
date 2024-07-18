using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordGenderApp
{
    public class SwipeAreaColoredBackground : ColoredBackground
    {
        [SerializeField]
        private Color _leftColor, _rightColor, _topColor, _bottomColor;
        private Dictionary<SwipeArea, Color> _swipeColorMap;

        private void Awake()
        {
            _swipeColorMap = new Dictionary<SwipeArea, Color>() {
                { SwipeArea.Bottom, _bottomColor },
                { SwipeArea.Left, _leftColor },
                { SwipeArea.Right, _rightColor },
                { SwipeArea.Top, _topColor },
            };
        }

        public void SetColorPerSwipeArea(SwipeArea targetArea, float alpha = 1f)
        {
            SetColor(_swipeColorMap[targetArea]);
            SetAlpha(alpha);
        }
    }
}