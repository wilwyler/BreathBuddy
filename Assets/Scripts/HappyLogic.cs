using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HappyLogic : MonoBehaviour
{
    public int health_points; 
    public string saved_time;
    public string username; 
    public Dropdown drop;

    public HappinessBar happinessBar; 
    

    // Start is called before the first frame update
    void Start()
    {
        username = PlayerPrefs.GetString("CurrentUser", "Player");
        health_points = PlayerPrefs.GetInt(username + ".health_points", 100);
        happinessBar.setHealth(health_points);
        setHealthTime();
    }

    void setHealthTime(){
        username = PlayerPrefs.GetString("CurrentUser", "Player"); 
        health_points = PlayerPrefs.GetInt(username + ".health_points", 100);
        saved_time = PlayerPrefs.GetString(username + ".health_time", System.DateTime.Now.ToString("F", System.Globalization.CultureInfo.InvariantCulture)); 
        Debug.Log("set start values"); 
        Debug.Log(username); 
        Debug.Log(health_points);
        Debug.Log(saved_time); 
        happinessBar.initBar(health_points); 
    }

    // Update is called once per frame
    void Update()
    {
        System.DateTime old_time = System.DateTime.ParseExact(saved_time, "F", System.Globalization.CultureInfo.InvariantCulture); 
        System.DateTime current_time = System.DateTime.Now; 
        double seconds_passed = (current_time - old_time).TotalSeconds; 
        int old_health = PlayerPrefs.GetInt(username + ".health_points", 100);// health_points; 
        health_points = old_health;
        if(seconds_passed > 1){
            //Debug.Log("More than 5 seconds have passed"); 
            if(health_points - 1 > 0){
                health_points -= 1; 
                Debug.Log(health_points); 
            }
            else{
                health_points = 0;
                Debug.Log(health_points); 
            }
            happinessBar.setHealth(health_points); 
            saved_time = System.DateTime.Now.ToString("F", System.Globalization.CultureInfo.InvariantCulture); 
        }
        if(old_health != health_points){
            PlayerPrefs.SetInt(username + ".health_points", health_points); 
            PlayerPrefs.SetString(username + ".health_time", saved_time); 
            PlayerPrefs.Save(); 
        }
    }

    public void TakeBreath(int selection)
    {
        if (selection == 1)
        {
            health_points += 10;
        } else if (selection == 2)
        {
            health_points += 15;
        } else if (selection == 3)
        {
            health_points += 20;
        }
        else if (selection == 4)
        {
            health_points += 25;
        }
        else if (selection == 5)
        {
            health_points += 30;
        }
        else if (selection == 6)
        {
            health_points += 35;
        } else
        {
            health_points += selection;
        }

        PlayerPrefs.SetInt(username + ".health_points", health_points);
        PlayerPrefs.SetString(username + ".health_time", saved_time);
        PlayerPrefs.Save();
    }


}
