using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Blueprint_script : MonoBehaviour
{
    public LayerMask layerMask;
    RaycastHit hit;
    public GameObject prefab;
    public GameObject const_prefab;
    public MeshRenderer rend;
    GameObject const_obj;
    private static Player player;
    static int i;
    static int call = 0;

    void Awake(){

       player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Start()
    {
        i = 0;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 100, layerMask ))
        {
            transform.position = hit.point;
        }  

    }

    void Update()
    {
        
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 100,layerMask ))
        {
            transform.position = hit.point;
        }

        StartCoroutine (InstatiateAfterTime());

    }
public IEnumerator InstatiateAfterTime(){


     if(Input.GetMouseButton(0))
        {
            Vector3 myposition = transform.position;
            rend.enabled = false;
            if(i==0)
            {
            const_obj = Instantiate(const_prefab,myposition,transform.rotation);
            i++;
            }
            if (call == 0)
            {
                yield return new WaitForSeconds(5);
                call++;
            }
            else if(call == 1)
            {
                yield return new WaitForSeconds(10);
                call++;
            }
            else
            {
                yield return new WaitForSeconds(15);
            }
            Debug.Log("call" + call);
            GameObject obj = Instantiate(prefab, myposition, transform.rotation,GameObject.Find("Player").transform);
            if(call == 1)
            { 
                player.Destroy_Garbage();
            }
            if(call == 2)
            {
                player.Destroy_Garbage();
            }
            
            Destroy(gameObject);
            Destroy(const_obj);
        }
    }
}
