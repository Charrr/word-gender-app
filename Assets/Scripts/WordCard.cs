using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace WordGenderApp
{
    public class WordCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Custom Parameters")]
        [SerializeField]
        [Range(1f, 300f)]
        private float _swipeAwaySpeed = 120f;
        [SerializeField]
        [Range(0.01f, 0.2f)]
        private float _returnToCenterDuration = 0.05f;
        [SerializeField]
        [Range(0f, 0.2f)]
        private float _rotateRate = 0.03f;

        [Header("Object References")]
        [SerializeField]
        private TMP_Text _wordText;

        private WordCardManager _manager;
        private Vector3 _defaultPosition;
        private Vector2 _delta;
        private bool _fingerDownOnUpperPart;

        private WordData _wordData;
        public WordData WordData
        {
            get => _wordData;
            set
            {
                _wordData = value;
                _wordText.text = value.Word;
            }
        }
        public SwipeArea CurrentArea => _manager.DetermineSwipeArea(transform.position);

        private void OnValidate()
        {
            if (!_wordText) _wordText = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _manager = WordCardManager.Instance;
            _defaultPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _delta = eventData.delta;

            transform.Translate(_delta);
            float horizDiviation = transform.position.x - _defaultPosition.x;
            float newAngle = horizDiviation * _rotateRate * (_fingerDownOnUpperPart ? 1f : -1f);
            transform.localEulerAngles = new Vector3(0f, 0f, newAngle);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var fingerPos = eventData.position;
            _fingerDownOnUpperPart = fingerPos.y < _defaultPosition.y;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_manager.DetermineGenderTagAlpha(transform.position) < 0.5f)
            {
                Debug.Log("(stays)");
                StartCoroutine(AnimateReturnToCenter());
                return;
            }

            var area = _manager.DetermineSwipeArea(transform.position);
            switch (area)
            {
                case SwipeArea.Left:
                    Debug.Log("Der");
                    StartCoroutine(AnimateSwipingCardAway());
                    break;
                case SwipeArea.Right:
                    Debug.Log("Die");
                    StartCoroutine(AnimateSwipingCardAway());
                    break;
                case SwipeArea.Top:
                    Debug.Log("Das");
                    StartCoroutine(AnimateSwipingCardAway());
                    break;
                case SwipeArea.Bottom:
                    Debug.Log("Idk?");
                    StartCoroutine(AnimateSwipingCardAway());
                    break;
            }
        }

        private IEnumerator AnimateSwipingCardAway()
        {
            float animDuration = 1f;
            Vector3 endPos = transform.position;
            Vector3 shift = endPos - _defaultPosition;

            float timer = 0f;
            while (timer < animDuration)
            {
                transform.Translate(shift * Time.deltaTime * _swipeAwaySpeed);
                yield return null;
                timer += Time.deltaTime;
            }

            Destroy(gameObject);
        }

        private IEnumerator AnimateReturnToCenter()
        {
            float animDuration = _returnToCenterDuration;
            Vector3 startPos = transform.position;
            Vector3 endPos = _defaultPosition;

            Quaternion startRot = transform.localRotation;
            Quaternion endRot = Quaternion.identity;

            float timer = 0f;
            while (timer < animDuration)
            {
                float t = timer / animDuration;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                transform.localRotation = Quaternion.Lerp(startRot, endRot, t);
                yield return null;
                timer += Time.deltaTime;
            }
            transform.position = _defaultPosition;
            transform.localRotation = Quaternion.identity;
        }
    }
}
