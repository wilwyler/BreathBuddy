using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static JellySprite;
using static SpiroSim;

public class ParkScene : MonoBehaviour {

    public Text directions;
    public HappinessBar happinessBar;
    // public Button replay;

    public JellySprite creature;
    public GameObject bubblesButton;
    public GameObject bubblesBottle;
    public GameObject bubblesWand;
    public GameObject bubble;

    public GameObject mouth0;
    public GameObject mouth1;
    public GameObject mouth2;
    public GameObject mouth3;
    public GameObject mouth4;

    public GameObject bubble1;
    public GameObject bubble2;
    public GameObject bubble3;
    public GameObject bubble4;
    public GameObject bubble5;


    bool bubblesSelected = false;
    bool breathFinished = false;
    Vector2 creaturePosition;
    Vector2 bubblesPosition;

    int healthPoints;
    string username;
    System.DateTime currTime;
    System.DateTime startTime;
    System.DateTime bubbles_entered_time;
    Vector3 startScale = new Vector3(0, 0, 0);


    public void Start() {
        bubblesButton.transform.position = new Vector2(-5.75f, -2.6f);

        directions.text = "Pick a game by moving over to it!";
        username = PlayerPrefs.GetString("CurrentUser", "Player");
        healthPoints = PlayerPrefs.GetInt(username + ".health_points", 100);
        happinessBar.setHealth(healthPoints);
        startScale = creature.transform.localScale;

    	creaturePosition = creature.transform.position;
    	bubblesPosition = bubblesButton.transform.position;

        // replay.gameObject.SetActive(false);
        bubblesBottle.SetActive(false);
        bubblesWand.SetActive(false);

        mouth0.SetActive(false);
        mouth1.SetActive(false);
        mouth2.SetActive(false);
        mouth3.SetActive(false);
        mouth4.SetActive(false);

        bubble1.SetActive(false);
        bubble2.SetActive(false);
        bubble3.SetActive(false);
        bubble4.SetActive(false);
        bubble5.SetActive(false);
    }

    public void Update() {
        creaturePosition = creature.transform.position;
        bubblesPosition = bubblesButton.transform.position;

        currTime = System.DateTime.Now;

        InBubblesVicinity(creaturePosition, bubblesPosition);

        if (bubblesSelected) {
        	BubblesGame();
        }
    }

    public void InBubblesVicinity(Vector2 creaturePosition, Vector2 bubblesPosition) {
    	double distance = Math.Abs(creaturePosition.x - bubblesPosition.x);
    	if (distance <= 1) {
    		bubblesSelected = true;
            bubbles_entered_time = System.DateTime.Now;
            directions.text = "Take in a deep breath first so you can blow a bubble!";
            bubblesButton.transform.position = new Vector2(-1000f, -10000f);
    	}
    }

    public void BubblesGame() {
        bubblesButton.SetActive(false);
        creature.transform.position = new Vector2(-6f, -2.903809f);
        bubblesBottle.SetActive(true);
        bubblesWand.SetActive(true);
        mouth0.SetActive(true);       

        double seconds_passed = Math.Ceiling((currTime - bubbles_entered_time).TotalSeconds);

        if (seconds_passed <= 5 && seconds_passed >= 4) {
            bubble.SetActive(false);
            directions.text = "Get Ready...";
        } 
        else if (seconds_passed <= 6 && seconds_passed > 5) {
            bubble.SetActive(false);
            directions.text = "Get Set...";
        } 
        else if (seconds_passed <= 7 && seconds_passed > 6) {
            directions.text = "Breathe!";
        }
        else if (seconds_passed == 8) {
            bubble.SetActive(false);
            startTime = System.DateTime.Now;
            UnityEngine.Debug.Log("FirstStartTime" + startTime);
            TakingBreath();
        }
        else if (seconds_passed > 8 && !breathFinished) {
            TakingBreath();
        }
        else if (breathFinished) {
            BlowBubble();
        }
    }

    public void TakingBreath() {
        double time_passed = Math.Ceiling((currTime - startTime).TotalSeconds);
        double time_remaining = 10 - time_passed;
        UnityEngine.Debug.Log("curr time" + currTime);
        UnityEngine.Debug.Log("start time" + startTime);
        if (time_remaining >= 0) {
            directions.text = $"{time_remaining}";
        }
        
        // UnityEngine.Debug.Log("seconds passed" + time_passed);
        // Vector3 scaleChange = new Vector3(0.000025f, 0.000025f, 0f); 


        if (time_passed >= 0 && time_passed < 2) {
            // creature.transform.localScale += scaleChange;
            mouth0.SetActive(true);
        }
        else if (time_passed >= 2 && time_passed < 4) {
            // creature.transform.localScale += scaleChange;
            mouth0.SetActive(false);
            mouth1.SetActive(true);
        }
        else if (time_passed >= 4 && time_passed < 6) {
            // creature.transform.localScale += scaleChange;
            mouth1.SetActive(false);
            mouth2.SetActive(true);
        }
        else if (time_passed >= 6 && time_passed < 8) {
            // creature.transform.localScale += scaleChange;
            mouth2.SetActive(false);
            mouth3.SetActive(true);
        }
        else if (time_passed >= 8 && time_passed < 10) {
            // creature.transform.localScale += scaleChange;
            mouth3.SetActive(false);
            mouth4.SetActive(true);
        }
        else if (time_passed == 10) {
            mouth4.SetActive(true);
            healthPoints = Math.Min(healthPoints + 20, 100);
            PlayerPrefs.SetInt(username + ".health_points", (int)healthPoints);
            directions.text = "Well done!";
            startTime = System.DateTime.Now;
            BlowBubble();
            breathFinished = true;
        }
        else if (time_passed > 10) {
            BlowBubble();
        }
    }

    public void BlowBubble() {
        double time_passed = Math.Ceiling((currTime - startTime).TotalSeconds);
        UnityEngine.Debug.Log("time_passed" + time_passed);

        if (time_passed >= 1 && time_passed < 2) {
            mouth4.SetActive(true);

            bubble1.SetActive(true);
        }
        else if (time_passed >= 2 && time_passed < 3) {
            mouth4.SetActive(false);
            mouth3.SetActive(true);

            bubble1.SetActive(false);
            bubble2.SetActive(true);
        }
        else if (time_passed >= 3 && time_passed < 4) {
            mouth3.SetActive(false);
            mouth2.SetActive(true);

            bubble2.SetActive(false);
            bubble3.SetActive(true);
        }
        else if (time_passed >= 4 && time_passed < 5) {
            mouth2.SetActive(false);
            mouth1.SetActive(true);

            bubble3.SetActive(false);
            bubble4.SetActive(true);
        }
        else if (time_passed >= 5 && time_passed < 6) {
            mouth1.SetActive(false);
            mouth0.SetActive(true);

            bubble4.SetActive(false);
            bubble5.SetActive(true);

            // replay.gameObject.SetActive(true);
        }
    }

}
