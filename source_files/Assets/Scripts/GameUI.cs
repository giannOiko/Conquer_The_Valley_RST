using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI unitCountText;
    public TextMeshProUGUI WoodText;
    public TextMeshProUGUI StoneText;
    public TextMeshProUGUI GoldText;

    public static GameUI instance;

    void Awake ()
    {
        instance = this;
    }

    public void UpdateUnitCountText (int value)
    {
        unitCountText.text = value.ToString();
    }

    public void UpdateWoodText (int value)
    {
        WoodText.text = value.ToString();
    }

    public void UpdateStoneText(int value)
    {
        StoneText.text = value.ToString();
    }

    public void UpdateGoldText(int value)
    {
        GoldText.text = value.ToString();
    }

    public void ToMainScreen(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}