using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class FlockManager : MonoBehaviour
{

    public static FlockManager Instance;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _numNpc = 20;
    [SerializeField] private GameObject[] allNpcs;
    public GameObject[] AllNpcs => allNpcs;
    [SerializeField] private Vector3 _boundary = new(5.0f, 5.0f, 5.0f);
    public Vector3 Boundary => _boundary;
    [SerializeField] private Vector3 _goalPos = Vector3.zero;
    public Vector3 GoalPos => _goalPos;

    [Header("Npc Settings")]
    [Range(0.0f, 5.0f)][SerializeField] private float _minSpeed;
    public float MinSpeed => _minSpeed;
    [Range(0.0f, 5.0f)][SerializeField] private float _maxSpeed;
    public float MaxSpeed => _maxSpeed;
    [Range(1.0f, 10.0f)][SerializeField] private float _neighbourDistance;
    public float NeighbourDistance => _neighbourDistance;
    [Range(1.0f, 5.0f)][SerializeField] private float rotationSpeed;
    public float RotationSpeed => rotationSpeed;

    [Header("Nav mesh settings")]
    [SerializeField][Range(1, 5)] private float _distance = 1;
    private NavMeshAgent _agent;
    private List<GameObject> _waypoints;
    private GameObject _target;
    private float _timer = 0;
    [Range(1, 60)] public int timerTime = 10;
    private Animator _anim;
    private GameObject _npcParent;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        allNpcs = new GameObject[_numNpc];
        _npcParent = new GameObject("NPCs");

        for (int i = 0; i < _numNpc; ++i)
        {

            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-_boundary.x, _boundary.x),
                0,
                Random.Range(-_boundary.y, _boundary.y));

            allNpcs[i] = Instantiate(_prefab, pos, Quaternion.identity, _npcParent.transform);
        }

        _goalPos = this.transform.position;

        _agent = GetComponent<NavMeshAgent>();
        _waypoints = GameObject.FindGameObjectsWithTag("WP").ToList();

        _target = _waypoints[Random.Range(0, _waypoints.Count)];
        _agent.speed = Random.Range(_minSpeed, _maxSpeed);

        _anim = GetComponent<Animator>();
        _anim.SetBool("isWalking", true);
    }


    void Update()
    {
        if (Random.Range(0, 100) < 10)
        {

            _goalPos = this.transform.position + new Vector3(
                Random.Range(-_boundary.x, _boundary.x),
                0,
                Random.Range(-_boundary.y, _boundary.y));
        }

        if (Vector3.Distance(_agent.transform.position, _target.transform.position) < _distance)
        {
            _timer += Time.deltaTime;
            _anim.SetBool("isWalking", false);

            if (_timer > timerTime)
            {
                _timer = 0;
                _target = _waypoints[Random.Range(0, _waypoints.Count)];
                _anim.SetBool("isWalking", true);
            }
        }

        _agent.SetDestination(_target.transform.position);
    }
}