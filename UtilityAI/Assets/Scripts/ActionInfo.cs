using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Action = UtilityAI.Action;

public class ActionInfo : MonoBehaviour {
    
    [SerializeField] private Image fillActionTimer;
    [SerializeField] private GameObject panel;
    public bool isActive;
    private float currentTimer;
    private float actionTimer;

    private void Awake() {
        isActive = false;
        panel.SetActive(false);
    }


    public void Init(float aTimer) {
        panel.SetActive(true);
        isActive = true;
        actionTimer = aTimer;
        currentTimer = 0;
    }

    private void Update() {
        if (isActive) {
            currentTimer += Time.deltaTime;
            fillActionTimer.fillAmount = Mathf.InverseLerp(0, actionTimer, currentTimer);
            if (currentTimer >= actionTimer) {
                FinishEvent();
            }
        }
    }

    public void FinishEvent() {
        isActive = false;
        currentTimer = 0;
        actionTimer = 0;
        panel.SetActive(false);
    }

}
