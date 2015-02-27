using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent navComponent;
    // Use this for initialization
    void Start()
    {
        //Assigns the NavMeshAgent of the ship to navComponent.
        navComponent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks to see if there is a valid target, and if there is, sets the destination of the PlayerShip.
        if (target)
        {
            navComponent.SetDestination(target.position);
        }
    }
    /*changeTarget is a method other objects can call to change the target variable for this object. */
    void changeTarget(Transform tar)
    {
        target = tar;
    }

    //
}