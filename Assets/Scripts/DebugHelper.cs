using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    [ExecuteInEditMode]
    public class DebugHelper : MonoBehaviour
    {
        public Transform TestTransform;
        public Vector3 TestPosition;
        public Vector3 PivotPosition;

        public float z;
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 p3;
        public Vector3 pXY;

        private void OnValidate()
        {
            if (!TestTransform) TestTransform = transform;
        }

        private void Update()
        {
            TestPosition = TestTransform.position;
            PivotPosition = TestTransform.GetComponent<RectTransform>().anchoredPosition;

            if (Application.isPlaying) z = WordCardManager.Instance.DetermineGenderTagAlpha(TestPosition);
        }
    }
}
