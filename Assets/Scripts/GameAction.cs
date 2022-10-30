
public class GameAction
{
    public string id;
    public string target;
    public string var;
    public string description;
    public string location;
    public string condition;

    public bool CanUse()
    {
        if (condition == "") return true;

        var flag = condition.IndexOf('!') == -1;
        var con = condition.Replace("!", "");

        return GlobalVariables.Instance.vars.ContainsKey(con) && flag && GlobalVariables.Instance.vars[con]
        || !flag && (!GlobalVariables.Instance.vars.ContainsKey(con) || !GlobalVariables.Instance.vars[con]);
    }
}
