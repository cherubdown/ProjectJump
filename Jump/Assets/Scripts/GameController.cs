﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject PlayerObj;
    public GameObject BasePlatform;

    Player PlayerScr;
    float previousPlatformY;

	void Start () {
       
        PlayerScr = PlayerObj.GetComponent<Player>();
        previousPlatformY = PlayerObj.transform.position.y;
	}
	
	void Update () {
        if (PlayerScr.MaxHeight > previousPlatformY + 10f)
        {
            previousPlatformY = PlayerScr.MaxHeight;
            Instantiate(BasePlatform,
                new Vector3(0f, PlayerObj.transform.position.y) + new Vector3(0f, 30f, 0),
                Quaternion.identity);
        }
	}
}
