using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerScript : MonoBehaviour {

    private static MenuControllerScript menuController;

    protected void Awake()
    {
        menuController = this;
    }

    protected void OnDestroy()
    {
        if (menuController != null)
        {
            menuController = null;
        }

    }
    protected void Update()
    {
        if (Input.GetKeyDown("p")) 
        {
            MainControllerScript.SwitchScene("GameScene");
        }
    }
}
