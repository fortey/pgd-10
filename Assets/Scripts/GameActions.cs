using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    public static GameActions instance;

    private Dictionary<string, Action> _actions;

    private void Awake()
    {
        instance = this;

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
        GlobalVariables.instance.vars["knife"] = true;
    }
}
