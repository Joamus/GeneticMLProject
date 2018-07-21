using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public GameObject playerPrefab;
    bool isAlive;

    Brain brain;

    public float velocity;

    public KeyCode left;
    public KeyCode right;

    public Transform leftRayCastStart;
    public Transform leftRayCastEnd;
    float leftRayDistance;

    public Transform upRayCastStart;
    public Transform upRayCastEnd;
    float upRayDistance;

    public Transform rightRayCastStart;
    public Transform rightRayCastEnd;
    float rightRayDistance;

    public Transform mostLeftRayCastStart;
    public Transform mostLeftRayCastEnd;
    float mostLeftRayDistance;

    public Transform mostRightRayCastStart;
    public Transform mostRightRayCastEnd;
    float mostRightRayDistance;


    public Transform leftWallRayStart;
    public Transform leftWallRayEnd;
    float leftWallRayDistance;

    public Transform rightWallRayStart;
    public Transform rightWallRayEnd;
    float rightWallRayDistance;

    float timeSinceSpawn;


    // Use this for initialization
    void Start () {
        brain = new Brain();
        InvokeRepeating("UpdateFitness", 0, 1);
		
	}
	
	// Update is called once per frame
	void Update () {
        // Balls
        leftRayDistance = BallRayCasting(leftRayCastStart, leftRayCastEnd);
        upRayDistance = BallRayCasting(upRayCastStart, upRayCastEnd);
        rightRayDistance = BallRayCasting(rightRayCastStart, rightRayCastEnd);
        mostRightRayDistance = BallRayCasting(mostRightRayCastStart, mostRightRayCastEnd);
        mostLeftRayDistance = BallRayCasting(mostLeftRayCastStart, mostLeftRayCastEnd);

        // Walls
        rightWallRayDistance = WallRayCasting(rightWallRayStart, rightWallRayEnd);
        leftWallRayDistance = WallRayCasting(leftWallRayStart, leftWallRayEnd);


        UpdateBrain();
	}

    void FixedUpdate()
    {
        DecideAction();
        if (Input.GetKey(left)) {
            MoveLeft();
        }
        if (Input.GetKey(right))
        {
            MoveRight();
        }

       
    }

    void MoveLeft() {
        Vector3 vector = new Vector3(-(velocity*Time.fixedDeltaTime), 0, 0);
        transform.Translate(vector);
       

    }

    void MoveRight() {
        Vector3 vector = new Vector3(velocity * Time.fixedDeltaTime, 0, 0);
        transform.Translate(vector);
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Brain clone = gameObject.GetComponent<PlayerScript>().brain;
            GameManagerScript.deadPlayers.Add(clone);
            Destroy(gameObject);
        

        }

    }

    float BallRayCasting(Transform start, Transform end) {
        Debug.DrawLine(start.position, end.position, Color.green);


        float rayDistance = Physics2D.Linecast(start.position, end.position, 1 << LayerMask.NameToLayer("Enemy")).distance;

        return rayDistance;
        
    }

    float WallRayCasting(Transform start, Transform end)
    {
        Debug.DrawLine(start.position, end.position, Color.green);


        float rayDistance = Physics2D.Linecast(start.position, end.position, 1 << LayerMask.NameToLayer("Wall")).distance;

        return rayDistance;

    }



    void UpdateBrain() {
        brain.UpdateInput(0, leftRayDistance);
        brain.UpdateInput(1, upRayDistance);
        brain.UpdateInput(2, rightRayDistance);
        brain.UpdateInput(3, mostLeftRayDistance);
        brain.UpdateInput(4, mostRightRayDistance);
        brain.UpdateInput(5, leftWallRayDistance);
        brain.UpdateInput(6, rightWallRayDistance);

    }

    void DecideAction() {
        
        int indexOfActiveNeuron = brain.GetAction();
    
   

        switch(indexOfActiveNeuron) {
            case 0:
                MoveLeft();
                break;
            case 1:
                MoveRight();
                break;
            default:
                break;

        }
    }

    void UpdateFitness() {
        brain.AdjustFitness(1);
        
    }

    public int GetFitness() {
        return brain.GetFitness();
    }

    public void SetIsAlive(bool b) {
        isAlive = b;
    }

    public bool GetIsAlive() {
        return isAlive;
    }

    public Brain GetBrain() {
        return brain;
    }

    public void SetBrain(Brain brain) {
        this.brain = brain;
        
    }
}
