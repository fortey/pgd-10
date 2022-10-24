using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActions : Singleton<GameActions>
{
    private Dictionary<string, Action> _actions;

    public override void Awake()
    {
        base.Awake();

        _actions = new Dictionary<string, Action>();
        _actions.Add("takeKnife", TakeKnife);
    }

    public void Invoke(string action)
    {
        if (_actions.ContainsKey(action))
            _actions[action]();
        else
            Debug.LogError(action);
    }

    private void TakeKnife()
    {
        GlobalVariables.Instance.vars["knife"] = true;
        Inventory.Instance.AddItem("нож");
    }

    public void TakeItem(Entity item)
    {
        GlobalVariables.Instance.vars[item.id] = true;
        Inventory.Instance.AddItem(item.name);
    }
}
