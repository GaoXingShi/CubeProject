using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseDown : MonoBehaviour
{
    public NucleonSpawner controllerNucleon;
	// Use this for initialization
	void OnMouseDown ()
	{
	    controllerNucleon.BombMethod();
	}
	
}