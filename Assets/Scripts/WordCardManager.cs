using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class WordCardManager : Singleton<WordCardManager>
    {
        public CanvasGroup LeftColoredBackground;
        public CanvasGroup RightColoredBackground;
        public CanvasGroup TopColoredBackground;
        public CanvasGroup BottomColoredBackground;

        public Dictionary<Datatypes.SwipeArea, CanvasGroup> ColoredBackgroundDict;

        protected override void Awake()
        {
            base.Awake();

            ColoredBackgroundDict = new()
            {
                { Datatypes.SwipeArea.Left, LeftColoredBackground },
                { Datatypes.SwipeArea.Right, RightColoredBackground },
                { Datatypes.SwipeArea.Top, TopColoredBackground },
                { Datatypes.SwipeArea.Bottom, BottomColoredBackground }
            };
        }
    }
}