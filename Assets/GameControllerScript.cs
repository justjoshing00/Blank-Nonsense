using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    private static GameControllerScript gameController;

    protected void Awake()
    {
        gameController = this; 
    }

    protected void OnDestroy()
    {
        if(gameController != null)
        {
            gameController = null;
        }
        
    }
    protected void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            MainControllerScript.SwitchScene("MenuScene");
        }
    }
}
