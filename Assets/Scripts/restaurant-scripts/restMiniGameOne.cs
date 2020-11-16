using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


//Object instance functions are asynchroous

public class restMiniGameOne : MonoBehaviour
{
    public GameObject playButton; 
    public GameObject happyMeter; 
    public GameObject spirometer; 
    public GameObject foodSmoke; 
    public GameObject gamePrompt; 
    public GameObject mouth; 

    public GameObject backButton; 
    public GameObject mapButton; 

    public restHappyLogic happyClassInst; 
    public restSpiroSim spiroClassInst; 
    public Text textPrompt; 

    public bool recievingData; 
    
    public List<int> cur_vol = new List<int>();  
    public List<int> cur_flow = new List<int>(); 


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("started instance of game"); 
        //Init the button 
        playButton.SetActive(true); 
        Button btn = playButton.GetComponent<Button>();
		btn.onClick.AddListener(StartGameWrapper);

        //Store instance of happyMeter class
        happyClassInst = happyMeter.GetComponent<restHappyLogic>(); 

        //Store instance of spiroSim class
        spiroClassInst = spirometer.GetComponent<restSpiroSim>(); 

        //Store instance of text from gamePrompt
        textPrompt = gamePrompt.GetComponent<Text>(); 

        //Make the mouth closed
        mouth.SetActive(false); 
    }

    //Start the game and start spiro challenge
    void StartGameWrapper()
    {
        StartCoroutine(StartGame()); 
	}

    IEnumerator StartGame(){
        cur_vol.Clear(); 
        cur_flow.Clear(); 
		Debug.Log("You have clicked the button!");
        ParticleSystem ps = foodSmoke.GetComponent<ParticleSystem>(); 
        var fo = ps.forceOverLifetime; 
        fo.enabled = false; 
        var em = ps.emission;
        em.rateOverTime = 4; 
        playButton.SetActive(false); 
        backButton.SetActive(false); 
        mapButton.SetActive(false); 
        textPrompt.text = "Get ready to breathe..."; 
        yield return new WaitForSeconds(3); 
        textPrompt.text = "Start Breathing!"; 
        mouth.SetActive(true); 
        yield return new WaitForSeconds(0.5f); 
        recievingData = true; 
        spiroClassInst.StartVariableLengthTest(10); 
    }

    //Read data from spiro
    public void sendInput(int vol, int flow)
    {
        //22 data points are added from hardcoded challenges, 2 extra data points are added for the end sentinel values
        //sentinel values = 2 0s
        //also relies on Spirometer to send an input every second in order to update prompt
        if(cur_vol.Count < 20 && recievingData == true){
            Debug.Log("recieved vol: " + vol); 
            Debug.Log("recieved flow: " + flow); 
            Debug.Log("Game controller recieved values from spiro!"); 
            textPrompt.text = ("Time elapsed:\n" + cur_vol.Count / 2) + " seconds"; 
            cur_vol.Add(vol); 
            cur_flow.Add(flow); 
        }
        if(cur_vol.Count == 20 && recievingData == true){
            recievingData = false; 
            //Determine the quality of breath depending on collected data
            //last two data points are sentinel values, indicating end of breath - do not include in data?
            Debug.Log("Done recieving spirometer data!");
            textPrompt.text = "Game Over"; 
            
            //Calculate number and length of all flow gaps (valleys)
            List<int> valleyLengths = new List<int>(); 
            var biggestVal = 0; 
            var start = 0; 
            var end = 0; 
            for(int i = 1; i < 20; i++){
                //If we encounter a dip to zero
                if(cur_flow[i] == 0){
                    //Occurs when we encounter a new valley 
                    if(i != end + 1){
                        start = i; 
                    }
                    end = i; 
                    if(i == 19){
                        Debug.Log("start: " + start + " end: " + end); 
                        valleyLengths.Add(end - start + 1); 
                        if(biggestVal < end - start + 1){
                            biggestVal = end - start + 1; 
                        }
                    }
                }
                else{
                    if(i == end + 1 && i != 1){
                        Debug.Log("start: " + start + " end: " + end); 
                        valleyLengths.Add(end - start + 1); 
                        if(biggestVal < end - start + 1){
                            biggestVal = end - start + 1; 
                        }
                    }
                }
            }
            Debug.Log("number of valleys:" + valleyLengths.Count); 
            foreach(var length in valleyLengths){
                Debug.Log("lengths: " + length); 
            }

            //penalize a lot of gaps in flow
            var selection = 6; 
            if(valleyLengths.Count >= 3){
                selection -= 5; 
            }
            if(valleyLengths.Count == 2){
                selection -= 2; 
            }
            if(valleyLengths.Count == 1){
                selection -= 1; 
            }

            //penalize longer gaps
            if(biggestVal >= 4){
                selection -= 2; 
            }
            if(biggestVal == 3){
                selection -= 1; 
            }

            //Reward flow that is mostly 1s or 2s
            int num_ones_twos = 0; 
            int num_twos = 0; 
            for(int i = 0; i < 20; i++){
                if(cur_flow[i] == 1 || cur_flow[i] == 2){
                    num_ones_twos += 1; 
                }
            }
            if(num_ones_twos >= 16){
                Debug.Log("rewarded"); 
                selection += 1; 
            }
            
            //Update health accordingly
            if(selection < 1){
                selection = 1; 
            }
            else if(selection > 6){
                selection = 6; 
            }
            happyClassInst.TakeBreath(selection); 

            StartCoroutine(endMiniGame()); 

        }
    }

    IEnumerator endMiniGame(){
        var mouth_phys = mouth.GetComponent<Transform>(); 
        mouth_phys.localScale += new Vector3(-0.45f, -0.25f, 0); 
        ParticleSystem ps = foodSmoke.GetComponent<ParticleSystem>(); 
        var fo = ps.forceOverLifetime; 
        fo.enabled = true; 
        fo.x = 20;
        yield return new WaitForSeconds(3); 
        fo.x = 0; 
        fo.enabled = false; 
        var em = ps.emission;
        em.rateOverTime = 1; 
        mouth_phys.localScale += new Vector3(0.45f, 0.25f, 0); 
        mouth.SetActive(false); 
        yield return new WaitForSeconds(2); 
        em.rateOverTime = 0; 
        textPrompt.text = "Click Start Breath button to play again"; 
        playButton.SetActive(true); 
        backButton.SetActive(true); 
        mapButton.SetActive(true); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
