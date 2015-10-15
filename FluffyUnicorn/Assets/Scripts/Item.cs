using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    #region Public variables
    public int m_Money;     //how much money to give to the player

    public string m_SpawnPoint = "Chest";

    public Vector2 m_RandomXPos;
    public Vector2 m_RandomYPos;
    public Vector2 m_RandomXForce;
    public Vector2 m_RandomYForce;
    #endregion

    #region Private variables
    private PlayerData player_;

    private enum m_Item
    {
        HEALTH = 0,
        MONEY
    };
    private m_Item m_ItemType;
    #endregion

    void Start () 
    {
        player_ = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
	}

    #region Set item
    public void MakeHealth()
    {
        m_ItemType = m_Item.HEALTH;
    }
    public void MakeMoney()
    {
        m_ItemType = m_Item.MONEY;
    }

    public void SetImage(Sprite image)
    {
        if (this.GetComponent<SpriteRenderer>().sprite == null) // if the sprite on spriteRenderer is null then
        {
            this.GetComponent<SpriteRenderer>().sprite = image;
        }

    }
    #endregion

    #region Upgrades
    public void FillHealth()
    {
        //give health and remove item 
        player_.m_PlayerHealth = Constants.PLAYER_DEFAULT_MAX_HEALTH;
        ObjectPool.Instance.PoolObject(this.gameObject);
    }

    public void AddCurrency()
    {
        //give currency and remove item 
        player_.m_Currency += m_Money;
        ObjectPool.Instance.PoolObject(this.gameObject);
    }
    #endregion

    public void GiveItem()
    {
        Vector3 randomPos = new Vector3(Random.Range(m_RandomXPos.x, m_RandomXPos.y), Random.Range(m_RandomYPos.x, m_RandomYPos.y), 0);            //random pos to spawn at
        Vector2 randomForce = new Vector2(Random.Range(m_RandomXForce.x, m_RandomXForce.y), Random.Range(m_RandomYForce.x, m_RandomYForce.y));     //random force to throw the item

        GameObject spawn = GameObject.FindGameObjectWithTag(m_SpawnPoint);   //where to spawn at. Find the chest game object

        //give item
        //set the position at the spawn point with the random pos added to it 
        //throw the object
        if (m_ItemType == m_Item.HEALTH)
        {
            this.transform.position = spawn.transform.position + randomPos;
            this.GetComponent<Rigidbody2D>().AddForce(randomForce);
        }
        else if (m_ItemType == m_Item.MONEY)
        {
            this.transform.position = spawn.transform.position + randomPos;
            this.GetComponent<Rigidbody2D>().AddForce(randomForce);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (m_ItemType == m_Item.HEALTH)
            {
                FillHealth();
            }
            else if (m_ItemType == m_Item.MONEY)
            {
                AddCurrency();
            }
        }
    }
}
