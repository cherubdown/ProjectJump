using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{

    public GameObject player;

    private float yRadius = 5;

    void Start()
    {
    }

    void Update()
    {
        float playerY = player.GetComponent<Rigidbody2D>().position.y;
            this.GetComponent<Rigidbody2D>().position = new Vector2(0, playerY);
        
    }
}
