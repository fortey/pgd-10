using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameActions : Singleton<GameActions>
{
    private Dictionary<string, Action> _events;
    private Dictionary<string, GameAction> _actions = new Dictionary<string, GameAction>();

    public override void Awake()
    {
        base.Awake();

        _events = new Dictionary<string, Action>();
        _events.Add("takeKnife", TakeKnife);

        if (File.Exists(Application.dataPath + "/actions.txt"))
        {
            StreamReader sr = new StreamReader(Application.dataPath + "/actions.txt");
            var text = sr.ReadToEnd();
            sr.Close();
            ParseText(text);
        }
        else
        {
            TextAsset theTextFile = Resources.Load<TextAsset>("actions");
            if (theTextFile != null)
            {
                ParseText(theTextFile.text);
            }
        }
    }

    public void Invoke(string action)
    {
        if (_actions.ContainsKey(action))
            _events[action]();
        else
            Debug.LogError(action);
    }

    private void TakeKnife()
    {

    }

    public void TakeItem(Entity item)
    {
        GlobalVariables.Instance.vars[item.id] = true;
        Inventory.Instance.AddItem(item);
    }

    private void ParseText(string text)
    {
        var lines = text.Split('\n');


        var id = "";

        var keys = new string[] { "target", "var", "description", "location" };
        var keysValue = new Dictionary<string, string>();
        foreach (var key in keys)
        {
            keysValue.Add(key, "");
        }

        foreach (var line in lines)
        {
            if (line.IndexOf('#') != -1)
            {
                if (id != "")
                {
                    _actions.Add(id, new GameAction() { id = id, target = keysValue["target"], var = keysValue["var"], description = keysValue["description"], location = keysValue["location"] });
                }

                var start = line.IndexOf('#');

                id = line.Substring(start + 1).Trim();

                foreach (var key in keys)
                {
                    keysValue[key] = "";
                }
            }
            else if (id != "")
            {
                var cur_line = line;
                foreach (var key in keys)
                {
                    var start = line.IndexOf($"{key}:");
                    if (start != -1)
                    {
                        keysValue[key] = line.Substring(start + key.Length + 1).Trim();
                    }
                }
            }
        }
    }
}
