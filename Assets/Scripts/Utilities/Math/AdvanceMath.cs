using System.Collections.Generic;
using UnityEngine;

namespace MathCalculation
{
    public static class AdvanceMath
    {
        private static readonly float _pi = Mathf.PI;
        public static float SuperEllipse(float degree, float _edgeVarious)
        {
            //I apply the super-ellipse equation :x(θ) = |cosθ|^(2/n)*sgin(cosθ)*a, y(θ) = |sinθ|^(2/n)*sgin(sinθ)*b, which runs perfectly.
            return Mathf.Sign(degree) * Mathf.Pow(BasicMath.Abs(degree), 2 / _edgeVarious);
        }
        public static float GetRadiansWithLengths(float xWidth, float yLength)
        {
            float abs_X = BasicMath.Abs(xWidth);
            float abs_Y = BasicMath.Abs(yLength);
            float sign_X = Mathf.Sign(xWidth);
            float sign_Y = Mathf.Sign(yLength);
            //use formula as follow
            //atan(x, y) = 180 / pi() * (pi() - pi() / 2 * (1 + sign(x)) * (1 - sign(y ^ 2)) - pi() / 4 * (2 + sign(x)) * sign(y) - sign(x * y) * atan((abs(x) - abs(y)) / (abs(x) + abs(y))))
            float rad = 180f / _pi * (_pi - _pi / 2f * (1f + sign_X)) * (1f - Mathf.Sign(Mathf.Pow(yLength, 2f))) - _pi / 4f * (2f + sign_X) * sign_Y - Mathf.Sign(yLength * xWidth) * Mathf.Atan((abs_X - abs_Y) / (abs_X + abs_Y));
            return rad;
        }

        /// <summary>
        /// It's Acutally An Radian
        /// </summary>
        /// <param name="x">Direction of X</param>
        /// <param name="y">Direction of Y</param>
        public static float GetRadian(float x, float y)
        {
            var radian = Mathf.PI - (Mathf.PI / 2f) * (1 + Mathf.Sign(x)) * (1 + Mathf.Sign(y * y)) - (Mathf.PI / 4f) * (2f + Mathf.Sign(x)) * Mathf.Sign(y) - Mathf.Sign(x * y) * Mathf.Atan((BasicMath.Abs(x) - BasicMath.Abs(y)) / (BasicMath.Abs(x) + BasicMath.Abs(y)));
            return radian;
        }
        public static float GetDegree(Vector3 _direction)
        {
            float degree = GetRadian(_direction.x, _direction.z) * Mathf.Rad2Deg;
            float newDegree = degree < 0 ? BasicMath.Abs(degree + 90f) : degree;
            return newDegree;
        }
        public static float GetHypotenuseLength(float _adjacent, float _opposite)
        {
            return Mathf.Sqrt(_adjacent * _adjacent + _opposite * _opposite);
        }
        /// <summary> Get Actual Inspector's Input Rotation(As resutl, it will output different angle) </summary>
        public static float GetInputRotation(float sourceAngle)
        {
            return BasicMath.Abs(180 + (((sourceAngle > 179) ? -1 : 1) * (sourceAngle))) % 360;
        }
        /// <summary> Me stealed this from "UnityEngine.ProBuilder" </summary>
        public static Vector2 PointInCircumference(float radius, float angleInDegrees, Vector2 origin)
        {
            // Convert from degrees to radians via multiplication by PI/180
            float x = (float)(radius * Mathf.Cos(Mathf.Deg2Rad * angleInDegrees)) + origin.x;
            float y = (float)(radius * Mathf.Sin(Mathf.Deg2Rad * angleInDegrees)) + origin.y;

            return new Vector2(x, y);
        }

        public static float GetAcceleratedSpeed(float initDist, float currentDist)
        {
            initDist = Mathf.Clamp(initDist, 0.01f, initDist);
            var speed = Mathf.Sin(currentDist / initDist * _pi);
            return Mathf.Clamp(speed, 0.6f, 1f);
            //return  Mathf.Sin(currentDist / initDist * _pi);            
        }
        /// <summary> Gathering the proper ratio of the collider's radius that will get the correct radius size wherever the collider located </summary>
        /// <param name="_transform"> This should be the parent or sibling transform of that collider </param>
        /// <param name="_collider"> This collider can be anywhere from it's childern</param>
        /// <returns> Actual Radius of that Collider </returns>
        public static float GetColliderRadius(Transform _transform, Collider _collider)
        {
            return _collider.bounds.extents.x / _transform.lossyScale.x;
        }

        public static Bounds GetMeanBounds(Transform _rootTrans, bool _IncludeSelf = false)
        {
            var allRenderers = new List<MeshRenderer>(_rootTrans.GetComponentsInChildren<MeshRenderer>());
            if (allRenderers == null || allRenderers.Count == 0)
                return new Bounds(Vector3.zero, Vector3.one);

            var allBounds = new Bounds(allRenderers[0].transform.position, Vector3.zero);
            for (int i = 0; i < allRenderers.Count; i++)
            {
                if (_IncludeSelf)
                {
                    allBounds.Encapsulate(allRenderers[i].bounds);
                    continue;
                }
                if (allRenderers[i].transform != _rootTrans)
                    allBounds.Encapsulate(allRenderers[i].bounds);
            }
            return allBounds;
        }
    }
}
