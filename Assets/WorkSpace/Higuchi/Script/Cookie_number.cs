using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie_number : MonoBehaviour
{
    public static float _cookie = 0;
    void Start()
    {

    }

    void Update()
    {
        _cookie += Time.deltaTime;
    }
}
