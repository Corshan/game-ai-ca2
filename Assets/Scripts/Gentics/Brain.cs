using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Gentics
{
    public class Brain : MonoBehaviour
    {
        public DNA dna;
        public GameObject eyes;
        (bool left, bool forward, bool right) seeWall;
        public float ammoFound = 0;
        LayerMask ingore = 6;
        bool canMove = false;

        public void Innit()
        {
            dna = new DNA();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ammo"))
            {
                ammoFound++;
                other.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            seeWall = (false, false, false);
            bool left = false;
            bool front = false;
            bool right = false;
            RaycastHit hit;
            canMove = true;
            Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 1f, Color.red);

            if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out  hit, 1f, ~ingore))
            {
                if (hit.collider.CompareTag("wall"))
                {
                    front = true;
                    canMove = false;
                }
            }
            if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.right, out hit, 1f, ~ingore))
            {
                if (hit.collider.CompareTag("wall"))
                {
                    right = true;
                }
            }
            if (Physics.SphereCast(eyes.transform.position, 0.1f, -eyes.transform.right, out hit, 1f, ~ingore))
            {
                if (hit.collider.CompareTag("wall"))
                {
                    left = true;
                }
            }

            seeWall = (left, front, right);
        }

        void FixedUpdate()
        {
            this.transform.Rotate(0, dna.genes[seeWall], 0);
            if(canMove) this.transform.Translate(0, 0, 0.1f);
        }
    }
}