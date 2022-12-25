using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music instance;
    void Awake()
    {
        // Mark first gameObject that loads this script as DontDestroyOnLoad and
        // destroy every gameObject made with this script after the first, or
        // there will be duplicate Music players when returning to Main Menu.
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
            return;
        }
        Destroy(gameObject);
    }
}
