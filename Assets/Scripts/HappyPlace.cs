using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using static JellySprite;
using static SpiroSim;
using static HappyLogic;
using System.Threading;
using System.Linq;



public class HappyPlace : MonoBehaviour
{

    public JellySprite creature;
	public Text Title;
	public Text timeCounter;
	public GameObject balloonButton;
	public GameObject balloon;
    public GameObject startBreathButton;
	public GameObject particals;
	public GameObject mouth;
	public HappinessBar happinessBar;

    List<int> volume = new List<int>();
    List<int> flow = new List<int>();

	//bool read = false;
	bool balloonStart = false;
	bool breathing = false;
	bool ending = false;

	//Player Pref
	string username;
	int health_points;

	System.DateTime start_time;

	SpiroSim simulated;


	public void Start()
    {
		Title.text = "Choose a Game!";
		username = PlayerPrefs.GetString("CurrentUser", "Player");
		health_points = PlayerPrefs.GetInt(username + ".health_points", 100);
	}

    public void Update()
    {
		
		if (balloonStart)
        {
			

			UnityEngine.Debug.Log("Balloon Start");
			//double seconds_passed;
			if (breathing)
            {
				UnityEngine.Debug.Log("Breathing");
				System.DateTime current_time = System.DateTime.Now;
				double seconds_passed = Math.Floor((current_time - start_time).TotalSeconds);
				timeCounter.text = seconds_passed.ToString();
				if (seconds_passed >= 10)
                {
					breathing = false;
					ending = true;
					Title.text = "Breath Over! Great Job!";
				}
				creature.transform.localScale += new Vector3(0.00003f, 0.00003f, 0f);
				
				//start_time = System.DateTime.Now;
				
			} else
            {
				if (creature.transform.localScale.x > 1)
                {
					particals.SetActive(true);
					creature.transform.localScale -= new Vector3(0.00006f, 0.00006f, 0f);
					balloon.transform.localScale += new Vector3(0.00003f, 0.00003f, 0f);
				} else if (ending)
                {
					balloonStart = false;
					particals.SetActive(false);
					mouth.SetActive(false);
					ending = false;
					read();
                }
            }

        }
		

    }

    public void StartBalloonGame()
    {
		balloonButton.SetActive(false);

		Title.text = "Help your buddy blow up \nthe balloon by taking a\ndeep breath!";
		if(balloon.transform.position.y == -9)
        {
			balloon.transform.position += new Vector3(0f, 7f, 0);
		}
		balloonStart = true;
		
		simulated = gameObject.AddComponent(typeof(SpiroSim)) as SpiroSim;
		startBreathButton.SetActive(true);
	}

	public void values()
	{
		int start = 0;

		volume.Clear();
		flow.Clear();

		string m_Path = Application.dataPath;
		//File.WriteAllText(m_Path + "/Scripts/RecentBreath.txt", string.Empty);
		StreamReader sr = new StreamReader(m_Path + "/RecentBreath.txt");
		string line = sr.ReadLine();
		UnityEngine.Debug.Log(line);
		while (line != null)
		{
			if (start % 2 == 0)
			{
				//UnityEngine.Debug.Log("VolumeAdded");
				volume.Add(Int32.Parse(line));
			}
			else if (start % 2 == 1)
			{
				//UnityEngine.Debug.Log("FlowAdded");
				flow.Add(Int32.Parse(line));
			}
			start++;
			line = sr.ReadLine();
		}

		sr.Close();


	}

	public void read()
    {

		/*------------------------------------------------------------------------------------------*/
		/*Calculations for happiness and coin increase based on variability of breath*/
		List<int> valleyLengths = new List<int>();
		var biggestVal = 0;
		var start = 0;
		var end = 0;
		for (int i = 1; i < 10; i++)
		{
			//If we encounter a dip to zero
			if (flow[i] == 0)
			{
				//Occurs when we encounter a new valley 
				if (i != end + 1)
				{
					start = i;
				}
				end = i;
				if (i == 9)
				{
					Debug.Log("start: " + start + " end: " + end);
					valleyLengths.Add(end - start + 1);
					if (biggestVal < end - start + 1)
					{
						biggestVal = end - start + 1;
					}
				}
			}
			else
			{
				if (i == end + 1 && i != 1)
				{
					Debug.Log("start: " + start + " end: " + end);
					valleyLengths.Add(end - start + 1);
					if (biggestVal < end - start + 1)
					{
						biggestVal = end - start + 1;
					}
				}
			}
		}
		Debug.Log("number of valleys:" + valleyLengths.Count);
		foreach (var length in valleyLengths)
		{
			Debug.Log("lengths: " + length);
		}

		//penalize a lot of gaps in flow
		var selection = 6;
		if (valleyLengths.Count >= 3)
		{
			selection -= 5;
		}
		if (valleyLengths.Count == 2)
		{
			selection -= 2;
		}
		if (valleyLengths.Count == 1)
		{
			selection -= 1;
		}

		//penalize longer gaps
		if (biggestVal >= 4)
		{
			selection -= 2;
		}
		if (biggestVal == 3)
		{
			selection -= 1;
		}

		//Reward flow that is mostly 1s or 2s
		int num_ones_twos = 0;
		int num_twos = 0;
		for (int i = 0; i < 20; i++)
		{
			if (flow[i] == 1 || flow[i] == 2)
			{
				num_ones_twos += 1;
			}
		}
		if (num_ones_twos >= 16)
		{
			Debug.Log("rewarded");
			selection += 1;
		}

		/*------------------------------------------------------------------------*/
		/*Set the values caluclated*/
		health_points = PlayerPrefs.GetInt(username + ".health_points", 100);
		health_points += selection * 5;
		timeCounter.text = "Added Happiness: " + health_points.ToString();

		var previousCoins = PlayerPrefs.GetInt(username + ".coins", 100) + (selection * 5);

		PlayerPrefs.SetInt(username + ".health_points", (int)health_points);
		PlayerPrefs.SetInt(username + ".coins", (int)previousCoins);

		string saved_time = System.DateTime.Now.ToString("F", System.Globalization.CultureInfo.InvariantCulture);

		PlayerPrefs.SetString(username + ".health_time", saved_time);
		PlayerPrefs.Save();

		happinessBar.setHealth(health_points);

	}


	public void StartBreath()
    {
		start_time = System.DateTime.Now;
		simulated.StartVariableLengthTest(10);
		Invoke("values", 11);
		breathing = true;
		mouth.SetActive(true);
		particals.SetActive(false);
		startBreathButton.SetActive(false);
	}
}
