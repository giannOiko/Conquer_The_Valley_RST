using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
   [SerializeField] GameObject PauseMenu;
   
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public GameObject button6;
    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button5.SetActive(false);
        button6.SetActive(false);
    }
    // Start is called before the first frame update
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        button4.SetActive(true);
        button5.SetActive(true);
        button6.SetActive(true);

    }

   
    
}
