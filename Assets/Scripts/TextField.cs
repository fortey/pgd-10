using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextField : MonoBehaviour, IPointerClickHandler
{
    public Func<string, Entity> GetPart;
    public Action<string> ShowPopup;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] private Button _actionButton;

    private Dictionary<string, string> _actionTexts = new Dictionary<string, string>();

    private Entity _lastEntity;


    public void SetText(string url)
    {
        _lastEntity = GetPart(url);
        _nameLabel.text = _lastEntity.name;
        _textMeshPro.text = ParseText(_lastEntity.text);
        _actionButton.gameObject.SetActive(_lastEntity.takeable);
    }

    public void RefreshText()
    {
        SetText(_lastEntity.id);
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
                    GameActions.Instance.Invoke(_actionTexts[linkInfo.GetLinkText()]);
                SetText(url);
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
            // if (!GlobalVariables.Instance.vars.ContainsKey(key))
            //     Debug.LogError(key);

            var sentense = "";
            if (GlobalVariables.Instance.vars.ContainsKey(key) && flag && GlobalVariables.Instance.vars[key] || !flag && (!GlobalVariables.Instance.vars.ContainsKey(key) || !GlobalVariables.Instance.vars[key]))
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
            var link = $"<link=\"{url}\"><color=blue>{name}</color></link>";
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
            var link = $"<link=\"!{url}\"><color=blue>{name}</color></link>";
            cur_text = cur_text.Replace(cur_text.Substring(start, end - start + 1), link);
        }

        return cur_text;
    }

    public void OnClickAction()
    {
        GameActions.Instance.TakeItem(_lastEntity);
    }
}
