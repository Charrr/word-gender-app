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

        private Vector3 _defaultPosition;
        private Vector2 _delta;

        public Datatypes.WordData WordData { get; private set; }
        public string Word => WordData.Word;

        private void OnValidate()
        {
            if (!_wordText) _wordText = GetComponentInChildren<TMP_Text>();
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
            transform.position = _defaultPosition;
        }
    }
}
