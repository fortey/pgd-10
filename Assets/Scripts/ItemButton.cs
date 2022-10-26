using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Button _button;
    private Entity _entity;
    private bool _isSelect;

    public void Initialize(Entity entity)
    {
        _entity = entity;
        _textMeshPro.text = entity.name;
        transform.SetAsFirstSibling();
        gameObject.SetActive(true);
    }


}
