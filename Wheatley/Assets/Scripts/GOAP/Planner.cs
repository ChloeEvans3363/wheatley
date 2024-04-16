using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Planner
{
    Goal[] goals;

    Action[] actions;

    private Goal activeGoal;
    private Action activeAction;

    // Just using this right now to prevent errors
    // while I redo the goap system
    WorldState test;

    // Right now the goals and actions are set by having
    // all the goals and actions on the same prefab
    // I may change this if there is time/ I find a better way
    private void Awake()
    {
        //goals = GetComponents<Goal>();
        //actions = GetComponents<Action>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static Action[] plan(WorldState state, int maxDepth)
    {
        WorldState[] states = new WorldState[maxDepth + 1];
        Action[] actions = new Action[maxDepth];

        Action[] currentPlan = new Action[maxDepth];
        states[0] = state;
        WorldState currentState = state;
        float cost = 0;
        float bestContentment = float.MinValue;
        int currentDepth = 0;

        // Do some kind of while statement to check
        // if the current world state has the player
        // at the end
        //!currentState.GoalAchieved()
        while (currentDepth >= 0)
        {
            // This is the part that adds the highest contentment plan
            // to the current plan list
            if(currentDepth >= maxDepth)
            {

                float currentContentment = states[currentDepth].GetContentment();

                /*
                Debug.Log("Reached Leaf Node with Utility: " + currentContentment);
                Debug.Log("Best Utility: " + bestContentment);
                Debug.Log("Current Plan:");
                foreach (Action action in actions)
                    Debug.Log(action);
                Debug.Log(states[currentDepth]);
                */

                if (currentContentment > bestContentment)
                {
                    bestContentment = currentContentment;

                    actions.CopyTo(currentPlan, 0);

                    /*
                    Debug.Log("Updated Plan:");
                    foreach (Action action in currentPlan)
                        Debug.Log(action);
                    Debug.Log("");
                    */
                }
                currentDepth -= 1;
            }

            // This is really the main part of GOAP
            else
            {
                Action nextAction = states[currentDepth].NextAction();

                if (nextAction != null)
                {
                    states[currentDepth + 1] = nextAction.OnActivated(states[currentDepth]);
                    actions[currentDepth] = nextAction;
                    currentDepth += 1;
                    //Debug.Log("action " + nextAction);
                }
                else
                {
                    currentDepth -= 1;
                    //Debug.Log("no action");
                }
                //Debug.Log("Current depth: " + currentDepth);
            }
        }
        Debug.Log("search end");

        return currentPlan;
    }

    // This is another version of goap that will probably not stay
    // Update is called once per frame
    void Update()
    {
        /*
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
         */
    }
}
