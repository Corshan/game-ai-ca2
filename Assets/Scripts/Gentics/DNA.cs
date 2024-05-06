using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gentics
{
    public class DNA
    {
        public Dictionary<(bool left, bool forward, bool right), float> genes;
        int dnaLenght;

        public DNA()
        {
            genes = new Dictionary<(bool left, bool forward, bool right), float>();
            SetRandom();
        }

        public void SetRandom()
        {
            genes.Clear();
            genes.Add((false, false, false), Random.Range(-90, 91));
            genes.Add((false, false, true), Random.Range(-90, 91));
            genes.Add((false, true, true), Random.Range(-90, 91));
            genes.Add((true, true, true), Random.Range(-90, 91));
            genes.Add((true, false, false), Random.Range(-90, 91));
            genes.Add((true, false, true), Random.Range(-90, 91));
            genes.Add((false, true, false), Random.Range(-90, 91));
            genes.Add((true, true, false), Random.Range(-90, 91));
            dnaLenght = genes.Count;
        }

        public void Combine(DNA d1, DNA d2)
        {
            int i = 0;
            Dictionary<(bool left, bool forward, bool right), float> newGenes = new();

            foreach (var gene in genes)
            {
                float f = (i < dnaLenght / 2) ? d1.genes[gene.Key] : d2.genes[gene.Key];
                newGenes.Add(gene.Key, f);
                i++;
            }
            genes = newGenes;
        }

        public float GetGenes((bool left, bool forward, bool right) seeWall)
        {
            return genes[seeWall];
        }
    }
}