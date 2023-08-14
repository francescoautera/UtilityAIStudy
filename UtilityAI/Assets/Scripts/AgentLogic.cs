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
	private Tuple<float, float> actionValues = new Tuple<float, float>(-1,-1);

	private void Awake() {
		Agent.isStopped = true;
		character = GetComponent<Character>();

	}
	
	public void Move(ObjectLogic objectLogic,Action action) {
		_objectLogic = objectLogic;
		currentTransform = objectLogic.GetDestination().position;
		currentAction = action;
		actionValues = _objectLogic.ObjectRestoreParameters.GetActionValues(action);
		Agent.SetDestination(currentTransform);
		Agent.isStopped = false;
	}

	public void MoveOnRandomPoint(Action action) {
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
		currentTransform = finalPosition;
		currentAction = action;
		Agent.SetDestination(finalPosition);
		Agent.isStopped = false;
	}

	private void Update() {
		if (Agent.isStopped == false) {
			character.SetAnimatorState(moveAnimatorTrigger,true);
			if (Vector3.Distance(transform.position, currentTransform) < tresholdStopCharacter) {
				Agent.isStopped = true;
				if (_objectLogic != null) {
					ExecuteObjectLogic();
				}
				character.SetAnimatorState(moveAnimatorTrigger,false);
				currentAction.ExecuteActionAfterMovement(GetComponent<Thinker>(), Time.deltaTime, actionValues.Item1, actionValues.Item2);
				currentTransform = Vector3.zero;
				currentAction = null;
				_objectLogic = null;
				actionValues = new Tuple<float, float>(-1,-1);
			}
		}
	}

	private void ExecuteObjectLogic() {
		_objectLogic.FreeOccupiedPost();
		transform.position = currentTransform;
		var animTrigger = _objectLogic.ObjectRestoreParameters.animTrigger;
		character.SetAnimatorState(animTrigger,true);
		character.ActiveActionInfo(actionValues.Item1);
	}
}
