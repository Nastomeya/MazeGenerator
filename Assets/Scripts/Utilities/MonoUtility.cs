using UnityEngine;

namespace Utilities
{
    public static class MonoUtility
    {
        internal static T GetItOrAddIt<T>(GameObject _obj) where T : Component
        {
            var _component = _obj.GetComponent<T>();
            if (_component == null)
                _component = _obj.AddComponent<T>();
            return _component;
        }
        internal static float CountDown(float _time, float _speed)
        {
            return _time - (Time.deltaTime * _speed);
        }
        internal static float GetTime01(float _turningSpeed, float _speed)
        {
            return Mathf.Clamp01(Time.deltaTime * _speed * _turningSpeed);
        }
    }

}