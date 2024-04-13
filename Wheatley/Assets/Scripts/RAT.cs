using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAT : MonoBehaviour
{
    // This class is going to contain the finite state machine
    // That switches between actions and moving

    public enum AIStates
    {
        Move,
        Action
    }

    public AIStates state;

    public List<Action> actions = new List<Action>();

    // Start is called before the first frame update
    void Start()
    {
        state = AIStates.Move;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case AIStates.Move:
                break;

            case AIStates.Action:
                break;
        }
    }
}
