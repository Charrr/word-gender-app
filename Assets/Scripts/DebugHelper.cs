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

        private void OnValidate()
        {
            if (!TestTransform) TestTransform = transform;
        }

        private void Update()
        {
            TestPosition = TestTransform.position;
            PivotPosition = TestTransform.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
