using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    private List<string> _items = new List<string>();
    [SerializeField] private TextMeshProUGUI _text;

    public void AddItem(string item)
    {
        _items.Add(item);
        RefreshText();
    }

    public void RemoveItem(string item)
    {
        _items.Remove(item);
        RefreshText();
    }

    private void RefreshText()
    {
        _text.text = string.Join("\n", _items);
    }

}
