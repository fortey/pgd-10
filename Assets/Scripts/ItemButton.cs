using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    public Entity entity { get; private set; }
    private bool _isSelect;

    public void Initialize(Entity entity)
    {
        this.entity = entity;
        _textMeshPro.text = entity.name;
        transform.SetAsFirstSibling();
        gameObject.SetActive(true);
    }

    public void OnClick()
    {
        _isSelect = !_isSelect;
        if (_isSelect)
        {
            Inventory.Instance.SelectItem(this);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
        }
        else
        {
            Inventory.Instance.DeselectItem();
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
        }
    }

    public void Deselect()
    {
        _isSelect = false;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
    }
}
