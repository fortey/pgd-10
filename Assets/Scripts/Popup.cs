using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextField _textField;
    public Action onAction;

    public void ShowText(string url)
    {
        gameObject.SetActive(true);
        _textField.SetText(url);
    }

    public void OnAction()
    {
        onAction();
    }
}
