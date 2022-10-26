using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    private List<Entity> _items = new List<Entity>();
    [SerializeField] private ItemButton _itemButtonPrefab;
    [SerializeField] private Transform _itemContainer;

    public void AddItem(Entity item)
    {
        _items.Add(item);

        var itemButton = Instantiate(_itemButtonPrefab, _itemContainer);
        itemButton.Initialize(item);
    }

    public void RemoveItem(Entity item)
    {
        _items.Remove(item);
    }

    public void onItemButtonClick()
    {

    }

}
