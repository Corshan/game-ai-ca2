using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gentics
{
    public class DNA
    {
        private List<Gene> _genes;
        public List<Gene> Genes => _genes;
        public DNA()
        {
            _genes = new List<Gene>();
        }
    }

    public struct Gene {
        public Vector3 _pos;
        public float _distanceMoved;
    }
}