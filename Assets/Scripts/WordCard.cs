using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace WordGenderApp
{
    public class WordCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Custom Parameters")]
        [SerializeField]
        [Range(1f, 300f)]
        private float _swipeAwaySpeed = 120f;
        [SerializeField]
        [Range(0.01f, 0.5f)]
        private float _returnToCenterOnEndDragDuration = 0.05f;
        [Range(0.01f, 0.5f)]
        private float _returnToCenterOnIncorrectDuration = 0.2f;
        [SerializeField]
        [Range(0f, 0.2f)]
        private float _rotateRate = 0.03f;

        [Header("Object References")]
        [SerializeField]
        private TMP_Text _wordText;
        [SerializeField]
        private GenderTag[] _genderTags;

        private WordCardManager _manager;
        private BackgroundManager _backgroundMngr;
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

        public event Action<Result> OnResult;

        private void Reset()
        {
            if (!_wordText) _wordText = GetComponentInChildren<TMP_Text>();
            _genderTags = GetComponentsInChildren<GenderTag>(true);
        }

        private void Start()
        {
            _manager = WordCardManager.Instance;
            _backgroundMngr = BackgroundManager.Instance;
            _defaultPosition = transform.position;
            OnResult += HandleResult;
        }

        private void OnDestroy()
        {
            OnResult = null;
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
            if (_manager.DetermineDecisionAlpha(transform.position) < 0.5f)
            {
                Debug.Log("(stays)");
                StartCoroutine(AnimateReturnToCenter(_returnToCenterOnEndDragDuration));
                return;
            }

            var area = _manager.DetermineSwipeArea(transform.position);

            switch (area)
            {
                case SwipeArea.Left:
                    Debug.Log("Der");
                    OnResult?.Invoke(GetResult(Gender.m));
                    break;
                case SwipeArea.Right:
                    Debug.Log("Die");
                    OnResult?.Invoke(GetResult(Gender.f));
                    break;
                case SwipeArea.Top:
                    Debug.Log("Das");
                    OnResult?.Invoke(GetResult(Gender.n));
                    break;
                case SwipeArea.Bottom:
                    Debug.Log("Idk?");
                    OnResult?.Invoke(Result.Idk);
                    break;
            }
        }

        private Result GetResult(Gender target)
        {
            return _wordData.Gender == target ? Result.Correct : Result.Incorrect;
        }

        private void HandleResult(Result res)
        {
            Debug.Log($"{res}. {_wordData.ToPrint()}");
            if (res == Result.Incorrect)
            {
                StartCoroutine(AnimateReturnToCenter(_returnToCenterOnIncorrectDuration));
                // TODO: Some other visual feedback like changing background color.
            }
            else
            {
                StartCoroutine(AnimateSwipingCardAway(destroyAfterwards: true));
            }
        }

        private IEnumerator AnimateSwipingCardAway(bool destroyAfterwards = false)
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

            if (destroyAfterwards)
                Destroy(gameObject);
        }

        private IEnumerator AnimateReturnToCenter(float duration)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = _defaultPosition;

            Quaternion startRot = transform.localRotation;
            Quaternion endRot = Quaternion.identity;

            float timer = 0f;
            while (timer < duration)
            {
                float t = timer / duration;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                transform.localRotation = Quaternion.Lerp(startRot, endRot, t);
                yield return null;
                timer += Time.deltaTime;
            }
            transform.position = _defaultPosition;
            transform.localRotation = Quaternion.identity;
        }

        public void UpdateTagAppearances(SwipeArea area, float alpha)
        {
            foreach (var tag in _genderTags)
            {
                tag.UpdateAlpha(area, alpha);
            }
        }
    }
}
