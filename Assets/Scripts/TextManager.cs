using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TextField _textField;
    [SerializeField] private TextField _popupTextField;
    [SerializeField] private Popup _popup;
    //private string _text;
    private Dictionary<string, Entity> _parts = new Dictionary<string, Entity>();
    //private Dictionary<string, Entity> _itemsParts = new Dictionary<string, Entity>();

    void Start()
    {
        _textField.GetPart = GetPart;
        _textField.ShowPopup = ShowPopup;

        _popupTextField.GetPart = GetPart;
        _popupTextField.ShowPopup = ShowPopup;

        TextAsset theTextFile = Resources.Load<TextAsset>("text");
        if (theTextFile != null)
        {
            ParseText(theTextFile.text);
        }

        theTextFile = Resources.Load<TextAsset>("items");
        if (theTextFile != null)
        {
            ParseText(theTextFile.text);
        }

        _textField.SetText("start");
    }

    private void ParseText(string text)
    {
        var lines = text.Split('\n');

        var key = "";
        var name = "";
        var takeable = false;
        var builder = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.IndexOf('#') != -1)
            {
                if (key != "")
                {
                    _parts.Add(key, new Entity() { id = key, name = name, text = builder.ToString(), takeable = takeable });
                }

                var start = line.IndexOf('#');
                var separator = line.IndexOf(' ', start);

                if (separator == -1) break;
                key = line.Substring(start + 1, separator - start - 1).Trim();
                name = line.Substring(separator + 1).Trim();

                if (name.IndexOf("takeable") != -1)
                {
                    takeable = true;
                    name = name.Replace("takeable", "").Trim();
                }
                else takeable = false;

                builder.Clear();
            }
            else if (key != "")
            {
                var cur_line = line;
                // while (cur_line.IndexOf('{') != -1)
                // {
                //     var start = cur_line.IndexOf('{');
                //     var separator = cur_line.IndexOf(' ', start);
                //     var end = cur_line.IndexOf('}');

                //     if (separator == -1 || end == -1) break;

                //     var url = cur_line.Substring(start + 1, separator - start - 1);
                //     var name = cur_line.Substring(separator + 1, end - separator - 1);
                //     var link = $"<link=\"{url}\"><u><color=blue>{name}</color></u></link>";
                //     cur_line = cur_line.Replace(cur_line.Substring(start, end - start + 1), link);
                // }
                builder.AppendLine(cur_line);
            }
        }
    }

    private Entity GetPart(string key)
    {
        if (_parts.ContainsKey(key))
            return _parts[key];
        else return new Entity();
    }

    private void ShowPopup(string key)
    {
        if (_parts.ContainsKey(key))
            _popup.ShowText(key);
    }


}

public struct Entity
{
    public string id;
    public string name;
    public string text;
    public bool takeable;
}
