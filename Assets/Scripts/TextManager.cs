using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TextField _textField;
    //private string _text;
    private Dictionary<string, string> _parts = new Dictionary<string, string>();

    void Start()
    {
        _textField.GetPart = getPart;

        TextAsset theTextFile = Resources.Load<TextAsset>("text");
        if (theTextFile != null)
        {
            ParseText(theTextFile.text);

            _textField.SetText(_parts["start"]);
        }
    }

    private void ParseText(string text)
    {
        var lines = text.Split('\n');

        var key = "";
        var builder = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.IndexOf('#') != -1)
            {
                if (key != "")
                {
                    _parts.Add(key, builder.ToString());
                }

                key = line.Substring(line.IndexOf('#') + 1).Trim();
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

    private string getPart(string key)
    {
        if (_parts.ContainsKey(key))
            return _parts[key];
        else return "";
    }
}
