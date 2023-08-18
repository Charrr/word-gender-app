using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public enum Gender
    {
        m,
        f,
        n
    }

    public struct WordData
    {
        public Gender Gender;
        public string Word;

        public string ToPrint()
        {
            string g = Gender switch
            {
                Gender.m => "Der",
                Gender.f => "Die",
                Gender.n => "Das",
                _ => ""
            };
            return $"{g} {Word}";
        }

        public WordData(Gender gender, string word)
        {
            Gender = gender;
            Word = word;
        }

        public WordData(string gender, string word)
        {
            if (TryParseToGender(gender, out Gender))
            {
                Word = word;
            }
            else
            {
                Word = "GenderNotSet";
            }
        }

        private static bool TryParseToGender(string str, out Gender gender)
        {
            switch (str)
            {
                case "m":
                case "der":
                case "Der":
                    gender = Gender.m;
                    return true;
                case "f":
                case "die":
                case "Die":
                    gender = Gender.f;
                    return true;
                case "n":
                case "das":
                case "Das":
                    gender = Gender.n;
                    return true;
                default:
                    Debug.LogError("Cannot convert string to gender type! Assigning neuter by default.");
                    gender = Gender.n;
                    return false;
            }
        }
    }

    public enum SwipeArea
    {
        Left,
        Right,
        Bottom,
        Top
    }

    public enum Result
    {
        Correct,
        Incorrect,
        Idk
    }
}
