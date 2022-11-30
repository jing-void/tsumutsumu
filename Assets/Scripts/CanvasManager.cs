using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CanvasManager : MonoBehaviour
{
    

    public void LoadRetry()
    {
        SceneManager.LoadScene("Main");
    }


    public void LoadExit()
    {       
        SceneManager.LoadScene("Title");
    }
  
    public void StopBGM()
    {
        SoundController.instance.StopMainBGM();
        SoundController.instance.PlayBGM(SoundController.SelectSound.Title);
    }
}
