using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Connection {
    Neuron from;
    Neuron to;
    float weight;

    public Connection(Neuron from, Neuron to) {
        this.from = from;
        this.to = to;


        weight = UnityEngine.Random.Range(-2f, 2f);
   

    }

    public Connection(Neuron from, Neuron to, float weight) {
        this.from = from;
        this.to = to;
        this.weight = weight;
        
    }

    public Neuron GetFrom() {
        return from;
    }

    public Neuron GetTo() {
        return to;
    }

    public float GetWeight() {
        return weight;
    }

    public void SetWeight(float value) {
        weight = value;
    }

 

    public void AdjustWeight(float deltaWeight) {
        weight += deltaWeight;
    } 

    }

