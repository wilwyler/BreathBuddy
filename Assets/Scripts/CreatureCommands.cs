using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using static SpiroSim;
using System.Collections.Generic;
using System.CodeDom;
using System.Linq;
using System.Collections.Specialized;

public class CreatureCommands : MonoBehaviour
{

	public float JumpForce = 500f;
	public JellySprite creature;

	public Dropdown drop;
	public Text txt;
	public Text jumpTxt;
	List<int> volume = new List<int>();
	List<int> flow = new List<int>();
	bool read = false;
	public System.DateTime start_time;
	bool started = false;
	Vector3 scaleChange = new Vector3(0.00003f, 0.00003f, 0f);
	Vector3 startScale = new Vector3(1.0f, 1.0f, 0f);
	public GameObject openMouth;
	bool mouthOpen = false;

	public GameObject mood1;
	public GameObject mood2;
	public GameObject mood3;
	public GameObject mood4;
	public GameObject mood5;

	public int health_points;
    public string username; 

	public void Start ()
    {
    	username = PlayerPrefs.GetString("CurrentUser", "Player");
		health_points = PlayerPrefs.GetInt(username + ".health_points", 100);

		startScale = creature.transform.localScale;

		mood1.SetActive(false);
		mood2.SetActive(false);
		mood3.SetActive(false);
		mood4.SetActive(false);
		mood5.SetActive(false);

		Mood();
    }

	void Update()
    {
    	if (!mouthOpen) {
    		Mood();
    	}

		double seconds_passed;
		bool growMore = false;
		if (started)
        {
        	MoodOff();
			System.DateTime current_time = System.DateTime.Now;
			seconds_passed = Math.Ceiling((current_time - start_time).TotalSeconds);
			growMore = true;
			txt.text = seconds_passed.ToString();
			openMouth.SetActive(true);
			mouthOpen = true;

			if (seconds_passed >= 11)
			{
				growMore = false;
				txt.text = "Breath Over! Jump Force was :";
				mouthOpen = false;
				Mood();

				//creature.transform.localScale = startScale;
				started = false;
			}
			if (growMore)
            {
				creature.transform.localScale += scaleChange;
			}           
		}


		Vector3 shrinkScale = new Vector3(-0.0006f, -0.0006f, 0f);
		if (creature.transform.localScale.x >= startScale.x && creature.transform.localScale.y >= startScale.y && !growMore)
		{
			openMouth.SetActive(false);
			mouthOpen = false;
			Mood();
			creature.transform.localScale += shrinkScale;
		}

	}

	public void SendInput(int type, int amount)
    {
		if (type == 0) 
        {
			volume.Add(amount);
        } else
        {
			flow.Add(amount);
        }
    }

	public void dropChange(int selection)
    {
		SpiroSim simulated = gameObject.AddComponent(typeof(SpiroSim)) as SpiroSim;

		JumpForce = 50;

		string m_Path = Application.dataPath;
		File.WriteAllText(m_Path + "/RecentBreath.txt", string.Empty);

		List<int> vol = new List<int>();
		List<int> flow = new List<int>();

		//File.WriteAllText(m_Path + "/Scripts/RecentBreath.txt", string.Empty);

		drop.value = 0;
		drop.enabled = false;
		started = true;
		start_time = System.DateTime.Now;
		selection = selection - 1;
		if (selection == 0)
        {
			simulated.StartBadBreath();
			Invoke("values", 11);
        } else if (selection == 1) {
			simulated.StartPoorerBreath();
			Invoke("values", 11);
		} else if (selection == 2)
        {
			simulated.StartPoorBreath();
			Invoke("values", 11);
		} else if (selection == 3)
        {
			simulated.StartGoodBreath();
			Invoke("values", 11);
		} else if (selection == 4)
        {
			simulated.StartBetterBreath();
			Invoke("values", 11);
		} else if (selection == 5)
        {
			simulated.StartBestBreath();
			Invoke("values", 11);
		}

		
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
		read = true;
		creatureJump();
	}


	public void creatureJump()
	{

		Vector2 jumpVec = Vector2.zero;

		//UnityEngine.Debug.Log(volume.Count);
		//UnityEngine.Debug.Log(flow.Count);
		JumpForce = 100f;
		if (volume.Count > 0 && flow.Count > 0 && read)
        {
			JumpForce = (float)(Math.Pow((volume.Average() / flow.Average()), 3) * 3);
			UnityEngine.Debug.Log("Jump force is : " + JumpForce);
			jumpTxt.text = JumpForce.ToString();
		}
		
		jumpVec.x = -0.1f;
		jumpVec.y = 1f;
		jumpVec.Normalize();
		creature.AddForce(jumpVec * JumpForce);
		JumpForce = 50;
		drop.value = 0;
        drop.enabled = true;
		read = false;
	}

	void Mood() {
		health_points = PlayerPrefs.GetInt(username + ".health_points", 100);

		if (health_points >= 0 && health_points < 20) {
			MoodOff();
			mood1.SetActive(true);
		}
		else if (health_points >= 20 && health_points < 40) {
			MoodOff();
			mood2.SetActive(true);
		}
		else if (health_points >= 40 && health_points < 60) {
			MoodOff();
			mood3.SetActive(true);
		}
		else if (health_points >= 60 && health_points < 80) {
			MoodOff();
			mood4.SetActive(true);
		}
		else if (health_points >= 80 && health_points < 100) {
			MoodOff();
			mood5.SetActive(true);
		}
	}

	void MoodOff() {
		mood1.SetActive(false);
		mood2.SetActive(false);
		mood3.SetActive(false);
		mood4.SetActive(false);
		mood5.SetActive(false);
	}
}
