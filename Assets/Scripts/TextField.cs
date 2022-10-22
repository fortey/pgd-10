using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextField : MonoBehaviour, IPointerClickHandler
{
    public Func<string, string> GetPart;
    private TextMeshProUGUI _textMeshPro;
    private Camera _camera;
    void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _camera = Camera.main;
    }

    public void SetText(string text)
    {
        _textMeshPro.text = ParseText(text);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _textMeshPro.textInfo.linkInfo[linkIndex];
            SetText(GetPart(linkInfo.GetLinkID()));
        }
    }

    private string ParseText(string text)
    {
        var cur_text = text;
        while (cur_text.IndexOf('{') != -1)
        {
            var start = cur_text.IndexOf('{');
            var separator = cur_text.IndexOf(' ', start);
            var end = cur_text.IndexOf('}');

            if (separator == -1 || end == -1) break;

            var url = cur_text.Substring(start + 1, separator - start - 1);
            var name = cur_text.Substring(separator + 1, end - separator - 1);
            var link = $"<link=\"{url}\"><u><color=blue>{name}</color></u></link>";
            cur_text = cur_text.Replace(cur_text.Substring(start, end - start + 1), link);
        }

        while (cur_text.IndexOf('[') != -1)
        {
            var start = cur_text.IndexOf('[');
            var separator = cur_text.IndexOf(' ', start);
            var end = cur_text.IndexOf(']');

            if (separator == -1 || end == -1) break;

            var url = cur_text.Substring(start + 1, separator - start - 1);
            var name = cur_text.Substring(separator + 1, end - separator - 1);
            var link = $"<link=\"{url}\"><u><color=blue>{name}</color></u></link>";
            cur_text = cur_text.Replace(cur_text.Substring(start, end - start + 1), link);
        }

        return cur_text;
    }
}
