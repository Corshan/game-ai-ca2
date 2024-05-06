using System.Collections;                               // 1) Almost indentical to previous PopulationManager of people and colour.
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Gentics
{
    public class PopulationManager : MonoBehaviour
    {
        public GameObject _prefab;
        public Vector2[] startingPos;
        public int populationSize = 50;
        List<GameObject> population = new();
        public static float elapsed = 0;
        public float trialTime = 10;
        public float timeScale = 2;
        int generation = 1;
        public GenerateMaze maze;

        GUIStyle guiStyle = new GUIStyle();
        void OnGUI()
        {
            guiStyle.fontSize = 25;
            guiStyle.normal.textColor = Color.white;
            GUI.BeginGroup(new Rect(10, 10, 250, 150));
            GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
            GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
            GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
            GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
            GUI.EndGroup();
        }


        void Start()
        {
            for (int i = 0; i < populationSize; i++)
            {
                int starti = Random.Range(0, startingPos.Length);
                GameObject go = Instantiate(_prefab, maze.GetTileLocation(startingPos[starti]), transform.rotation);
                go.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
                go.GetComponent<Brain>().Innit();
                population.Add(go);
            }
            Time.timeScale = timeScale;
        }

        GameObject Breed(GameObject parent1, GameObject parent2)
        {
            int starti = Random.Range(0, startingPos.Length);
            GameObject offspring = Instantiate(_prefab, maze.GetTileLocation(startingPos[starti]), transform.rotation);
            offspring.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
            Brain brain = offspring.GetComponent<Brain>();

            if (Random.Range(0, 100) == 1)
            {
                brain.Innit();

            }
            else
            {
                brain.Innit();
                brain.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
            }

            return offspring;
        }

        void BreedNewPopulation()
        {
            List<GameObject> sortedList = population.OrderByDescending(x => x.GetComponent<Brain>().ammoFound).ToList();
            string ammoColected = $"generation: {generation}";

            foreach (GameObject go in sortedList)
            {
                ammoColected += $", {go.GetComponent<Brain>().ammoFound}";
            }

            Debug.Log($"Ammo Colected: {ammoColected}");
            population.Clear();

            while (population.Count < populationSize)
            {
                int bestParentCutoff = sortedList.Count / 2;

                for (int i = 0; i < bestParentCutoff - 1; i++)
                {
                    for (int j = 1; j < bestParentCutoff; j++)
                    {
                        population.Add(Breed(sortedList[i], sortedList[j]));
                        if (population.Count == populationSize) break;
                        population.Add(Breed(sortedList[j], sortedList[i]));
                        if (population.Count == populationSize) break;
                    }
                    if (population.Count == populationSize) break;
                }
            }

            for (int i = 0; i < sortedList.Count; i++)
            {
                Destroy(sortedList[i]);
            }
            generation++;
        }

        void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed > trialTime)
            {
                maze.Reset();
                BreedNewPopulation();
                elapsed = 0;
            }
        }
    }
}