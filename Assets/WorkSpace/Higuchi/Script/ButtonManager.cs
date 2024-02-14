using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public float required_count = 0;
    Button _button;
    float _currentcookie = 0; //現在のクッキー数
    void Start()
    {
        _button = GetComponent<Button>();
        _button.interactable = false;
        _button.onClick.AddListener(Consumed_cookie);
    }

    void Update()
    {
        _currentcookie += Time.deltaTime;
        Debug.Log("現在のクッキー: " + _currentcookie);
        _button.interactable = (_currentcookie >= required_count);
    }
    void Consumed_cookie()
    {
        if (_currentcookie >= required_count)
            _currentcookie -= required_count;
    }
}
