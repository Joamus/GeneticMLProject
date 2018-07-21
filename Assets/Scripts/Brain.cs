using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Brain
{
    int fitness = 0;

    List<Neuron> inputLayer;
    int inputAmount = 7;

    List<Neuron> hiddenLayer;
    int hiddenAmount = 6;

    List<Neuron> outputLayer;
    int outputAmount = 2;

    float outputThreshhold = 0.5f;

    float mutationRate;

    List<float> weights;


    public Brain() {
        this.inputLayer = MakeLayer(inputAmount);
        this.hiddenLayer = MakeLayer(hiddenAmount);
        this.outputLayer = MakeLayer(outputAmount);

        ConnectLayers();

    }

    public Brain(List<float> weights, float mutationRate) {
        this.inputLayer = MakeLayer(inputAmount);
        this.hiddenLayer = MakeLayer(hiddenAmount);
        this.outputLayer = MakeLayer(outputAmount);
        this.mutationRate = mutationRate;

        ConnectLayers(weights);
        
        
    }

    List<Neuron> MakeLayer(int neuronCount) {
        List<Neuron> list = new List<Neuron>();
        for (int i = 0; i < neuronCount; i++) {
            list.Add(new Neuron());
        }
        return list;
        
    }

    void ConnectLayers() {
        ConnectInputHidden();
        ConnectHiddenOutput();
    }

    void ConnectLayers(List<float> weights)
    {
        int currentIndex = ConnectInputHidden(weights);
        ConnectHiddenOutput(currentIndex, weights);
    }

  

    int ConnectInputHidden(List<float> weights)
    {
        /* We need to keep track of which weights we have used, to make sure that the crossover works as supposed.
           Weight at index 0 is the first connection between i0 and h0 (hopefully), and the next connection is i0 to h1 and so on.
           The reason we use "weights[i+j]" is because it will give us the right index aka the # of the connection. So for example
           The 3rd connection would be between i0 and h2, but then i would be 0 still and h would be 2
           Ugly code, but it will hopefully work.

        After all of this, we'll return the current index, so that the next "Connect" function will start at the appropriate conncetion
        */

        int currentIndex = 0;
        for (int i = 0; i < inputLayer.Count; i++)
        {
            for (int j = 0; j < hiddenLayer.Count; j++)
            {
                Connection connection = new Connection(inputLayer[i], hiddenLayer[j], weights[i+j]);
                if (shouldMutate())
                {
                    connection.SetWeight(GeneticAlgorithm.mutate());
                }
                inputLayer[i].AddConnection(connection);
                hiddenLayer[j].AddConnection(connection);
            }

        }

        return currentIndex;
    }



    void ConnectInputHidden() {
        for (int i = 0; i < inputLayer.Count; i++)
        {
            for (int j = 0; j < hiddenLayer.Count; j++)
            {
                Connection connection = new Connection(inputLayer[i], hiddenLayer[j]);
                inputLayer[i].AddConnection(connection);
                hiddenLayer[j].AddConnection(connection);
            }

        }
    }

    void ConnectHiddenOutput() {
        for (int i = 0; i < hiddenLayer.Count; i++) {
            for (int j = 0; j < outputLayer.Count; j++) {
                Connection connection = new Connection(hiddenLayer[i], outputLayer[j]);
                hiddenLayer[i].AddConnection(connection);
                outputLayer[j].AddConnection(connection);
            }
        }

    }

    void ConnectHiddenOutput(int currentIndex, List<float> weights)
    {
        
        for (int i = 0; i < hiddenLayer.Count; i++)
        {
            for (int j = 0; j < outputLayer.Count; j++)
            {
                Connection connection = new Connection(hiddenLayer[i], outputLayer[j], weights[currentIndex+i+j]);
                // Mutation
                if (shouldMutate()) {
                    connection.SetWeight(GeneticAlgorithm.mutate());
                }

                hiddenLayer[i].AddConnection(connection);
                outputLayer[j].AddConnection(connection);
            }
        }

    }

   public void UpdateInput(int index, float value) {
        inputLayer[index].SetOutput(value);
        // Input 0 is left raycast, that finds distance
        // Input 1 is center raycast
        // Input 2 is right raycast

        // When the input gets updated, we will forward propagate the rest of the neural network (hidden layers, output layers)
        FeedForward();


    }

    void FeedForward() {
        for (int i = 0; i < hiddenLayer.Count; i++) {
            hiddenLayer[i].CalculateOutput();
        }

        for (int i = 0; i < outputLayer.Count; i++) {
            outputLayer[i].CalculateOutput();
        }

    }

    public void SetThreshold(float value) {
        outputThreshhold = value;
        
    }

    public float GetThreshhold() {
        return outputThreshhold;
    }

    public void AdjustFitness(int value) {
        fitness += value;
    }

    public int GetFitness() {
        return fitness;
        
    }

    public void SetFitness(int value) {
        fitness = value;
        
    }


    // Returns the index of the most active neuron
    public int GetAction() {
        int firstTime = 0;
        Neuron mostActiveNeuron = new Neuron();
        int indexOfNeuron = -1;


        if (firstTime == 0) {
            PrintNetwork();
            firstTime = 1;
        }

        for (int i = 0; i < outputLayer.Count; i++)
        {
            if (outputLayer[i].GetOutput() > mostActiveNeuron.GetOutput())
            {
                if (outputLayer[i].GetOutput() >= GetThreshhold())
                {
                    mostActiveNeuron = outputLayer[i];
                    indexOfNeuron = i;
                }
            }
        }

            return indexOfNeuron;          

    }


    public List<Neuron> GetNeurons() {
        List<Neuron> neurons = new List<Neuron>();
        for (int i = 0; i < inputLayer.Count; i++) {
            neurons.Add(inputLayer[i]);

        }
        for (int i = 0; i < hiddenLayer.Count; i++)
        {
            neurons.Add(hiddenLayer[i]);

        }

        for (int i = 0; i < outputLayer.Count; i++)
        {
            neurons.Add(outputLayer[i]);

        }

        return neurons;
    }

    public List<float> GetWeights() {
        List<Neuron> neurons = GetNeurons();
        List<float> weights = new List<float>();

        for (int i = 0; i < neurons.Count; i++) {
            for (int j = 0; j < neurons[i].connections.Count; j++) {
                weights.Add(neurons[i].connections[j].GetWeight());
            }
        }
        this.weights = weights;
        return weights;
    }

    public float GetWeight(int index) {
        if (weights == null) {
            weights = GetWeights();
        }
        return weights[index];
        
    }

    public void PrintNetwork() {
        for (int i = 0; i < inputLayer.Count; i++) {
        //    Debug.Log("Input " + i + " " + inputLayer[0].GetOutput());
        }

        for (int i = 0; i < hiddenLayer.Count; i++)
        {
       //     Debug.Log("Hidden " + i + " " + hiddenLayer[0].GetOutput());
        }

        for (int i = 0; i < outputLayer.Count; i++)
        {
       //     Debug.Log("Output " + i + " " + outputLayer[0].GetOutput());
        }

    }

    bool shouldMutate() {
        float foundNumber = Random.Range(mutationRate, 100);
        if (mutationRate >= foundNumber) {
            return true;
        } else {
            return false;
        }
        
    }


      

}
