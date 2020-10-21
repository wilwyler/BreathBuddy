using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBtnClick : MonoBehaviour
{
    public void BtnNewScene() {
        SceneManager.LoadScene("Home-Scene");
    }
}
