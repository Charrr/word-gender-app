using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class WordCardManager : Singleton<WordCardManager>
    {
        public GameObject LeftColoredBackground;
        public GameObject RightColoredBackground;
        public GameObject TopColoredBackground;

        public Dictionary<Datatypes.SwipeArea, GameObject> ColoredBackgroundDict;

        protected override void Awake()
        {
            base.Awake();

            ColoredBackgroundDict = new()
            {
                { Datatypes.SwipeArea.Left, LeftColoredBackground },
                { Datatypes.SwipeArea.Right, RightColoredBackground },
                { Datatypes.SwipeArea.Top, TopColoredBackground }
            };
        }
    }
}