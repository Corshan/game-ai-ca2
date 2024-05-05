using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Gentics
{
    public class Brain : MonoBehaviour
    {
        [SerializeField][Range(1, 20)] private float _speed = 10;
        private float _timeAlive;
        public float TimeAlive => _timeAlive;
        private float _distanceMoved;
        public float DistanceMoved => _distanceMoved;
        private Vector3 startPos;
        public Vector3 StartPos => startPos;
        private DNA dna;
        public DNA DNA => dna;
        private int _index;
        private bool alive = true;
        private Vector3 pos = Vector3.zero;
        private float _timer;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("death")) alive = false;
        }

        void Start()
        {
            dna = new DNA();
            pos = ChoosePoint();
            _timer = 0;
        }

        public void Init()
        {
            dna = new DNA();

            _timeAlive = 0;
            alive = true;
            _distanceMoved = 0;
            _index = 0;
            startPos = transform.position;
            _timer = 0;
        }


        void Update()
        {
            var positions = dna.Genes;

            _timer += Time.deltaTime;

            if (_timer > 1)
            {
                pos = ChoosePoint();
                _timer = 0;
            }

            Move(pos);
        }

        void Move(Vector3 vector)
        {
            transform.position += _speed * Time.deltaTime * vector;
        }

        Vector3 ChoosePoint()
        {
            int rand = Random.Range(0, 100);
            int distance = 1;

            // print(rand);

            Vector3 pos = Vector3.zero;
            switch (rand)
            {
                case < 25:
                    pos = transform.forward;
                    break;
                case < 50:
                    pos = transform.right;
                    break;
                case < 75:
                    pos = transform.forward * -1;
                    break;
                case < 100:
                    pos = transform.right * -1;
                    break;

            }

            // dna.Genes.Add(pos);
            return pos;
        }
    }
}