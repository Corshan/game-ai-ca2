using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

public class RavengerBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tents;
    private GameObject _targetTent;
    private Vector3 _startPos;
    private NavMeshAgent _agent;
    private Tree _tree;
    private ActionState _state;
    private Node.Status _treeStatus;
    public enum ActionState
    {
        IDLE,
        WORKING
    };
    private float _timer = 0;
    private Animator _anim;

    void Start()
    {
        _startPos = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _tree = new Tree();
        _state = ActionState.IDLE;
        _treeStatus = Node.Status.RUNNING;
        
        _tents ??= GameObject.FindGameObjectsWithTag("Tent").ToList();

        Sequence kill = new("Kill NPC");

        Leaf chooseTent = new("Choose a tent", ChooseTent);
        Leaf goToTent = new("goToTent", GoToTent);
        Leaf flee = new("Flee", Flee);

        Selector killOrSteal = new("killOrSteal");
        Leaf killNpc = new("Kill Npc", KillNpc);
        Leaf steal = new("Steal item", Steal);

        killOrSteal.AddChild(killNpc);
        killOrSteal.AddChild(steal);

        kill.AddChild(chooseTent);
        kill.AddChild(goToTent);
        kill.AddChild(killOrSteal);
        kill.AddChild(flee);

        _tree.AddChild(kill);

        _tree.PrintTree();
    }

    Node.Status Steal()
    {
        var item = _targetTent.GetComponent<TentNpc>();

        Destroy(item.Item);
        return Node.Status.SUCCESS;
    }

    Node.Status Flee()
    {
        return GoToLocation(_startPos);
    }

    Node.Status ChooseTent()
    {
        _targetTent = _tents[Random.Range(0, _tents.Count)];
        return Node.Status.SUCCESS;
    }

    Node.Status KillNpc()
    {
        var npc = _targetTent.GetComponent<TentNpc>();
        _anim.SetBool("punch", false);

        if (npc.IsNpc)
        {
            if (npc.IsAlive)
            {
                _anim.SetBool("attack", true);
                if (_timer > 3)
                {
                    npc.ChangeHealth(10);
                    _timer = 0;
                    _anim.SetBool("punch", true);
                }
                _timer += Time.deltaTime;
                return Node.Status.RUNNING;
            }
            else
            {
                _anim.SetBool("attack", false);
                return Node.Status.SUCCESS;
            }
        }
        else
        {
            return Node.Status.FAILURE;
        }
    }

    Node.Status GoToTent()
    {
        return GoToLocation(_targetTent.transform.position);
    }

    Node.Status GoToLocation(Vector3 destination)
    {

        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (_state == ActionState.IDLE)
        {
            _anim.SetBool("attack", false);
            _agent.SetDestination(destination);
            _state = ActionState.WORKING;
        }
        else if (Vector3.Distance(_agent.pathEndPosition, destination) >= 2.0f)
        {
            // _state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2.0f)
        {

            _state = ActionState.IDLE;
            _anim.SetBool("attack", true);
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    void Update()
    {
        if (_treeStatus != Node.Status.SUCCESS) _treeStatus = _tree.Process();
    }
}
