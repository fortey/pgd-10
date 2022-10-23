using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalVariables : Singleton<GlobalVariables>
{
    [SerializeField] private Variables _variables;
    public Dictionary<string, bool> vars;

    public override void Awake()
    {
        base.Awake();
        vars = _variables.vars.ToDictionary(v => v.name, v => v.value);
    }
}
