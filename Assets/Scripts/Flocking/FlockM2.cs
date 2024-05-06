using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockM2 : MonoBehaviour
{
    float speed;
    bool turning = false;
    [SerializeField][Range(1, 10)] private float _fleeRadius = 3;
    [SerializeField][Range(1, 20)] private float _detectionRadius = 10;
    [SerializeField][Range(1, 10)] private float _speed = 1;
    private bool _fleeing = false;
    private Vector3 _fleePoint;
    private float _timer;

    void Start()
    {
        speed = Random.Range(FlockManager.Instance.MinSpeed, FlockManager.Instance.MaxSpeed);
        _timer = 0;
    }


    void Update()
    {
        if (_fleeing)
        {
            Flee();
            _timer += Time.deltaTime;
            if (_timer > 3)
            {
                _timer = 0;
                _fleeing = false;
            }
        }
        else
        {
            Flocking();
        }
    }

    public void SetFlee(Vector3 pos)
    {
        _fleePoint = pos;
        _fleeing = true;
    }

    private void Flee()
    {
        var distance = Vector3.Distance(_fleePoint, transform.position);
        Debug.Log($"{distance}  {_detectionRadius}", this);
        if (distance < _detectionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - _fleePoint).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * _fleeRadius;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(fleeDirection),
                FlockManager.Instance.RotationSpeed * Time.deltaTime);

            Move(fleeDirection);
        }
        else
        {
            _fleeing = false;
        }
    }

    void Move(Vector3 vector)
    {
        var vel = _speed * Time.deltaTime * vector;
        vel.y = 0;
        Debug.Log(vel);
        transform.position += vel;
    }

    private void Flocking()
    {
        Bounds b = new(FlockManager.Instance.transform.position, FlockManager.Instance.Boundary * 2.0f);

        if (!b.Contains(transform.position))
        {

            turning = true;
        }
        else
        {

            turning = false;
        }

        if (turning)
        {

            Vector3 direction = FlockManager.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                FlockManager.Instance.RotationSpeed * Time.deltaTime);
        }
        else
        {


            if (Random.Range(0, 100) < 10)
            {

                speed = Random.Range(FlockManager.Instance.MinSpeed, FlockManager.Instance.MaxSpeed);
            }


            if (Random.Range(0, 100) < 10)
            {
                ApplyRules();
            }
        }

        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
    }

    private void ApplyRules()
    {

        GameObject[] gos;
        gos = FlockManager.Instance.AllNpcs;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;

        float gSpeed = 0.01f;
        float mDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {

            if (go != this.gameObject)
            {

                mDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (mDistance <= FlockManager.Instance.NeighbourDistance)
                {

                    vCentre += go.transform.position;
                    groupSize++;

                    if (mDistance < 1.0f)
                    {

                        vAvoid += this.transform.position - go.transform.position;
                    }

                    FlockM2 anotherFlock = go.GetComponent<FlockM2>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {

            vCentre = vCentre / groupSize + (FlockManager.Instance.GoalPos - this.transform.position);
            speed = gSpeed / groupSize;

            if (speed > FlockManager.Instance.MaxSpeed)
            {

                speed = FlockManager.Instance.MaxSpeed;
            }

            Vector3 direction = (vCentre + vAvoid) - transform.position;
            if (direction != Vector3.zero)
            {

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    FlockManager.Instance.RotationSpeed * Time.deltaTime);
            }
        }
    }
}
