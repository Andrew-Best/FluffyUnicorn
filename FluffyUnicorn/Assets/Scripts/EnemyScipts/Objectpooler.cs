using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Objectpooler : MonoBehaviour 
{
	[System.Serializable]
	public class ObjectPoolEntry
	{
		[SerializeField]
		public GameObject mPrefab;

		[SerializeField]
		public int mCount = 3;

	}

	public ObjectPoolEntry[] m_Entries;

	public List<GameObject>[] m_Pool;

	protected GameObject containerObject_;

	public static Objectpooler Instance { get { return instance_; } }
	private static Objectpooler instance_ = null;

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

		m_Pool = new List<GameObject>[m_Entries.Length];

		for (int i = 0; i < m_Pool.Length; ++i)
		{
			ObjectPoolEntry prefab = m_Entries[i];
			m_Pool[i] = new List<GameObject>();
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

		for (int i = 0; i < m_Entries.Length; ++i)
		{
			if (m_Entries[i].mPrefab.name == obj.name)
			{
				obj.SetActive(false);
				obj.transform.parent = containerObject_.transform;
				m_Pool[i].Add(obj);
				return;
			}
		}
	}

	public GameObject GetObjectForType(string typeName, bool onlyPooled)
	{
		for (int i = 0; i < m_Entries.Length; ++i)
		{
			GameObject prefab = m_Entries[i].mPrefab;
			if (prefab.name == typeName)
			{
				if (m_Pool[i].Count > 0)
				{
					GameObject pooledObject = m_Pool[i][0];
					m_Pool[i].RemoveAt(0);
					pooledObject.transform.parent = null;
					pooledObject.SetActive(true);
					return pooledObject;
				}
				if (!onlyPooled)
				{
					GameObject newObject = (GameObject)GameObject.Instantiate(m_Entries[i].mPrefab);
					newObject.name = m_Entries[i].mPrefab.name;
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
