using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {
    public int generation;
    int fitnessSum;
    public static int playerAmount = 50;
    float timeAtLastFitnessUpdate;

    public GameObject playerPrefab;
    public static GameObject[] players;
    public static List<Brain> deadPlayers;

    public GameObject leftBoundary;
    public GameObject rightBoundary;

    public GameObject ballPrefab;
    public float spawnDelay;
    float timeAtLastBallSpawn;

    public Text fitnessText;
    public Text generationText;
    public Text playerAmountText;

    bool firstTimeRunning;
    bool gamePaused;
    // Use this for initialization
    void Start () {
        gamePaused = false;
        firstTimeRunning = true;
        FindBallSpawnRangeX();
        StartGame();
      
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - timeAtLastBallSpawn >= spawnDelay) {
            InstantiateBalls();
            timeAtLastBallSpawn = Time.time;
        }
        players = GameObject.FindGameObjectsWithTag("Player");
        UpdateTextFields();

        if (!gamePaused)
        {
            if (Time.time - timeAtLastFitnessUpdate >= 1)
            {
                fitnessSum = GetFitnessSum();
            }
        }

        if (players.Length == 0) {
            gamePaused = true;
            fitnessSum = 0;
            StartGame();
            gamePaused = false;

        }

        //if (gameRunning == false) {           
        //    gameRunning = true;
        //    generation++;
        //    StartGame();

        //}
		
	}

    void InstantiatePlayers() {
        for (int i = 0; i < playerAmount; i++)
        {
            Instantiate(playerPrefab, new Vector3(0, -3.5f, 0), Quaternion.identity);
        }
    }

    void InstantiatePlayers(List<Brain> brains)
    {
        for (int i = 0; i < playerAmount; i++)
        {
            
            GameObject player = Instantiate(playerPrefab, new Vector3(0, -3.5f, 0), Quaternion.identity);
            player.GetComponent<PlayerScript>().SetBrain(brains[i]);
        }
    }

    float FindBallSpawnRangeX() {
        float leftBoundaryX = leftBoundary.transform.position.x;
        float rightBoundaryX = rightBoundary.transform.position.x;
        float spawnX = Random.Range(leftBoundaryX, rightBoundaryX);

        return spawnX;
        
    }

    void InstantiateBalls() {
        Vector3 vector = new Vector3(FindBallSpawnRangeX(), 10, 0);
        Instantiate(ballPrefab, vector, Quaternion.identity);

    }


    void PrintPlayersAliveAmount() {

    }

    void UpdateTextFields() {
        generationText.text = "Generation: " + generation;
        fitnessText.text = "Fitness: " + fitnessSum;
        playerAmountText.text = "Players Alive: " + players.Length;

    }

    int GetFitnessSum() {
        timeAtLastFitnessUpdate = Time.time;
        int sum = 0;


        if (players.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null) {
                    PlayerScript playerScript = players[i].GetComponent<PlayerScript>();
                    int playerFitness = playerScript.GetFitness();

                    sum += playerFitness;
                }
            }
        }

        if (deadPlayers.Count > 0)
        {
            for (int i = 0; i < deadPlayers.Count; i++)
            {
                if (deadPlayers[i] != null)
                {
                    int playerFitness = deadPlayers[i].GetFitness();

                    sum += playerFitness;

                }
            }
        }

        return sum;
    }



    void StartGame() {
        if (firstTimeRunning)
        {
            InstantiatePlayers();
            players = GameObject.FindGameObjectsWithTag("Player");
            deadPlayers = new List<Brain>();
            firstTimeRunning = false;
        } else {
            InstantiatePlayers(GeneticAlgorithm.selection(deadPlayers));
            deadPlayers = new List<Brain>();
            players = GameObject.FindGameObjectsWithTag("Player");
            generation++;

            GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < allBalls.Length; i++) {
                Destroy(allBalls[i]);
            }
        }

        InstantiateBalls();
        
    }
}
