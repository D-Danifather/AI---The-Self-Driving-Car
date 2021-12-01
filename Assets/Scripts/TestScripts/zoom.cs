using UnityEngine;
using System.Collections;

public class zoom : MonoBehaviour {


    void Update() {
        // -------------------Code for Zooming Out------------
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 95)
                Camera.main.fieldOfView += 5;
            if (Camera.main.orthographicSize <= 90)
                Camera.main.orthographicSize += 3f;

        }
        // ---------------Code for Zooming In------------------------
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 10)
                Camera.main.fieldOfView -= 5;
            if (Camera.main.orthographicSize >= 1)
                Camera.main.orthographicSize -= 3f;
        }

        if (Input.GetButton("ButtonX"))
        {
            if (Input.GetAxis("Look Y") > 0)
            {
                if (Camera.main.fieldOfView > 10)
                    Camera.main.fieldOfView -= 0.2f;         
            }
            else if (Input.GetAxis("Look Y") < 0)
            {
                if (Camera.main.fieldOfView <= 95)
                    Camera.main.fieldOfView += 0.2f;            
            }
        }
    }
}



