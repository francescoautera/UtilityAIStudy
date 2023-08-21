using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SplineFollowerLogic : MonoBehaviour {
    
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] List<SplineComputer> splineComputers;
    private int indexSpline;

    private void Awake() {
        splineFollower.spline = splineComputers[0];
        indexSpline = 0;
    }

    private void Update() {
        if (splineFollower.GetPercent() >= 1) {
            splineFollower.SetPercent(0);
            indexSpline++;
            if (indexSpline % splineComputers.Count == 0) {
                indexSpline = 0;
            }
            splineFollower.spline = splineComputers[indexSpline];
        }
    }

}
