using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace WordGenderApp
{
    public class WordCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private TMP_Text _wordText;
        [SerializeField]
        private Transform _thresholdReferences;
        private Transform _leftThreshold;
        private Transform _rightthreshold;
        private Transform _topThreshold;
        private Transform _bottomThreshold;

        private Vector3 _defaultPosition;
        private Vector2 _delta;

        public Datatypes.WordData WordData { get; private set; }
        public string Word => WordData.Word;

        private void OnValidate()
        {
            if (!_wordText) _wordText = GetComponentInChildren<TMP_Text>();
            if (!_thresholdReferences) _thresholdReferences = GameObject.Find("Canvas/Threshold References").transform;
        }

        private void Awake()
        {
            _leftThreshold = _thresholdReferences.Find("Left Threshold");
            _rightthreshold = _thresholdReferences.Find("Right Threshold");
            _bottomThreshold = _thresholdReferences.Find("Bottom Threshold");
            _topThreshold = _thresholdReferences.Find("Top Threshold");
        }

        private void Start()
        {
            _defaultPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _delta = eventData.delta;
            transform.Translate(_delta);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var area = DetermineSwipeArea(transform.position);
            switch (area)
            {
                case Datatypes.SwipeArea.Left:
                    Debug.Log("Der");
                    break;
                case Datatypes.SwipeArea.Right:
                    Debug.Log("Die");
                    break;
                case Datatypes.SwipeArea.Top:
                    Debug.Log("Das");
                    break;
                case Datatypes.SwipeArea.Bottom:
                    Debug.Log("Idk?");
                    break;
            }

            transform.position = _defaultPosition;
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
