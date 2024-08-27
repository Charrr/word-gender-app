using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class BackgroundManager : Singleton<BackgroundManager>
    {
        public GameObject FallBackBackground;
        public SwipeAreaColoredBackground SwipeAreaBackground;
        public ResultColoredBackground ResultBackground;

        public void SetSwipeAreaBackgroundColor(SwipeArea area, float alpha)
        {
            SwipeAreaBackground.SetColorPerSwipeArea(area, alpha);
        }
    }
}
