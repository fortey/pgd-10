using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public string selectedItem { get; private set; } = "";
    private List<Entity> _items = new List<Entity>();
    private List<ItemButton> _itemsButtons = new List<ItemButton>();
    [SerializeField] private ItemButton _itemButtonPrefab;
    [SerializeField] private Transform _itemContainer;

    public void AddItem(Entity item)
    {
        _items.Add(item);

        var itemButton = Instantiate(_itemButtonPrefab, _itemContainer);
        itemButton.Initialize(item);
        _itemsButtons.Add(itemButton);
    }

    public void RemoveItem(Entity item)
    {
        _items.Remove(item);
    }

    public void onItemButtonClick()
    {

    }

    public void SelectItem(ItemButton button)
    {
        selectedItem = button.entity.id;
        foreach (var itemButton in _itemsButtons)
        {
            if (itemButton != button)
                itemButton.Deselect();
        }
    }

    public void DeselectItem()
    { selectedItem = ""; }

}
