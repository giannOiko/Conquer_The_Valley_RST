﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum UnitState
{
    Idle,
    Move,
    MoveToResource,
    Gather,
    MoveToUpgrade,
    Upgrade,
    MoveToEnemy,
    Attack
}

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public UnitState state;

    public int curHp;
    public int maxHp;

    public double minAttackDamage;
    public double maxAttackDamage;

    public double attackRate;
    private double lastAttackTime;

    public double attackDistance;

    public float pathUpdateRate = 1.0f;
    private float lastPathUpdateTime;

    public int gatherAmount;
    public float gatherRate;
    private float lastGatherTime;
    public float updateRate;
    private float lastUpdateTime;

    public ResourceSource curResourceSource;
    private Unit curEnemyTarget;

    private Vector3 scaleChange;


    [Header("Components")]
    public GameObject selectionVisual;
    private NavMeshAgent navAgent;
    public UnitHealthBar healthBar;

    private Player player;
    public int typeUnit; //0 for villager, 1 for miner etc.

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        scaleChange = new Vector3(+0.01f, +0.01f, +0.01f);
    }

    void Start ()
    {
        navAgent = GetComponent<NavMeshAgent>();

        SetState(UnitState.Idle);
    }

    void SetState (UnitState toState)
    {
        state = toState;

        if(toState == UnitState.Idle)
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }
    }

    void Update ()
    {
        switch(state)
        {
            case UnitState.Move:
            {
                MoveUpdate();
                break;
            }
            case UnitState.MoveToResource:
            {
                MoveToResourceUpdate();
                break;
            }
            case UnitState.Gather:
            {
                GatherUpdate();
                break;
            }
            case UnitState.MoveToUpgrade:
            {
                MoveToUpgradeUpdate();
                break;
            }
            case UnitState.Upgrade:
            {
                UpgradeUpdate();
                break;
            }
            case UnitState.MoveToEnemy:
            {
                MoveToEnemyUpdate();
                break;
            }
            case UnitState.Attack:
            {
                AttackUpdate();
                break;
            }
        }
    }

    void MoveUpdate ()
    {
        if(Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Idle);
    }

    void MoveToResourceUpdate ()
    {
        if(curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if(Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Gather);
    }

    void GatherUpdate ()
    {
        if(curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        LookAt(curResourceSource.transform.position);

        if(Time.time - lastGatherTime > gatherRate)
        {
            lastGatherTime = Time.time;
            curResourceSource.GatherResource(gatherAmount, player);
        }
    }

    void MoveToUpgradeUpdate()
    {
        if(Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Upgrade);
    }

    void UpgradeUpdate()
    {
        if (this.minAttackDamage > 5)
            SetState(UnitState.Idle); 

        if(Time.time - lastUpdateTime > updateRate)
        {
            lastUpdateTime = Time.time;

            this.minAttackDamage += 0.1;
            this.maxAttackDamage += 0.1;
            this.attackDistance +=0.01; 
            gameObject.transform.localScale += scaleChange;
        }
    }


    void MoveToEnemyUpdate ()
    {
        if(curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if(Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
            navAgent.isStopped = false;
            navAgent.SetDestination(curEnemyTarget.transform.position);
        }

        if(Vector3.Distance(transform.position, curEnemyTarget.transform.position) <= attackDistance)
            SetState(UnitState.Attack);
    }

    void AttackUpdate ()
    {
        if(curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if(!navAgent.isStopped)
            navAgent.isStopped = true;

        // attack every 'attackRate' seconds
        if(Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            curEnemyTarget.TakeDamage(Random.Range((int)minAttackDamage, (int)maxAttackDamage + 1));
        }

        LookAt(curEnemyTarget.transform.position);

        // if we're too far away, move towards the enemy
        if(Vector3.Distance(transform.position, curEnemyTarget.transform.position) > attackDistance)
            SetState(UnitState.MoveToEnemy);
    }

    public void TakeDamage (int damage)
    {
        curHp -= damage;

        if(curHp <= 0)
            Die();

        healthBar.UpdateHealthBar(curHp, maxHp);
    }

    void Die ()
    {
        if(this.typeUnit == 3)
            player.enemyunits.Remove(this);
            
        else
        {
            player.units.Remove(this);
            GameUI.instance.UpdateUnitCountText(player.units.Count);
        }

        Destroy(gameObject);
    }

    public void MoveToPosition (Vector3 pos)
    {
        SetState(UnitState.Move);

        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    public void GatherResource (ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        SetState(UnitState.MoveToResource);

        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    public void UpgradeStats(Vector3 pos)
    {
        SetState(UnitState.MoveToUpgrade);

        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    public void UnitAttack(Unit target)
    {
        curEnemyTarget = target;
        SetState(UnitState.MoveToEnemy);
    }

    public void ToggleSelectionVisual (bool selected)
    {
        if(selectionVisual != null)
            selectionVisual.SetActive(selected);
    }

    void LookAt (Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}