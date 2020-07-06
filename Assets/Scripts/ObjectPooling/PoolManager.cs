using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectPooling
{
    public enum PoolObjectType
    {
        None = 0,
        PurpleBullet,
        ImprovedBullet,
        WaveBullet,
        ShotgunBullet,
        EnemyZombie,
        EnemySkeleton,
        EnemyRobot
    }

    [System.Serializable]
    public class PoolInfo
    {
        public PoolObjectType type;
        public int amount;
        public GameObject prefab;
        public GameObject container;

        [HideInInspector] public List<GameObject> pool = new List<GameObject>();
    }

    public class PoolManager : GenericSingleton<PoolManager>
    {
        [SerializeField] private List<PoolInfo> listOfPool = new List<PoolInfo>();

        private readonly Vector3 _defaultPos = new Vector3(0, 0, 0);

        private void Start()
        {
            foreach (var t in listOfPool)
            {
                FindPool(t);
            }
        }

        private void FindPool(PoolInfo info)
        {
            for (var i = 0; i < info.amount; i++)
            {
                var obInstance = Instantiate(info.prefab, info.container.transform);
                obInstance.gameObject.SetActive(false);
                obInstance.transform.position = _defaultPos;
                info.pool.Add(obInstance);
            }
        }

        public GameObject GetPoolObject(PoolObjectType type)
        {
            var selected = GetPoolByType(type);
            var pool = selected.pool;

            GameObject obInstance;
            if (pool.Count > 0)
            {
                obInstance = pool[pool.Count - 1];
                pool.Remove(obInstance);
            }
            else
                obInstance = Instantiate(selected.prefab, selected.container.transform);

            return obInstance;
        }

        public void CoolObject(GameObject ob, PoolObjectType type)
        {
            ob.SetActive(false);
            ob.transform.position = _defaultPos;

            var selected = GetPoolByType(type);
            var pool = selected.pool;

            if (!pool.Contains(ob))
                pool.Add(ob);
        }

        private PoolInfo GetPoolByType(PoolObjectType type)
        {
            return listOfPool.FirstOrDefault(t => type == t.type);
        }
    }
}