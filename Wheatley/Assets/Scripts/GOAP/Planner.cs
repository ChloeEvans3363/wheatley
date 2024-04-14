using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour
{
    [SerializeField]
    Goal[] goals;

    [SerializeField]
    Action[] actions;

    Goal activeGoal;
    Action activeAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Goal bestGoal = null;
        Action bestAction = null;

        // find the highest priority goal that can be actived

        foreach(Goal goal in goals)
        {
            // Checks if the goal can run
            // If not then skip it
            if (!goal.CanRun())
                continue;

            // Is it a better priority?
            if ((bestGoal == null || goal.CalculatePriority() > bestGoal.CalculatePriority()))
                continue;

            // Find the best cost action
            Action currentAction = null;
            foreach(Action action in actions)
            {
                if (!action.GetSupportedGoals().Contains(goal.GetType()))
                    continue;

                // Found a suitable action
                if(currentAction == null || action.GetCost() < currentAction.GetCost())
                    currentAction = action;
            }

            // Did we find an action?
            if(currentAction != null)
            {
                bestGoal = goal;
                bestAction = currentAction;
            }

        }

        // if no current goal
        if(activeGoal == null && bestGoal != null)
        {
            activeGoal = bestGoal;
            activeAction = bestAction;

            if (activeGoal != null)
                activeGoal.OnGoalActivated();
            if (activeAction != null)
                activeAction.OnActivated();

        } // No change
        else if(activeGoal == bestGoal)
        {
            // Action changed?
            if(activeAction != bestAction)
            {
                activeAction.OnDeactived();
                activeAction = bestAction;
                activeAction.OnActivated();
            }
        } // new goal or no valid goal?
        else if(activeGoal != bestGoal)
        {
            activeGoal.OnGoalDeactivated();
            activeAction.OnDeactived();

            activeGoal = bestGoal;
            activeAction = bestAction;

            if(activeGoal != null)
                activeGoal.OnGoalActivated();
            if (activeAction != null)
                activeAction.OnActivated();
        }
    }
}
