using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

    private static Singleton instance_;

    private Singleton() { }

    public static Singleton Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = new Singleton();
            }
            return instance_;
        }
    }

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
    }
}
