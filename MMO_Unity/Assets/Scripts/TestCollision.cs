using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 look = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position+Vector3.up,look*10,Color.red);
        RaycastHit hit;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, look, 10);

        foreach(RaycastHit hi in hits)
        {
            Debug.Log($"RayCast {hi.collider.gameObject.name}");
        }

        if (Physics.Raycast(transform.position, look,out hit, 10))
        {
            Debug.Log($"RayCast {hit.collider.gameObject.name}");
        }

        if(Input.GetMouseButtonDown(0))
        {
            
        }
        
    }
}
