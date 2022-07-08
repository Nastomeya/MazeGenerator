using System.Collections.Generic;
using UnityEngine;

namespace JasonPoolSystem
{
    public class Pool : MonoBehaviour
    {
        private static readonly Dictionary<PooledMonoBehaviour, Pool> pools = new Dictionary<PooledMonoBehaviour, Pool>();
        private readonly Queue<PooledMonoBehaviour> objects = new Queue<PooledMonoBehaviour>();

        private PooledMonoBehaviour prefab, pooledObject;

        public static Pool GetPool(PooledMonoBehaviour _prefab)
        {
            if (pools.ContainsKey(_prefab))
                return pools[_prefab];

            var poolGameObject = new GameObject("Pool-" + _prefab.name);
            var pool = poolGameObject.AddComponent<Pool>();
            pool.prefab = _prefab;

            pools.Add(_prefab, pool);
            return pool;
        }
        public T Get<T>() where T : PooledMonoBehaviour//T could be anything as you can imagine.
        {
            if (objects.Count == 0)
            {
                GrowPool();
            }

            var _pooledObject = objects.Dequeue();
            return _pooledObject as T;
        }

        private void GrowPool()
        {
            for (int i = 0; i < prefab.InitialPoolSize; i++)
            {
                pooledObject = Instantiate(prefab);// as PooledMonoBehaviour;
                pooledObject.gameObject.name += "_" + i;

                pooledObject.OnReturnToPool += AddObjectToAvailableQueue;//subscribed.

                pooledObject.transform.SetParent(gameObject.transform);
                pooledObject.gameObject.SetActive(false);//after subscribed, we set it off and it will effected to enqueue
            }
        }
        private void OnDisable()
        {
            pooledObject.OnReturnToPool -= AddObjectToAvailableQueue;
        }
        private void AddObjectToAvailableQueue(PooledMonoBehaviour _pooledObject)
        {
            objects.Enqueue(_pooledObject);
        }
    }
}