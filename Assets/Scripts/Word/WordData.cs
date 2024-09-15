using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordGenderApp
{
    public class GenderV2
    {
        public bool Masc { get; private set; }
        public bool Fem { get; private set; }
        public bool Neu { get; private set; }

        public int Code
        {
            get
            {
                return 1 * (Masc ? 1 : 0) + 2 * (Fem ? 1 : 0) + 4 * (Neu ? 1 : 0); 
            }
            set
            {
                bool[] flags = CodeToFlags(value);
                Masc = flags[0];
                Fem = flags[1];
                Neu = flags[2];
            }
        }

        public GenderV2(int code)
        {
            Code = code;
        }

        public GenderV2(string gender)
        {
            string[] genders = gender.Split('/');
            foreach(var g in genders)
            {
                if (string.Compare(g, "der", ignoreCase: true) == 0)
                    Masc = true;
                else if (string.Compare(g, "die", ignoreCase: true) == 0)
                    Fem = true;
                else if (string.Compare(g, "das", ignoreCase: true) == 0)
                    Neu = true;
            }
        }

        public static bool[] CodeToFlags(int code)
        {
            if (code < 0)
                return new bool[3];

            string binary = Convert.ToString(code, 2).PadLeft(3, '0');
            return Array.ConvertAll(binary.ToCharArray(), x => x == '1');
        }
    }

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

        public int GenderCode
        {
            get
            {
                int code = Gender switch
                {
                    Gender.m => 4,
                    Gender.f => 2,
                    Gender.n => 1,
                    _ => 0,
                };
                return code;
            }
        }

        public override string ToString()
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
}
