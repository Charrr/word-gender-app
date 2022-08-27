using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class WordCardManager : Singleton<WordCardManager>
    {
        [Header("Colored Background References")]
        [SerializeField]
        private CanvasGroup _leftColoredBackground;
        [SerializeField]
        private CanvasGroup _rightColoredBackground;
        [SerializeField]
        private CanvasGroup _topColoredBackground;
        [SerializeField]
        private CanvasGroup _bottomColoredBackground;

        [Header("Swipe Threshold References")]
        [SerializeField]
        private Transform _leftThreshold;
        [SerializeField]
        private Transform _rightthreshold;
        [SerializeField]
        private Transform _topThreshold;
        [SerializeField]
        private Transform _bottomThreshold;

        public Dictionary<Datatypes.SwipeArea, CanvasGroup> ColoredBackgroundDict;

        protected override void Awake()
        {
            base.Awake();

            SetUpColorBackgrounds();
        }

        private void SetUpColorBackgrounds()
        {
            ColoredBackgroundDict = new()
            {
                { Datatypes.SwipeArea.Left, _leftColoredBackground },
                { Datatypes.SwipeArea.Right, _rightColoredBackground },
                { Datatypes.SwipeArea.Top, _topColoredBackground },
                { Datatypes.SwipeArea.Bottom, _bottomColoredBackground }
            };
        }

        // --------------------
        // |\                /|
        // | \      T       / |
        // |  \            /  |
        // |   \          /   |
        // |    ----------    |
        // |    |        |    |                  
        // | L  |   C    |  R |
        // |    |        |    |
        // |    ----------    |
        // |   /          \   |
        // |  /            \  |
        // | /      B       \ |
        // |/                \|
        // --------------------
        public Datatypes.SwipeArea DetermineSwipeArea(Vector2 point)
        {
            float x = point.x;
            float y = point.y;
            float w = Screen.width;
            float h = Screen.height;

            float x_left = _leftThreshold.position.x;
            float x_right = _rightthreshold.position.x;
            float y_bottom = _bottomThreshold.position.y;
            float y_top = _topThreshold.position.y;

            if (x < x_left)
            {
                if (y < Mathf.Lerp(0f, y_bottom, x / x_left))
                {
                    return Datatypes.SwipeArea.Bottom;
                }
                else if (y < Mathf.Lerp(h, y_top, x / x_left))
                {
                    return Datatypes.SwipeArea.Left;
                }
                else
                {
                    return Datatypes.SwipeArea.Top;
                }
            }
            else if (x < x_right)
            {
                if (y < y_bottom)
                {
                    return Datatypes.SwipeArea.Bottom;
                }
                else if (y < y_top)
                {
                    return Datatypes.SwipeArea.Center;
                }
                else
                {
                    return Datatypes.SwipeArea.Top;
                }
            }
            else
            {
                if (y < Mathf.Lerp(0, y_bottom, (w - x) / (w - x_right)))
                {
                    return Datatypes.SwipeArea.Bottom;
                }
                else if (y < Mathf.Lerp(h, y_top, (w - x) / (w - x_right)))
                {
                    return Datatypes.SwipeArea.Right;
                }
                else
                {
                    return Datatypes.SwipeArea.Top;
                }
            }
        }
    }
}