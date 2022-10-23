using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance;

    [SerializeField] private Variables _variables;
    public Dictionary<string, bool> vars;

    private void Awake()
    {
        instance = this;
        vars = _variables.vars.ToDictionary(v => v.name, v => v.value);
    }
}
