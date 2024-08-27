using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class WordCardManager : Singleton<WordCardManager>
    {
        [Header("Word Card References")]
        [SerializeField]
        private GameObject _wordCardPrefab;
        [SerializeField]
        private Transform _wordCardSpawnRoot;
        [SerializeField]
        private WordCard _defaultWordCard;
        private Vector2 _wordCardDefaultPos;

        public List<WordData> WordList = new();

        /// <summary>
        /// The word card on top layer of the UI, which is the last child under the spawn root.
        /// </summary>
        public WordCard CurrentCard
        {
            get
            {
                int count = _wordCardSpawnRoot.childCount;
                if (count > 0)
                {
                    return _wordCardSpawnRoot.GetChild(count - 1).GetComponent<WordCard>();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Which area the current card belongs to at this moment.
        /// </summary>
        public SwipeArea CurrentCardArea => DetermineSwipeArea(CurrentCard.transform.position);
        public bool ShouldUpdateBackground { get; set; } = true;

        /// <summary>
        /// A value between 0 and 1 that represents how much a card is decided to be swiped to one area.
        /// 0 corresponds to the state where the card lies untouched at the center.
        /// 1 corresponds to states when the user has fully dragged a card 
        /// </summary>
        public float CurrentCardDecisionAlpha => DetermineDecisionAlpha(CurrentCard.transform.position);

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.2f, 1f, 0.5f);
            //Gizmos.DrawSphere(_defaultWordCard.transform.position, 10f);

            Vector2[] corners = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1),
            };

            foreach (var corner in corners)
            {
                Gizmos.DrawLine(_defaultWordCard.transform.position, Camera.main.ViewportToScreenPoint(corner));
            }
        }

        protected override void Awake()
        {
            base.Awake();
            InitDummyWordList();
        }

        private void Start()
        {
            InstantiateWordCardsFromList();
            _wordCardDefaultPos = _defaultWordCard.transform.position;
            _defaultWordCard.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (CurrentCard == null)
                return;

            float alpha = CurrentCardDecisionAlpha;
            SwipeArea area = CurrentCardArea;
            CurrentCard.UpdateTagAppearances(area, alpha);

            if (ShouldUpdateBackground)
                BackgroundManager.Instance.SwipeAreaBackground.SetColorPerSwipeArea(area, alpha);
        }

        private void InitDummyWordList()
        {
            WordList = WordLoader.LoadWords();
        }

        private void InstantiateWordCardsFromList()
        {
            foreach (var wordData in WordList)
            {
                var card = Instantiate(_wordCardPrefab, _wordCardSpawnRoot).GetComponent<WordCard>();
                card.WordData = wordData;
                card.gameObject.name = "Word Card - " + card.WordData.Word;
            }
        }

        // Illustration of the division of four swipe areas.
        // The central intersection corresponds to the position of the word card.
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
        /// <summary>
        /// Determine which swipe area a 2D position falls into.
        /// </summary>
        /// <param name="point">2D position of a point to be examined.</param>
        /// <returns>The area that the given point falls into.</returns>
        public SwipeArea DetermineSwipeArea(Vector2 point)
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
                    return SwipeArea.Bottom;
                }
                else if (y < Mathf.Lerp(h, y0, x / x0))
                {
                    return SwipeArea.Left;
                }
                else
                {
                    return SwipeArea.Top;
                }
            }
            else
            {
                if (y < Mathf.Lerp(0, y0, (w - x) / (w - x0)))
                {
                    return SwipeArea.Bottom;
                }
                else if (y < Mathf.Lerp(h, y0, (w - x) / (w - x0)))
                {
                    return SwipeArea.Right;
                }
                else
                {
                    return SwipeArea.Top;
                }
            }
        }

        /// <summary>
        /// Determine how transparent the gender tag (and the background) should be
        /// as per the position of the word card. See /Docs/SwipeAreaAlpha.md for details.
        /// </summary>
        /// <param name="pos">Position of the word card.</param>
        /// <returns>A value between 0 and 1 as the alpha of the gender tag (and the backaground)</returns>
        public float DetermineDecisionAlpha(Vector2 pos)
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

        /// <summary>
        /// Given a plane formed by three points, shoot a ray from a point xy plane, and get the
        /// distance of the raycast on this plane. The value is thus the height for this point.
        /// </summary>
        /// <param name="p1">1st 3D point to form a plane.</param>
        /// <param name="p2">2nd 3D point to form a plane.</param>
        /// <param name="p3">3rd 3D point to form a plane.</param>
        /// <param name="pXY">Any point from the xy plane to be examined.</param>
        /// <returns>The z value i.e. the height of the intersection of the up-shooting ray and the plane.</returns>
        public static float GetIntersectionHeightAsAlpha(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pXY)
        {
            Plane plane = new(p1, p2, p3);
            Ray ray = new(origin: pXY, direction: new Vector3(0f, 0f, 1f));
            plane.Raycast(ray, out float z);
            return z;
        }
    }
}