using System;
using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using UnityEngine.AI;
using UtilityAI;
using Action = UtilityAI.Action;

public class AgentLogic : MonoBehaviour {

	public NavMeshAgent Agent;
	private Transform currentTransform;
	private Action currentAction;
	public float tresholdStopCharacter;
	

	public void Move(ObjectLogic objectLogic,Action action) {
		currentTransform = objectLogic.transform;
		currentAction = action;
		Agent.SetDestination(currentTransform.position);
	}

	private void Update() {
		if (Agent.isStopped == false) {
			if (Vector3.Distance(transform.position, currentTransform.position) < tresholdStopCharacter) {
				Agent.isStopped = true;
				currentAction.ExecuteActionAfterMovement(GetComponent<Thinker>(), Time.deltaTime);
				currentTransform = null;
				currentAction = null;
			}
		}
	}
}
