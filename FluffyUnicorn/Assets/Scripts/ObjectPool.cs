using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {


    [System.Serializable]
    public class ObjectPoolEntry
    {
        [SerializeField]
        public GameObject mPrefab;

        [SerializeField]
        public int mCount = 3;

    }

    public ObjectPoolEntry[] mEntries;

    public List<GameObject>[] mPool;

    protected GameObject containerObject_;

    public static ObjectPool Instance { get { return instance_; } }
    private static ObjectPool instance_ = null;

    void Awake()
    {
        if (instance_ != null && instance_ != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance_ = this;
        }

        DontDestroyOnLoad(this.gameObject);
        InitializePool();
    }

    void InitializePool()
    {
        containerObject_ = new GameObject("ObjectPool");
        containerObject_.transform.parent = transform;

        mPool = new List<GameObject>[mEntries.Length];

        for (int i = 0; i < mPool.Length; ++i)
        {
            ObjectPoolEntry prefab = mEntries[i];
            mPool[i] = new List<GameObject>();
            for (int j = 0; j < prefab.mCount; ++j)
            {
                GameObject newObj = (GameObject)GameObject.Instantiate(prefab.mPrefab);
                newObj.name = prefab.mPrefab.name;
                PoolObject(newObj);
            }
        }
    }

    public void PoolObject(GameObject obj)
    {

        for (int i = 0; i < mEntries.Length; ++i)
        {
            if (mEntries[i].mPrefab.name == obj.name)
            {
                obj.SetActive(false);
                obj.transform.parent = containerObject_.transform;
                mPool[i].Add(obj);
                return;
            }
        }
    }

    public GameObject GetObjectForType(string typeName, bool onlyPooled)
    {
        for (int i = 0; i < mEntries.Length; ++i)
        {
            GameObject prefab = mEntries[i].mPrefab;
            if (prefab.name == typeName)
            {
                if (mPool[i].Count > 0)
                {
                    GameObject pooledObject = mPool[i][0];
                    mPool[i].RemoveAt(0);
                    pooledObject.transform.parent = null;
                    pooledObject.SetActive(true);
                    return pooledObject;
                }
                if (!onlyPooled)
                {
                    GameObject newObject = (GameObject)GameObject.Instantiate(mEntries[i].mPrefab);
                    newObject.name = mEntries[i].mPrefab.name;
                    return newObject;
                }
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
