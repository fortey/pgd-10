using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextField _textField;

    public void ShowText(string text)
    {
        gameObject.SetActive(true);
        _textField.SetText(text);
    }
}
