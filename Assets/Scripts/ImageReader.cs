using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageReader : MonoBehaviour
{

    public GameObject Cub;

    public Camera ARCamera;

  

    // Update is called once per frame
    void Update()
    {
        Cub = GameObject.FindWithTag("Cub");
        Cub.name = "Cub";

        var CubRenderer = Cub.GetComponent<Renderer>();

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                  
                        CubRenderer.material.SetColor("_Color", Color.yellow);

                }
            }
        }
    }
}
