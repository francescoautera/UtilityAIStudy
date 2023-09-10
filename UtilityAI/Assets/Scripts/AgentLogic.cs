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
	[SerializeField] float lastTimerUpdatePosition = 0.2f;
	private float updateTimePosition;
	private Tuple<float, float> actionValues = new Tuple<float, float>(-1,-1);
	private Vector3 lastPosition;

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
		randomDirection.y = transform.position.y;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomDirection, out hit, offsetShoot, 1)) {
			var result = hit.position;
			currentTransform = result;
			currentAction = action;
			Agent.SetDestination(result);
		}
		Agent.isStopped = false;
		NavMeshPath navMeshPath = new NavMeshPath();
		if (!Agent.CalculatePath(currentTransform,navMeshPath)) {
			Debug.Log("enter");
			MoveOnRandomPoint(currentAction);
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

		if(!Agent.isStopped) {
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
				else {
					if (currentAction is IdleAction && character.characterJob is Job.None) {
						
						TryCheckBlocked();
					}
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
		updateTimePosition += Time.deltaTime;
		if (updateTimePosition >= lastTimerUpdatePosition) {
			updateTimePosition = 0;
			if (Vector3.Distance(lastPosition, transform.position) < 0.1f) {
				Debug.Log("Change");
				currentTimer += Time.deltaTime;
				if (currentTimer >= timerDestination) {
					currentTimer = 0;
					MoveOnRandomPoint(currentAction);
				}
			}
			else {
				Debug.Log("Update");
				lastPosition = transform.position;
			}
		}
		
		// if (currentAction is IdleAction && character.characterJob is Job.None) {
		// 	currentTimer += Time.deltaTime;
		// 	if (currentTimer >= timerDestination) {
		// 		currentTimer = 0;
		// 		MoveOnRandomPoint(currentAction);
		// 	}
		// }
	}

}
