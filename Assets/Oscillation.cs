﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillation : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    //todo remove from inspector later
    [Range(0,1)]
    [SerializeField]
    float movementFactor;  // 0 for not moved, 1 for fully moved

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
       
        if (period <= Mathf.Epsilon) { return; } //protects against NaN error
        
        float cycles = Time.time / period;
        

        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offest = movementVector * movementFactor;
        transform.position = startingPos + offest;
	}
}
