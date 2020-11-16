using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SaveUsers : MonoBehaviour {
	public Text username_field;
	public Button PlayButton;
	public string[] usernames;

    void Start() {
        var numUsers = PlayerPrefs.GetInt("NumUsers", 0); // get total num of users
        Debug.Log("number of users:" + numUsers);
		usernames = new string[numUsers+1]; // create the user name array
		for (var n = 1; n <= numUsers; n++) {
			usernames[n] = PlayerPrefs.GetString("User"+n, "Player"); // load users
 			Debug.Log("user " + n + ": " + usernames[n]);
 		}
 		UserCredentials();
    }

    void UserCredentials() {
        PlayButton.onClick.AddListener(SaveUser);
        void SaveUser() {
        	Debug.Log("button clicked");
        	string username = username_field.text.ToString();
        	// Debug.Log("username:" + username);
        	if (!usernames.Contains(username)) {
	        	var numUser = PlayerPrefs.GetInt("NumUsers", 0);
				numUser += 1; // count this user
				PlayerPrefs.SetInt("NumUsers", numUser); // update total users count
				PlayerPrefs.SetString("User"+numUser, username);
	        }
	        PlayerPrefs.SetString("CurrentUser", username);
        }
        // to reset user data
        // PlayerPrefs.DeleteAll();
    }
}