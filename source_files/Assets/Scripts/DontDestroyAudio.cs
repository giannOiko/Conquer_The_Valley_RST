using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    public static DontDestroyAudio Instance = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
		{
			Instance = this;
		}
        else if (Instance != this)
		{
			Destroy(gameObject);
		}
        
        DontDestroyOnLoad(transform.gameObject);
    }
}
