using System;
using System.Collections;
using System.Collections.Generic;
using CharacterData;
using Objects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UtilityAI;
using Action = UtilityAI.Action;
using Random = UnityEngine.Random;

public class AgentLogic : MonoBehaviour {

	public NavMeshAgent Agent;
	[SerializeField,ReadOnly] Vector3 currentTransform;
	private Action currentAction;
	private Character character;
	public float tresholdStopCharacter;
	[SerializeField] private float offsetShoot;
	[SerializeField] private string moveAnimatorTrigger;
	[SerializeField] ObjectLogic _objectLogic;
	[SerializeField] private float timerDestination;
	[SerializeField,ReadOnly] float currentTimer;
	private Tuple<float, float> actionValues = new Tuple<float, float>(-1,-1);

	public Action CurrentAction => currentAction;

	private void Awake() {
		Agent.isStopped = true;
		character = GetComponent<Character>();

	}
	
	public void Move(ObjectLogic objectLogic,Action action) {
		UpdateAgent(true);
		_objectLogic = objectLogic;
		currentTransform = objectLogic.GetDestination().position;
		currentAction = action;
		actionValues = _objectLogic.ObjectRestoreParameters.GetActionValues(action);
		Agent.SetDestination(currentTransform);
		Agent.isStopped = false;
	}

	[Button]
	public void MoveOnRandomPoint(Action action) {
		UpdateAgent(true);
		var randomDirection = Random.insideUnitSphere * offsetShoot;
		randomDirection += transform.position;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomDirection, out hit, offsetShoot, 1)) {
			var result = hit.position;
			currentTransform = result;
			currentAction = action;
			Agent.SetDestination(result);
			Agent.isStopped = false;
		}
		
		
	}

	public void MoveToSpecificPosition(Action action, Vector3 finalPosition) {
		if (Vector3.Distance(transform.position, finalPosition) > tresholdStopCharacter) {
			UpdateAgent(true);
		}
		currentTransform = finalPosition;
		currentAction = action;
		Agent.SetDestination(finalPosition);
		Agent.isStopped = false;
	}

	private void Update() {
		if(!Agent.enabled)
			return;
		
		switch (Agent.isStopped) {
			case false when currentAction == null && character.characterJob is Job.None :
				TryCheckBlocked();
				return;
			case false when (character.characterJob is Job.None && currentAction is IdleAction): 
				character.SetAnimatorState(moveAnimatorTrigger,true);
				TryCheckBlocked();
				return;
			case false : {
				character.SetAnimatorState(moveAnimatorTrigger,true);
				if (Vector3.Distance(transform.position, currentTransform) <= tresholdStopCharacter) {
					Agent.isStopped = true;
					if (_objectLogic != null) {
						ExecuteObjectLogic();
					}
					character.SetAnimatorState(moveAnimatorTrigger,false);
					UpdateAgent(false);
					currentAction.ExecuteActionAfterMovement(GetComponent<Thinker>(), Time.deltaTime, actionValues.Item1, actionValues.Item2);
					currentTransform = Vector3.zero;
					currentAction = null;
					_objectLogic = null;
					actionValues = new Tuple<float, float>(-1,-1);
				}
				break;
			}
		}
	}

	private void ExecuteObjectLogic() {
		transform.position = currentTransform;
		var animTrigger = _objectLogic.ObjectRestoreParameters.animTrigger;
		character.SetAnimatorState(animTrigger,true);
		character.ActiveActionInfo(actionValues.Item1);
	}

	public void UpdateAgent(bool active) {
		Agent.enabled = active;
	}

	private void TryCheckBlocked() {
		if (currentAction is IdleAction && character.characterJob is Job.None) {
			currentTimer += Time.deltaTime;
			if (currentTimer >= timerDestination) {
				currentTimer = 0;
				MoveOnRandomPoint(currentAction);
			}
		}
	}

}
