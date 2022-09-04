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
        private Transform _rightThreshold;
        [SerializeField]
        private Transform _topThreshold;
        [SerializeField]
        private Transform _bottomThreshold;

        [Header("Word Card References")]
        public WordCard CurrentWordCard;
        private Vector2 _wordCardDefaultPos;

        public Dictionary<Datatypes.SwipeArea, CanvasGroup> ColoredBackgroundDict;

        protected override void Awake()
        {
            base.Awake();

            SetUpColorBackgrounds();
            _wordCardDefaultPos = CurrentWordCard.transform.position;
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
        // | \              / |
        // |  \            /  |
        // |   \          /   |
        // |    \   T    /    |
        // |     \      /     |
        // |      \    /      |
        // |       \  /       |
        // |  L     \/     R  |  
        // |        /\        |
        // |      /    \      |
        // |    /        \    |
        // |  /     B      \  |
        // |/                \|
        // --------------------
        public Datatypes.SwipeArea DetermineSwipeArea(Vector2 point)
        {
            float x = point.x;
            float y = point.y;
            float w = Screen.width;
            float h = Screen.height;

            float x0 = _wordCardDefaultPos.x;
            float y0 = _wordCardDefaultPos.y;

            if (x < x0)
            {
                if (y < Mathf.Lerp(0f, y0, x / x0))
                {
                    return Datatypes.SwipeArea.Bottom;
                }
                else if (y < Mathf.Lerp(h, y0, x / x0))
                {
                    return Datatypes.SwipeArea.Left;
                }
                else
                {
                    return Datatypes.SwipeArea.Top;
                }
            }
            else
            {
                if (y < Mathf.Lerp(0, y0, (w - x) / (w - x0)))
                {
                    return Datatypes.SwipeArea.Bottom;
                }
                else if (y < Mathf.Lerp(h, y0, (w - x) / (w - x0)))
                {
                    return Datatypes.SwipeArea.Right;
                }
                else
                {
                    return Datatypes.SwipeArea.Top;
                }
            }
        }

        public bool IsWithinThresholdArea(Vector2 pos)
        {
            float x = pos.x;
            float y = pos.y;

            float x_left = _leftThreshold.position.x;
            float x_right = _rightThreshold.position.x;
            float y_bottom = _bottomThreshold.position.y;
            float y_top = _topThreshold.position.y;

            return x > x_left && x < x_right && y > y_bottom && y < y_top;
        }

        public float DetermineGenderTagAlpha(Vector2 pos)
        {
            float x = pos.x;
            float y = pos.y;
            float w = Screen.width;
            float h = Screen.height;

            float x0 = _wordCardDefaultPos.x;
            float y0 = _wordCardDefaultPos.y;

            Vector3 p1;
            Vector3 p2;
            Vector3 p3;

            if (x < x0)
            {
                if (y < y0)
                {
                    p1 = new(0, 0, 0);
                    p2 = new(0, y0, 1);
                    p3 = new(x0, y0, 0);
                }
                else
                {
                    p1 = new(0, y0, 1);
                    p2 = new(0, h, 0);
                    p3 = new(x0, y0, 0);
                }
            }
            else
            {
                if (y < y0)
                {
                    p1 = new(x0, 0, 1);
                    p2 = new(x0, y0, 0);
                    p3 = new(w, 0, 0);
                }
                else
                {
                    p1 = new(x0, y0, 0);
                    p2 = new(w, h, 0);
                    p3 = new(w, y0, 1);
                }
            }

            float a = Mathf.Abs(GetIntersectionHeightAsAlpha(p1, p2, p3, pos));
            return Mathf.Clamp(a * 3 - 0.2f, 0f, 1f);
        }

        public static float GetIntersectionHeightAsAlpha(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 pXY)
        {
            Plane plane = new(p1, p2, p3);
            Ray ray = new(origin: pXY, direction: Vector3.forward);
            plane.Raycast(ray, out float z);
            return z;
        } 
    }
}