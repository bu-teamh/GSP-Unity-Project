using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;   

public class EnemyAITest : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    //public List<Transform> trashList;
    //private Vector3 nearestTrash = Vector3.zero;

    public LayerMask _groundMask;
    public LayerMask _playerMask;
    //public LayerMask _trashMask;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float _attackDelay;
    bool _alreadyAttacked;

    public float _sightRange, _attackRange;
    public bool _inSight, _inRange, _inSightTrash, _inRangeTrash;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

       // foreach(var TrashObject in GameObject.FindGameObjectsWithTag("Trash"))
        //{
        //    trashList.Add(TrashObject.transform);
        //}
       // print(trashList.Count);
    }

    // Update is called once per frame
    private void Update()
    {
        _inSight = Physics.CheckSphere(transform.position, _sightRange, _playerMask);
        _inRange = Physics.CheckSphere(transform.position, _attackRange, _playerMask);
       
        //_inRangeTrash = Physics.CheckSphere(transform.position, _attackRange, _trashMask);
        //_inSightTrash = Physics.CheckSphere(transform.position, _sightRange, _trashMask);

        //Collider[] TrashInSight = Physics.OverlapSphere(transform.position, _sightRange, _trashMask);
        //if(TrashInSight.Length > 0)
        //{
        //    nearestTrash = Vector3.zero;
        //    foreach (var trash in TrashInSight)
         //   {
         //       Vector3 tempDis = transform.position - trash.transform.position;
         //       if (nearestTrash.magnitude != 0)
         //       {
          //          if (tempDis.magnitude < nearestTrash.magnitude)
          //          {
          //              nearestTrash = tempDis;
          //          }
          //      }
          //      else if (nearestTrash.magnitude == 0)
          //      {
          //          nearestTrash = tempDis;
          //      }
           // }
       // }
        //print(nearestTrash);

        if(!_inSight && !_inRange)
        {
            Patrolling();
        }
        if(_inSight && !_inRange)
        {
            Chasing();
        }
        if(_inSight && _inRange)
        {
            Attacking();
        }


        //if(_inSightTrash && !_inRangeTrash && !_inSight && !_inRange)
        //{
        //    MoveToTrash();
        //}
        //if(_inSightTrash && _inRangeTrash && !_inSight && !_inRange)
        //{
         //   CollectTrash(); 
        //}

    }

    //private void MoveToTrash()
    //{
    //   agent.SetDestination(nearestTrash);
    //}

    //private void CollectTrash()
    //{
     //   print("Trash Collected");
    //    
    //}
    private void Patrolling()
    {
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, _groundMask ))
        {
            walkPointSet = true;
        }
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        print("attacking");

        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!_alreadyAttacked)
        {
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
}
