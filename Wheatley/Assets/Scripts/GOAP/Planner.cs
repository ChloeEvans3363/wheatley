using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour
{
    [SerializeField]
    Goal[] goals;

    [SerializeField]
    Action[] actions;

    private Goal activeGoal;
    private Action activeAction;

    public Goal mainGoal;

    WorldState test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public List<Action> plan(WorldState initialState)
    {
        List<WorldState> states = new List<WorldState>();
        List<Action> actions = new List<Action>();

        List<Action> currentPlan = new List<Action>();
        states.Add(initialState);
        WorldState currentState = initialState;
        float cost = 0;
        int currentDepth = 0;

        // Do some kind of while statement to check
        // if the current world state has the player
        // at the end
        while (!currentState.GoalAchieved())
        {

        }

        return null;
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
            if (!goal.Satisfied(test))
                continue;

            // Is it a better priority?
            if ((bestGoal == null || goal.CalculatePriority() > bestGoal.CalculatePriority()))
                continue;

            // Find the best cost action
            Action currentAction = null;
            foreach(Action action in actions)
            {
                // Checks if the action supports the current goal
                if (!action.GetSupportedGoals().Contains(goal.GetType()))
                    continue;

                // Found a suitable action
                if(currentAction == null || action.GetCost(test) < currentAction.GetCost(test))
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
                activeAction.OnActivated(test);

        } // No change
        else if(activeGoal == bestGoal)
        {
            // Action changed?
            if(activeAction != bestAction)
            {
                activeAction.OnDeactived();
                activeAction = bestAction;
                activeAction.OnActivated(test);
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
                activeAction.OnActivated(test);
        }
    }
}
