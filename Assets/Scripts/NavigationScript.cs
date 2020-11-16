﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NavigationScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //change scene function for navigation
    public void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);

    }

    public void ExitGame()
    {
        //Quit();
        Application.Quit();
    }

    }
