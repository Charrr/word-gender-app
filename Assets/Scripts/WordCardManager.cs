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

        [Header("Word Card References")]
        [SerializeField]
        private GameObject _wordCardPrefab;
        [SerializeField]
        private Transform _wordCardSpawnRoot;
        [SerializeField]
        private WordCard _defaultWordCard;
        private Vector2 _wordCardDefaultPos;

        public Dictionary<Datatypes.SwipeArea, CanvasGroup> ColoredBackgroundDict;
        public List<Datatypes.WordData> WordList = new();

        protected override void Awake()
        {
            base.Awake();

            SetUpColorBackgrounds();
            _wordCardDefaultPos = _defaultWordCard.transform.position;

            InitDummyWordList();
        }

        private void Start()
        {
            foreach (var wordData in WordList)
            {
                var card = Instantiate(_wordCardPrefab, _wordCardSpawnRoot).GetComponent<WordCard>();
                card.WordData = wordData;
            }
        }

        private void InitDummyWordList()
        {
            WordList.Add(new Datatypes.WordData("das", "Messer"));
            WordList.Add(new Datatypes.WordData("der", "Teller"));
            WordList.Add(new Datatypes.WordData("die", "Gabel"));
            WordList.Add(new Datatypes.WordData("die", "Sonne"));
            WordList.Add(new Datatypes.WordData("der", "Mond"));
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

        /// <summary>
        /// Determine how transparent the gender tag (and the background) should be
        /// as per the position of the word card. See /Docs/SwipeAreaAlpha.md for details.
        /// </summary>
        /// <param name="pos">Position of the word card.</param>
        /// <returns>A value between 0 and 1 as the alpha of the gender tag (and the backaground)</returns>
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