using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Planner
{

    public static Action[] plan(WorldState state, int maxDepth)
    {
        WorldState[] states = new WorldState[maxDepth + 1];
        Action[] actions = new Action[maxDepth];

        Action[] currentPlan = new Action[maxDepth];
        states[0] = state;
        float bestCost = float.MaxValue;
        float bestContentment = float.MinValue;
        int currentDepth = 0;

        while (currentDepth >= 0)
        {
            // This is the part that adds the highest contentment plan
            // to the current plan list
            if(currentDepth >= maxDepth)
            {

                float currentContentment = states[currentDepth].GetContentment();
                float currentCost = states[currentDepth].GetTotalCost(actions);

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

                    bestCost = currentCost;

                    actions.CopyTo(currentPlan, 0);

                    /*
                    Debug.Log("Updated Plan:");
                    foreach (Action action in currentPlan)
                        Debug.Log(action);
                    Debug.Log("");
                    */
                }
                else if(currentContentment == bestContentment &&
                    bestCost > currentCost)
                {
                    bestCost = currentCost;

                    actions.CopyTo(currentPlan, 0);
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
                }
                else
                {
                    currentDepth -= 1;
                }
            }
        }

        // If the contentment level is
        // greater then 0 that means
        // the end was reached
        if(bestContentment > 0)
        {
            //Debug.Log("cost: " + bestCost);
            return currentPlan;
        }
        return null;
    }
}
