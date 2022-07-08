using UnityEngine;

namespace MathCalculation
{
    public static class BasicMath
    {
        public const float TAU = Mathf.PI * 2f;

        public static float Abs(float value)
        {
            if (value < 0)
                value = value * -1f;

            return value;
        }
        public static int Abs(int value)
        {
            if (value < 0)
                value = value * -1;

            return value;
        }


        public static float Distance(Vector3 originalPosition, Vector3 targetPosition)
        {
            float xD, yD, zD;
            float result;

            xD = GetAbsSubstraction(originalPosition.x, targetPosition.x);
            yD = GetAbsSubstraction(originalPosition.y, targetPosition.y);
            zD = GetAbsSubstraction(originalPosition.z, targetPosition.z);
            result = Mathf.Sqrt(xD * xD + yD * yD + zD * zD);

            return result;
        }

        private static float GetAbsSubstraction(float valueA, float valueB)
        {
            float result = (valueA > valueB) ? valueA - valueB : valueB - valueA;
            return result;
        }

        /// <summary> ie. 0f,0.25f,0.5f,0.75f,0.75f,0.5f,0.25f,0f</summary>
        public static float ConvertIncrement2UCurve(float value, int totalElement)
        {
            var half = totalElement / 2f;
            if (totalElement % 2 != 0)//odd
                return (value > half ? (totalElement - 1 - value) : value) / totalElement * 2f;
            else//even
                return (value >= half ? (totalElement - 1 - value) : value) / totalElement * 2f;
        }
        /// <summary> ie. 012343210 or 01233210 </summary>
        public static int Convert2InvertedUSequenceValue(int value, int totalElement)
        {
            var finalIndex = totalElement - 1;
            var reverseValue = finalIndex - value;
            var symmetryValue = Abs(value - reverseValue);
            var evenSymmetryValue = finalIndex - symmetryValue;
            var result = evenSymmetryValue == 0 ? 0 : evenSymmetryValue / 2;
            return result;
        }
        /// <summary> ie. -3-2-1 0 1 2 3 4 or -4-3-2-1 0 1 2 3 4 </summary>
        public static int Convert2USequenceValue(int value, int totalElement)
        {
            var finalIndex = totalElement - 1;
            var reverseValue = finalIndex - value;
            var symmetryValue = value - reverseValue;// Abs(value - reverseValue);           
            int result = 0;
            if (symmetryValue != 0)
            {
                if (symmetryValue < 0)
                    result = Mathf.CeilToInt(symmetryValue / 2f);
                else
                    result = Mathf.FloorToInt(symmetryValue / 2f);
            }
            if (totalElement % 2 == 0)
            {
                var half = totalElement / 2;
                if (value >= half)
                    result += 1;
            }
            return result;
        }
    }
}