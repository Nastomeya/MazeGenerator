using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataExtensions
{
    public class Bitwise
    {
        internal static int GetOppoisteElement(int _enum, int _oppositePair) => _enum ^ _oppositePair;
        internal static int Remove(int _enum, int _removeEnum)
        {
            if ((_enum & _removeEnum) > 0)//make sure it has something to be removed 1st
                _enum ^= _removeEnum;
            return _enum;
        }
        internal static int GetSumOfEnumValues<T>(T _dummyEnumType)
        {
            int[] bitwiseElements = GetElements(_dummyEnumType);
            int fullElement = 0;
            for (int i = 0; i < bitwiseElements.Length; i++)
                fullElement |= bitwiseElements[i];

            return fullElement;//Enum.GetValues(typeof(T)).Cast<int>().Sum(x => x);
        }
        internal static int[] GetElements<T>(T _dummyEnumType) => (int[])Enum.GetValues(_dummyEnumType.GetType());//(int[])Enum.GetValues(typeof(T));
        internal static int CombinationCount(int[] bitwiseElements, int combination)
        {
            int count = 0;
            for (int i = 0; i < bitwiseElements.Length; i++)
            {
                if ((bitwiseElements[i] & combination) == bitwiseElements[i])
                    count++;
            }
            return count;
        }
        internal static bool IsContainedOneOfPairSet(int combination, int bitwiseElement, int _oppositePair)
        {
            return (combination & bitwiseElement) == bitwiseElement && (combination | bitwiseElement) == combination && (combination | _oppositePair) != combination;
        }
        internal static bool IsContainedBothPairSet(int combination, int _oppositePair)
        {
            return (combination | _oppositePair) == combination;
        }
        internal static bool HasOneOfTheElement(int combination, int bitwiseElement)
        {
            return (combination & bitwiseElement) > 0;
        }
        internal static bool OnlyOppositePairCheck(int combination, int _oppositePair)
        {
            if ((combination | _oppositePair) == _oppositePair)
                return true;
            return false;
        }

        /// <summary> Check If The Enum Value Is Not A Combination Enum </summary>
        /// <param name="_enumValue">enum value</param>
        internal static bool CheckIfAnEnumElement(int _enumValue)
        {
            var powerValue = Mathf.Log(_enumValue, 2);
            if (powerValue == 0)
                return true;

            float beingMultipleOfOne = powerValue / Mathf.Floor(powerValue);
            if (beingMultipleOfOne == 1f)
                return true;

            return false;
        }
        internal static object GetCombinationNames<T>(T enumType, int _combination)
        {
            var rootValue = Mathf.Log(_combination, 2);
            bool IsOriginalEnumElement = rootValue - Mathf.FloorToInt(rootValue) == 0;
            if (IsOriginalEnumElement)
                return GetName(enumType, _combination);
            else
            {
                var enumElements = GetElements(enumType);
                string name = "";
                for (int i = 0; i < enumElements.Length; i++)
                {
                    if ((_combination | enumElements[i]) == _combination)
                    {
                        var addSign = "<color=red>+</color>";
                        if (name == "")
                            name += GetName(enumType, enumElements[i]);
                        else
                            name += addSign + GetName(enumType, enumElements[i]);
                    }
                }
                return name;
            }
        }
        //the list doesn't need to be returned cause, it's already been referenced.
        internal static void GetCombinationValues<T>(List<int> elements, T enumType, int _combination)//internal static List<int> GetCombinationValues<T>(List<int> elements, T enumType, int _combination)
        {
            var rootValue = Mathf.Log(_combination, 2);
            bool IsOriginalEnumElement = rootValue - Mathf.FloorToInt(rootValue) == 0;
            if (IsOriginalEnumElement)
                elements.Add(_combination);
            else
            {
                var enumElements = GetElements(enumType);
                for (int i = 0; i < enumElements.Length; i++)
                {
                    if ((_combination | enumElements[i]) == _combination)
                        elements.Add(enumElements[i]);
                }
            }
            //return elements;
        }
        private static string GetName<T>(T enumType, int _combination) => "<color=orange>" + _combination + "</color>:" + Enum.GetName(enumType.GetType(), _combination);
    }
}