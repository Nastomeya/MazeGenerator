using System;
using System.Collections;
using UnityEngine;

namespace JasonPoolSystem
{
    public class PooledMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private int initialPoolSize = 50;
        public int InitialPoolSize { get { return initialPoolSize; } }
        public event Action<PooledMonoBehaviour> OnReturnToPool = delegate { };

        private static Pool pool;//private static Pool pool;

        internal virtual void OnDisable() => OnReturnToPool.Invoke(this);
        public T Get<T>(bool enable = true) where T : PooledMonoBehaviour
        {
            pool = Pool.GetPool(this);

            var pooledObject = pool.Get<T>();
            if (enable) pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }
        public T Get<T>(Vector3 _position, Quaternion _rotation) where T : PooledMonoBehaviour
        {
            var pooledObject = Get<T>();
            pooledObject.transform.position = _position;
            pooledObject.transform.rotation = _rotation;

            return pooledObject;
        }
        public Pool GetPool()
        {
            return pool;
        }
        internal void ReturnToPool(float delay = 0, Action _doItAfterSeconds = null)
        {
            if (delay == 0) Vanished();
            else StartCoroutine(ReturnToPoolAfeterSeconds(delay, _doItAfterSeconds));
        }
        internal void SetPoolSize(int _poolSize) => initialPoolSize = _poolSize;
        private void Vanished() => gameObject.SetActive(false);

        private IEnumerator ReturnToPoolAfeterSeconds(float delay, Action _doItAfterSeconds)
        {
            yield return new WaitForSeconds(delay);
            if (_doItAfterSeconds != null)
                _doItAfterSeconds?.Invoke();
            Vanished();
        }
    }
}