using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    // May change to not use a string
    // Since strings are not the most efficient
    //public string name = "Action";
    
    // Cost will be calculated by A* and will be how many steps
    //public int cost;

    //public Dictionary<string, int> preconditions;
    //public Dictionary<string, int> effects;

    public virtual List<System.Type> GetSupportedGoals()
    {
        return null;
    }

    public virtual float GetCost()
    {
        return 0f;
    }

    // Consider linking a goal to the action here
    public virtual void OnActivated()
    {

    }

    // Consider unlinking a goal to the action here
    public virtual void OnDeactived()
    {

    }

    /*
public Action()
{
    preconditions = new Dictionary<string, int>();
    effects = new Dictionary<string, int>();
}


public bool IsAchievableGiven(Dictionary<string, int> conditions)
{
    foreach(KeyValuePair<string, int> p in preconditions)
    {
        if (!conditions.ContainsKey(p.Key))
            return false;
    }
    return true;
}
*/
}
