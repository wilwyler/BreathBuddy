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

	public void Start ()
    {
		
    }

	void Update()
    {
		double seconds_passed;
		if (started)
        {
			System.DateTime current_time = System.DateTime.Now;
			seconds_passed = Math.Ceiling((current_time - start_time).TotalSeconds);
			
			txt.text = seconds_passed.ToString();
			if (seconds_passed >= 11)
			{
				started = false;
				txt.text = "Breath Over! Jump Force was :";
			}
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
}
