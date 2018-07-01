using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour {

    public Transform eyePos;
    private Transform tr;

    public float minXRotation;
    public float maxXRotation;
	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        tr.position = eyePos.position;
        CameraRotation();
	}

    void CameraRotation()
    {
        
        float mouseY = -Input.GetAxis("Mouse Y");

        float XAngle = tr.rotation.eulerAngles.x;
        XAngle -= 360.0f;
        if (XAngle < -270.0f)
        {
            XAngle += 360.0f;
        }
        if (XAngle + mouseY >= minXRotation && XAngle + mouseY <= maxXRotation)
        {
            tr.Rotate(new Vector3(mouseY, 0.0f));
        }
    }
}
