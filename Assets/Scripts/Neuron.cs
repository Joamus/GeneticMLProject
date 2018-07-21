using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    protected float output;
    public List<Connection> connections;
    float bias = 1;


    public Neuron(float bias) {
        this.output = 0;
        connections = new List<Connection>();

    }

    public Neuron() {
        this.output = 0;
        connections = new List<Connection>();
    }

    public void CalculateOutput()
    {

        float sum = 0;

        for (int i = 0; i < connections.Count; i++)
        {
            Connection connection = connections[i];
            Neuron from = connection.GetFrom();
            Neuron to = connection.GetTo();

            if (to == this)
            {
                sum += from.GetOutput() * connection.GetWeight();

            }

            output = Sigmoid(sum + bias);
        }
    }
        

    float Sigmoid(float x) {
        float sigmoid = (1 / (1 + Mathf.Exp(-x)));



        return sigmoid;
        
    }

    public float GetOutput() {
        return output;
    }
    public float GetBias() {
        return bias;
    }
    public List<Connection> GetConnections() {
        return connections;
    }
    public void AddConnection(Connection connection) {
        connections.Add(connection);
        
    }

    public void SetOutput(float value)
    {
        output = value;

    }

}
