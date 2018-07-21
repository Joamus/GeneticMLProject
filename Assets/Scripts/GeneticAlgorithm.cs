using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

    public class GeneticAlgorithm : MonoBehaviour
    {
        static List<Brain> fittestBrains;
        public GameObject playerPrefab;
        static float mutationRate = 0.2f;

        // For selection I'll just take the top 5% of the list, which we know are the ones added last to the array.

        public static List<Brain> selection(List<Brain> generation) {
            int topTenPercent = (int)(generation.Count / 10);
            
            fittestBrains = new List<Brain>();


            // If there are 100 cubes in the generation, this should take the last 10
        for (int i = generation.Count-topTenPercent; i < generation.Count; i++) {
            Debug.Log("Top : " + i + " Fitness: " + generation[i].GetFitness());
                fittestBrains.Add(generation[i]);
            }

            fittestBrains = crossover();
            
        return fittestBrains;

        }

        static List<Brain> crossover() {
            int newPopulationAmount = GameManagerScript.playerAmount;
            List<Brain> newGeneration = new List<Brain>();

        for (int i = 0; i < newPopulationAmount; i++)
        {
            Brain dad = pickRandomBrain(fittestBrains);
            Brain mom = pickRandomBrain(fittestBrains);

            List<float> childWeights = new List<float>();


            for (int j = 0; j < dad.GetWeights().Count; j++)
            {
                int randomNumber = UnityEngine.Random.Range(0, 2);

                if (randomNumber == 0)
                {
                    childWeights.Add(dad.GetWeight(j));
                } else if (randomNumber == 1) {
                    childWeights.Add(mom.GetWeight(j));
                }

            }
            Brain child = new Brain(childWeights, mutationRate);
            newGeneration.Add(child);
        }

        return newGeneration;
            
        }

        static Brain pickRandomBrain(List<Brain> brains) {
            Brain random = brains[UnityEngine.Random.Range(0, brains.Count)];
            return random;
        }

        public static float mutate() {
           
            return UnityEngine.Random.Range(-2, 2);

        }

     

        static void setMutationRate(float value) {
            mutationRate = value;
            
        }

        static float getMutationRate() {
            return mutationRate;
        }




    }

