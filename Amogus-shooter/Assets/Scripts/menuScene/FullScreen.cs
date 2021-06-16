using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{
    public void FullScene(bool isFullScene)
    {
        Screen.fullScreen = isFullScene;
    }
}
