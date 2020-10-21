using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SaveUserHealth : MonoBehaviour {
    public Button SaveButton;
    public string username;
    public static int health_points;

    void Start() {
        username = PlayerPrefs.GetString("CurrentUser", "default");
        Debug.Log("curr user: " + username);
        health_points = PlayerPrefs.GetInt(username + ".health_points", 100);
        Debug.Log("start health_points: " + health_points);
    }

    void Update() {
        health_points = GetComponent<HappyLogic>().health_points;
        PlayerPrefs.SetInt(username+".health_points", health_points);
        SaveHealth();
    }

    void SaveHealth() {
        SaveButton.onClick.AddListener(SaveButtonClicked);
        void SaveButtonClicked() {
            health_points = GetComponent<HappyLogic>().health_points;
            Debug.Log("updated health_points: " + health_points);
            PlayerPrefs.SetInt(username + ".health_points", health_points);
        }
    }
}