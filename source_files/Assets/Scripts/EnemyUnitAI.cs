using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitAI : MonoBehaviour
{

    public float checkRate = 1.0f;
    public float checkRange;
    public float moveRate;

    public LayerMask unitLayerMask;
    private Unit unit;
    
    float TimeRemaining;

    
    void Awake ()
    {
        unit = GetComponent<Unit>();     
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Check", 0.0f, checkRate);
    }


    void Check ()
    {
        TimeRemaining = GameObject.Find("Player").GetComponent<Timer>().getTimeRemaining();
        //Debug.Log(TimeRemaining);
        // check if we have nearby enemies - if so, attack them, else move around, and closer by 100 secs
        if(unit.state != UnitState.Attack && unit.state != UnitState.MoveToEnemy)
        {
            Unit potentialEnemy = CheckForNearbyEnemies();

            if(potentialEnemy != null)
            {
                unit.UnitAttack(potentialEnemy);
            }
            else
            {
                if(unit.state == UnitState.Idle)
                {   if(TimeRemaining<100)
                    {
                        unit.MoveToPosition(getMovingPositions(65.0f));
                    }
                    else if(TimeRemaining<200)
                    {
                        unit.MoveToPosition(getMovingPositions(43.0f));
                    }
                    else if(TimeRemaining < 300)
                    {
                        unit.MoveToPosition(getMovingPositions(-21.5f));
                    }
                    else if(TimeRemaining<400)
                    {
                        unit.MoveToPosition(getMovingPositions(0.0f));
                    }
                    else if(TimeRemaining < 500)
                    {
                        unit.MoveToPosition(getMovingPositions(-20.0f));
                    }
                    else
                        unit.MoveToPosition(getMovingPositions(-37.0f));
                        
                }
            } 
            
        }
    }
    Unit CheckForNearbyEnemies ()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, checkRange, Vector3.up, unitLayerMask);

        GameObject closesthit = null;
        float closestDist = 0.0f;

        for(int x = 0; x < hits.Length; x++)
        {
            // skip if this is our team
            if(hits[x].collider.CompareTag("EnemyUnit"))
                continue;

            //get to the closest of the sphereCast hits.
            if(!closesthit || Vector3.Distance(transform.position, hits[x].transform.position) < closestDist)
            {
                closesthit = hits[x].collider.gameObject;
                closestDist = Vector3.Distance(transform.position, hits[x].transform.position);
            }
        }

        if(closesthit != null)
            return closesthit.GetComponent<Unit>();
        else
            return null;
    }

    private Vector3 getMovingPositions(float xValue)
    {
        Vector3 cur_pos = gameObject.transform.position;

        Vector3 randomPosition = new Vector3(
        Random.Range(-71.0f, xValue),
        cur_pos.y,
        Random.Range(-30, 28)
    );
    return randomPosition;
    }
    
}
