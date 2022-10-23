using UnityEngine;

[CreateAssetMenu(fileName = "Variables", menuName = "pgd-10/Variables", order = 0)]
public class Variables : ScriptableObject
{
    public VarItem[] vars;
}

[System.Serializable]
public class VarItem
{
    public string name;
    public bool value;
}
