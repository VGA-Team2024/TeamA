using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private Text _cookieText;
    [SerializeField] private Text _cpsText;
    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.Instance.OnResourceChanged += ExpressResource;
        ResourceManager.Instance.OnResourceGenerateChanged += ExpressCPS;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceChanged -= ExpressResource;
        ResourceManager.Instance.OnResourceGenerateChanged -= ExpressCPS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExpressResource(decimal currentResource)
    {
        _cookieText.text = currentResource.ToString(".00");
    }
    void ExpressCPS(float currentCps)
    {
        _cpsText.text = currentCps.ToString(".00");
    }
}
