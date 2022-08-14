using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{

    public static class Datatypes
    {
        public enum Gender
        {
            m,
            f,
            n,
        }

        public struct WordData
        {
            public string Word;
            public Gender Gender;
        }
    }
}
