using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [Header("Units")]
    public List<Unit> units = new List<Unit>();
    public List<Unit> enemyunits = new List<Unit>();

    [Header("Resources")]
    public int wood;
    public int rock;
    public int gold;

    [Header("Components")]
    public GameObject unitPrefab;
    public GameObject minerUnitPrefab;
    public GameObject warriorUnitPrefab;
    public GameObject minerBlueprint;
    public GameObject warriorBlueprint;
    public GameObject blacksmithBlueprint;
    public Transform unitSpawnPos;
    public GameObject MinerBuildingConstructionButton;
    public GameObject WarriorBuildingConstructionButton;
    public GameObject BlacksmithBuildingConstructionButton;
    public GameObject Win;
    public GameObject Lose;

    static float time = 0;

    public readonly int _25Cost = 25;
    public readonly int _50Cost = 50;
    public readonly int _100Cost = 100;
    public readonly int _200Cost = 200;
    bool winorlose;

    
    void Start ()
    {
        time = 5;
        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateWoodText(wood);
        GameUI.instance.UpdateStoneText(rock);
        GameUI.instance.UpdateGoldText(gold);

        wood += _25Cost;
        CreateNewUnit();
    }

    void Update()
    {
        winorlose = WinLoseCondition();
        if(winorlose)
        {
            if(time > 0)
            {
                time-=Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + -1);
            }
        }
    }

    // called when a unit gathers a certain resource
    public void GainResource (ResourceType resourceType, int amount)
    {
        switch(resourceType)
        {
            case ResourceType.Wood:
            {
                wood += amount;
                GameUI.instance.UpdateWoodText(wood);
                break;
            }

            case ResourceType.Rock:
            {
                rock += amount;
                GameUI.instance.UpdateStoneText(rock);
                break;
            }
            case ResourceType.Gold:
            {
                gold += amount;
                GameUI.instance.UpdateGoldText(gold);
                break;
            }
        }
    }
  
    // creates a new unit for the player
    public void CreateNewUnit ()
    {
        if(wood - _25Cost < 0)
            return;

        GameObject unitObj = Instantiate(unitPrefab, unitSpawnPos.position, Quaternion.identity, transform);
        Unit unit = unitObj.GetComponent<Unit>();

        units.Add(unit);
        wood -= _25Cost;

        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateWoodText(wood);
    }

    public void CreateNewMinerUnit()
    {
        if(gameObject.transform.Find("MinerSpawn(Clone)") == null)
            return;
        if(wood - _50Cost < 0)
            return;
        
        Vector3 pos = gameObject.transform.Find("MinerSpawn(Clone)").GetChild(1).transform.position;
        GameObject mineunit = Instantiate(minerUnitPrefab,pos,Quaternion.identity,transform);
        Unit unit = mineunit.GetComponent<Unit>();

        units.Add(unit);
        wood -= _50Cost;
        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateWoodText(wood);
    }

    

    public void CreateNewWarriorUnit()
    {
        if(gameObject.transform.Find("WarriorSpawn Variant(Clone)") == null)
            return;

        if(wood - _100Cost < 0)
            return;

        Vector3 pos = gameObject.transform.Find("WarriorSpawn Variant(Clone)").GetChild(1).transform.position;
        GameObject mineunit = Instantiate(warriorUnitPrefab,pos,Quaternion.identity,transform);
        Unit unit = mineunit.GetComponent<Unit>();

        units.Add(unit);
        wood -= _100Cost;
        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateWoodText(wood);
    }

    public void CreateNewMinerBuilding()
    {
        if(wood - _50Cost < 0 || rock - _50Cost < 0)
            return;

        GameObject houseobj = Instantiate(minerBlueprint);
        wood -= _50Cost;
        rock -= _50Cost;
        GameUI.instance.UpdateWoodText(wood);
        GameUI.instance.UpdateStoneText(rock);

        MinerBuildingConstructionButton.SetActive(false);

    }

    public void CreateNewWarriorBuilding()
    {
        if(gameObject.transform.Find("MinerSpawn(Clone)") == null)
            return;

        if(rock - _100Cost < 0 || gold - _100Cost < 0)
            return;

        GameObject houseobj = Instantiate(warriorBlueprint);
        rock-=_100Cost;
        gold-=_100Cost;
        GameUI.instance.UpdateStoneText(rock);
        GameUI.instance.UpdateGoldText(gold);
        WarriorBuildingConstructionButton.SetActive(false);

    }

    public void CreateNewBlackSmithBUilding(){
        if(gameObject.transform.Find("WarriorSpawn Variant(Clone)") == null)
            return;

        if(rock - _200Cost < 0 || gold - _200Cost < 0)
            return;
        GameObject houseobj = Instantiate(blacksmithBlueprint);

        rock-=_200Cost;
        gold-=_200Cost;
        GameUI.instance.UpdateStoneText(rock);
        GameUI.instance.UpdateGoldText(gold);
        BlacksmithBuildingConstructionButton.SetActive(false);
    }


    public void Destroy_Garbage()
    {
       
        int k = 0;
        int l = 0;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
           if (gameObject.transform.GetChild(i).CompareTag("MinerSpawn"))
           {
                Debug.Log("Found1");
                k++;
                if(k == 2)
                {
                    Destroy(gameObject.transform.GetChild(i).gameObject);
                    Debug.Log("Destroy!!");
                }
           }

           if (gameObject.transform.GetChild(i).CompareTag("WarriorSpawn"))
           {
                Debug.Log("Found1");
                l++;
                if(l == 2)
                {
                    Destroy(gameObject.transform.GetChild(i).gameObject);
                    Debug.Log("Destroy!!");
                }

           }
        }
    }

    public bool WinLoseCondition()
    {
        if(enemyunits.Count == 0)
        {
            Win.SetActive(true);
            return true;
        }
        else if(units.Count ==0 && wood < 25)
        {
            Lose.SetActive(true);
            return true;
        }
        else return false;
    }
}
