using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosetBtnClick : MonoBehaviour
{
    public void BtnNewScene() {
        SceneManager.LoadScene("Closet-Scene");
    }
}
