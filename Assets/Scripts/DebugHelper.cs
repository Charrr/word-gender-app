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

        private void Update()
        {
            TestPosition = TestTransform.position;
        }
    }
}
