using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiScene : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
