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
            float x_right = _rightThreshold.position.x;
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

        public float DetermineGenderTagAlpha(Vector2 pos)
        {
            float x = pos.x;
            float y = pos.y;
            float w = Screen.width;
            float h = Screen.height;

            float x_left = _leftThreshold.position.x;
            float x_right = _rightThreshold.position.x;
            float y_bottom = _bottomThreshold.position.y;
            float y_top = _topThreshold.position.y;

            Vector3 p1;
            Vector3 p2;
            Vector3 p3;

            float a;
            if (x < x_left)
            {
                if (y < y_bottom)
                {
                    p1 = new(0, 0, 0);
                    p2 = new(0, y_bottom, 1);
                    p3 = new(x_left, y_bottom, 0);
                    a = Mathf.Abs(GetIntersectionHeightAsAlpha(p1, p2, p3, pos));
                }
                else if (y < y_top)
                {
                    a = Mathf.InverseLerp(x_left, 0, x);
                }
                else
                {
                    p1 = new(0, y_top, 1);
                    p2 = new(0, h, 0);
                    p3 = new(x_left, y_top, 0);
                    a = Mathf.Abs(GetIntersectionHeightAsAlpha(p1, p2, p3, pos));
                }
            }
            else if (x < x_right)
            {
                if (y < y_bottom)
                {
                    a = Mathf.InverseLerp(y_bottom, 0, y);
                }
                else if (y < y_top)
                {
                    a = 0;
                }
                else
                {
                    a = Mathf.InverseLerp(y_top, h, y);
                }
            }
            else
            {
                if (y < y_bottom)
                {
                    p1 = new(x_right, 0, 1);
                    p2 = new(x_right, y_bottom, 0);
                    p3 = new(w, y_bottom, 0);
                    a = Mathf.Abs(GetIntersectionHeightAsAlpha(p1, p2, p3, pos));
                }
                else if (y < y_top)
                {
                    a = Mathf.InverseLerp(x_right, w, x);
                }
                else
                {
                    p1 = new(x_right, y_top, 0);
                    p2 = new(w, h, 0);
                    p3 = new(w, y_top, 1);
                    a = Mathf.Abs(GetIntersectionHeightAsAlpha(p1, p2, p3, pos));
                }
            }

            return Mathf.Clamp(a * 2, 0f, 1f);
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