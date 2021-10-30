using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowerPlayer : MonoBehaviour {

    public Transform target;
    UnityEngine.AI.NavMeshAgent agent;

    float myCollosionRadius;
    float targetCollisionRadius;

    [Range(1.0f, 10.0f)]
    public float attackDistanceThreshold = 1.0f;

    float nextAttackTime;
    float timeBetweenAttacks = 1;                      //  1 second.

    // Use this for initialization
	void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        myCollosionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        StartCoroutine(UpdatePath());
    }
	
	// Update is called once per frame
	void Update () {
        if(Time.time > nextAttackTime)
        {
            float sqrMag = (target.position - transform.position).sqrMagnitude;

            float sqrAttackRange = Mathf.Pow(myCollosionRadius + targetCollisionRadius + attackDistanceThreshold, 2);

            if(sqrMag < sqrAttackRange)
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                Debug.Log("Attack!");
            }
        }		
	}

    IEnumerator UpdatePath()
    {
        while(target != null)
        {
            //Vector3 targetPosition = new Vector3(target.position.x, 0f, target.position.z);
            //agent.SetDestination(targetPosition);

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Vector3 targetPosition = target.position - directionToTarget * (myCollosionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
            agent.SetDestination(targetPosition);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
