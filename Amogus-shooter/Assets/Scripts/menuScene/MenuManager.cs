using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnClickStartButton(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }
}
