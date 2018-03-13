using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour {


    public GyroController gyroController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGUI()
	{
		
        if(GUILayout.Button("reset",new GUILayoutOption[]{GUILayout.Height(100),GUILayout.Width(100)})){


            gyroController.ResetTransform();
        }
	}
}
