using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class GameActions : Singleton<GameActions>
{
    private Dictionary<string, Action> _events;
    private List<GameAction> _actions = new List<GameAction>();

    [SerializeField] private TextMeshProUGUI _actionDescription;
    //private GameAction _currentAction;
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
        if (_events.ContainsKey(action))
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

        var keys = new string[] { "target", "var", "description", "location", "condition" };
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
                    _actions.Add(new GameAction() { id = id, target = keysValue["target"], var = keysValue["var"], description = keysValue["description"], location = keysValue["location"], condition = keysValue["condition"] });
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

    public void OnLinkSelected(string url)
    {
        var currentItem = Inventory.Instance.selectedItem;
        if (currentItem == "") return;
        var target = url.Replace("!", "");
        var action = _actions.FirstOrDefault(a => a.id == currentItem && a.target == target);
        if (action != null && action.CanUse())
        {
            //_currentAction = action;
            _actionDescription.text = action.description;
        }
    }

    public void OnLinkDeselected()
    {
        _actionDescription.text = "";
        //_currentAction = null;
    }

    public void UseItem(string url, string item)
    {
        var action = _actions.FirstOrDefault(a => a.id == item && a.target == url);
        if (action != null && action.CanUse())
        {
            var value = action.var.IndexOf('!') == -1;
            var variable = action.var.Replace("!", "");
            GlobalVariables.Instance.vars[variable] = value;
            TextManager.Instance.GoTo(action.location);
        }
    }
}
