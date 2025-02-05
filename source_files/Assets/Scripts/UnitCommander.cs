using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public GameObject selectionMarkerPrefab;
    public LayerMask layerMask;

    // components
    private UnitSelection unitSelection;
    private Camera cam;
    List<Unit> allButWarrior = new List<Unit>();
    List <Unit> minerUnits = new List<Unit>();
    List <Unit> warriorUnits = new List<Unit>(); 

    void Awake ()
    {
        unitSelection = GetComponent<UnitSelection>();
        cam = Camera.main;
    }

    void Update ()
    {
        if(Input.GetMouseButtonDown(1) && unitSelection.HasUnitsSelected())
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            unitSelection.RemoveNonExistingUnits();
            List<Unit> selectedUnits = unitSelection.GetSelectedUnits();
            

            foreach(Unit unit in selectedUnits){
                if(unit.typeUnit == 0){
                    allButWarrior.Add(unit);
                }
                if(unit.typeUnit == 1)
                {
                    minerUnits.Add(unit);
                    allButWarrior.Add(unit);
                }
                else if(unit.typeUnit == 2)
                {
                    warriorUnits.Add(unit);
                }
                else
                    continue;

            }


            // shoot the raycast
            if(Physics.Raycast(ray, out hit, 100, layerMask))
            {
                // are we clicking on the ground?
                if(hit.collider.CompareTag("Ground"))
                {
                    UnitsMoveToPosition(hit.point, selectedUnits);
                    CreateSelectionMarker(hit.point, false);
                }

                // did we click on a resource?
                else if(hit.collider.CompareTag("ResourceTree") || hit.collider.CompareTag("ResourceRock"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), allButWarrior);
                    CreateSelectionMarker(hit.collider.transform.position, true);
                }
                else if(hit.collider.CompareTag("ResourceGold"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), minerUnits);
                    CreateSelectionMarker(hit.collider.transform.position, true);
                }
                else if(hit.collider.CompareTag("BlacksmithBuilding"))
                {
                    UnitsUpgradeStats(hit.collider.gameObject.transform.position , warriorUnits);
                    Debug.Log(hit.collider.gameObject.transform.position);
                }
                else if(hit.collider.CompareTag("EnemyUnit"))
                {
                   Unit enemy = hit.collider.gameObject.GetComponent<Unit>();

                    UnitsAttack(enemy,selectedUnits);
                    CreateSelectionMarker(enemy.transform.position, false);
            
                }
            }
           allButWarrior.Clear();
           minerUnits.Clear();
           warriorUnits.Clear();
           }
           }

    void UnitsMoveToPosition (Vector3 movePos, List<Unit> units)
    {
        Vector3[] destinations = UnitMover.GetUnitGroupDestinations(movePos, units.Count, 2);

        for(int x = 0; x < units.Count; x++)
        {
            units[x].MoveToPosition(destinations[x]);
        }
    }

    void UnitsGatherResource (ResourceSource resource, List<Unit> units)
    {
        if(units.Count == 1)
        {
            units[0].GatherResource(resource, UnitMover.GetUnitDestinationAroundResource(resource.transform.position));
        }
        else
        {
            Vector3[] destinations = UnitMover.GetUnitGroupDestinationsAroundResource(resource.transform.position, units.Count);

            for(int x = 0; x < units.Count; x++)
            {
                units[x].GatherResource(resource, destinations[x]);
            }
        }
    }

    void UnitsUpgradeStats(Vector3 BlacksmithPos , List<Unit> units)
    {
        if(units.Count == 1)
        {
            units[0].UpgradeStats(UnitMover.GetUnitDestinationAroundResource(BlacksmithPos));
        }
        else
        {
            Vector3[] destinations = UnitMover.GetUnitGroupDestinationsAroundResource(BlacksmithPos, units.Count);

            for(int x = 0; x < units.Count; x++)
            {
                units[x].UpgradeStats(destinations[x]);
            }
        }
    }

    void UnitsAttack (Unit enemy, List<Unit> units)
    {
        foreach(Unit unit in units){
            unit.UnitAttack(enemy);
        }
    }

    void CreateSelectionMarker (Vector3 pos, bool large)
    {
        GameObject marker = Instantiate(selectionMarkerPrefab, new Vector3(pos.x, 0.01f, pos.z), Quaternion.identity);

        if(large)
            marker.transform.localScale = Vector3.one * 3;
    }
}