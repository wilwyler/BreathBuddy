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


public class BeachSceneFunctions : MonoBehaviour
{

    /*Pysical Scene Components*/
    public JellySprite creature;
    public HappinessBar happinessBar;
    public GameObject lemonaidButton;
    public GameObject mainWalls;
    public GameObject LemonaidGlass;
    public GameObject lemonaidLiquid;
    public GameObject restartLemonaid;
    public GameObject openMouth;

    /*UI Elements*/
    public Text Title;
    public Text timeCounter;
    
    /*Flags*/
    bool drinking = false;
    bool breathStart = false;
    bool read = false;
    
    /*Spirometer Simulations*/
    SpiroSim simulated;

    /*Spirometer Recordings*/
    List<int> volume = new List<int>();
    List<int> flow = new List<int>();

    /*Local Variables*/
    System.DateTime startTime;
    string username;
    int healthPoints;
    Vector3 startScale = new Vector3(0, 0, 0);

    public void Start()
    {

        lemonaidButton.SetActive(true);
        mainWalls.SetActive(true);
        Title.text = "Choose a Game!";
        username = PlayerPrefs.GetString("CurrentUser", "Player");
        healthPoints = PlayerPrefs.GetInt(username + ".health_points", 100);
        
        startScale = creature.transform.localScale;
    }

    public void Update()
    {
        if (drinking)
        {
            //if the player chooses lemonaid drink game
            System.DateTime current_time = System.DateTime.Now;
            double seconds_passed = Math.Ceiling((current_time - startTime).TotalSeconds);
            Vector3 scaleChange = new Vector3(0.000025f, 0.000025f, 0f);
            
            //countdown to start of minigame
            if (seconds_passed <= 7 && seconds_passed >= 5)
            {
                Title.text = "Get Ready...";
            } else if (seconds_passed <= 8 && seconds_passed > 7)
            {
                Title.text = "Get Set...";
            } else if (seconds_passed > 8)
            {
                Title.text = "Breathe!";
                //begin breath test, 10 seconds
                //after 11 seconds, read the values from file
                openMouth.SetActive(true);

                if (!breathStart)
                {
                    simulated.StartVariableLengthTest(10);
                    Invoke("values", 11);
                    breathStart = true;
                }

                timeCounter.text = (seconds_passed - 9).ToString();

                //change the size of the lemonaid liquid 
                Vector3 shrink = new Vector3(0.0000005f, -0.00002f, 0f);
                Vector3 drift = new Vector3(0f, -0.00003f, 0f);
                if (lemonaidLiquid.transform.localScale.y >= 0.2)
                {
                    lemonaidLiquid.transform.localScale += shrink;
                    lemonaidLiquid.transform.position += drift;
                    creature.transform.localScale += scaleChange;
                }

                //once the player is done breathing
                if (lemonaidLiquid.transform.localScale.y <= 0.2 || seconds_passed > 20)
                {
                    drinking = false;
                    lemonaidLiquid.SetActive(false);
                    breathStart = false;
                    openMouth.SetActive(false);
                    //creature.transform.localScale = startScale;
                    timeCounter.text = "";
                }
            }
        }

        //once spirometer is done writing values, can read values
        if (read)
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
            healthPoints = PlayerPrefs.GetInt(username + ".health_points", 100);
            healthPoints += selection * 5;
            timeCounter.text = "Average volume: " + (volume.Average()).ToString() + "\n Average flow: " + (flow.Average()).ToString() + "\n Added Happiness: " + healthPoints.ToString();
            
            PlayerPrefs.SetInt(username + ".health_points", (int)healthPoints);

            string saved_time = System.DateTime.Now.ToString("F", System.Globalization.CultureInfo.InvariantCulture);

            PlayerPrefs.SetString(username + ".health_time", saved_time);

            int coins = PlayerPrefs.GetInt(username + ".coins", 100) + (selection * 5);

            PlayerPrefs.SetInt(username + ".coins", coins);
            PlayerPrefs.Save();

            happinessBar.setHealth(healthPoints);
            read = false;

            restartLemonaid.SetActive(true);
            //happyLogic.TakeBreath(health_points);
        }

        if (creature.transform.localScale.x > 1 && !breathStart)
        {
            creature.transform.localScale -= new Vector3(0.00006f, 0.00006f, 0);
        }
    }

    public void StartLemonaid()
    {
        Title.text = "Help your buddy take a drink \n of their refreshing lemonade!";
        timeCounter.text = "";
        lemonaidButton.SetActive(false);
        restartLemonaid.SetActive(false);
        lemonaidLiquid.SetActive(true);

        Vector3 movedLocation = new Vector3(0f, 9f, 0);

        if (LemonaidGlass.transform.position.y != -1)
        {
            LemonaidGlass.transform.position += movedLocation;
        }

        lemonaidLiquid.transform.localScale = new Vector3(1, 1, 1);
        lemonaidLiquid.transform.position = new Vector3(0, -1.5f, 0);

        startTime =  System.DateTime.Now; 
        drinking = true;
        simulated = gameObject.AddComponent(typeof(SpiroSim)) as SpiroSim;
    }

    /*Read the spirometer values from the file*/
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

    }


}

