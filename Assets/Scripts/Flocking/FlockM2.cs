using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockM2 : MonoBehaviour
{
    float speed;
    bool turning = false;

    void Start()
    {
        speed = Random.Range(FlockManager.Instance.MinSpeed, FlockManager.Instance.MaxSpeed);
    }


    void Update()
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
