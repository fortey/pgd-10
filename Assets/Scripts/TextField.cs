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
    public Action<string> ShowPopup;
    private TextMeshProUGUI _textMeshPro;
    private Camera _camera;

    private Dictionary<string, string> _actionTexts = new Dictionary<string, string>();
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
            var url = linkInfo.GetLinkID();
            if (url.IndexOf("!") == -1)
            {
                if (_actionTexts.ContainsKey(linkInfo.GetLinkText()))
                    GameActions.instance.Invoke(_actionTexts[linkInfo.GetLinkText()]);
                SetText(GetPart(url));
            }
            else
            {
                url = url.Replace("!", "");
                ShowPopup(url);
            }


        }
    }

    private string ParseText(string text)
    {
        var cur_text = text;
        _actionTexts.Clear();

        while (cur_text.IndexOf('<') != -1)
        {
            var start = cur_text.IndexOf('<');
            var separator = cur_text.IndexOf(' ', start);
            var end = cur_text.IndexOf('>');

            if (separator == -1 || end == -1) break;

            var key = cur_text.Substring(start + 1, separator - start - 1);
            var flag = true;
            if (key.IndexOf('!') != -1)
            {
                key = key.Replace("!", "");
                flag = false;
            }
            if (!GlobalVariables.instance.vars.ContainsKey(key))
                Debug.LogError(key);

            var sentense = "";
            if (GlobalVariables.instance.vars.ContainsKey(key) && (flag && GlobalVariables.instance.vars[key] || !flag && !GlobalVariables.instance.vars[key]))
                sentense = cur_text.Substring(separator + 1, end - separator - 1);
            cur_text = cur_text.Replace(cur_text.Substring(start, end - start + 1), sentense);
        }

        while (cur_text.IndexOf('{') != -1)
        {
            var start = cur_text.IndexOf('{');
            var separator = cur_text.IndexOf(' ', start);
            var end = cur_text.IndexOf('}');

            if (separator == -1 || end == -1) break;

            var url = cur_text.Substring(start + 1, separator - start - 1);

            var action = "";
            // action
            if (cur_text.Substring(separator + 1, end - separator - 1).IndexOf('&') != -1)
            {
                var start_action = cur_text.IndexOf('&');
                separator = cur_text.IndexOf(' ', start_action);

                action = cur_text.Substring(start_action + 1, separator - start_action - 1);
                //GameActions.instance.Invoke(action);
            }

            var name = cur_text.Substring(separator + 1, end - separator - 1);
            var link = $"<link=\"{url}\"><u><color=blue>{name}</color></u></link>";
            cur_text = cur_text.Replace(cur_text.Substring(start, end - start + 1), link);

            if (action != "")
                _actionTexts.Add(name, action);

        }

        while (cur_text.IndexOf('[') != -1)
        {
            var start = cur_text.IndexOf('[');
            var separator = cur_text.IndexOf(' ', start);
            var end = cur_text.IndexOf(']');

            if (separator == -1 || end == -1) break;

            var url = cur_text.Substring(start + 1, separator - start - 1);
            var name = cur_text.Substring(separator + 1, end - separator - 1);
            var link = $"<link=\"!{url}\"><u><color=blue>{name}</color></u></link>";
            cur_text = cur_text.Replace(cur_text.Substring(start, end - start + 1), link);
        }

        return cur_text;
    }
}
