using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {
    
    public static Plane Instance;

    private void Awake() {
        Instance = this;
    }
}
