using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallScript : MonoBehaviour
{
    public float velocity;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        MoveDown();

    }

    void MoveDown() {
        Vector3 vector = new Vector3(0, -(velocity*Time.fixedDeltaTime), 0);
        transform.Translate(vector);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            Destroy(gameObject);
        }
    }


}
