using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SimulateMouseClick();        
    }

    void SimulateMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
            hit.collider.SendMessage("OnMouseDown",SendMessageOptions.DontRequireReceiver);

    }
}
