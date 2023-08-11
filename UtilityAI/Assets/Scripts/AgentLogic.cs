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
	private Collider terrainCollider;
	[SerializeField] private string moveAnimatorTrigger;
	private ObjectLogic _objectLogic;
	private Tuple<float, float> actionValues = new Tuple<float, float>(-1,-1);

	private void Awake() {
		Agent.isStopped = true;
		terrainCollider = Plane.Instance.GetComponent<Collider>();
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
		var bounds = terrainCollider.bounds;
		var xPos = Random.Range(bounds.min.x, bounds.max.x);
		var zPos = Random.Range(bounds.min.z, bounds.max.z);
		var destination = new Vector3(xPos, transform.position.y, zPos);
		currentTransform = destination;
		currentAction = action;
		Agent.SetDestination(destination);
		Agent.isStopped = false;
	}

	public void MoveToSpecificPosition(Action action, Vector3 finalPosition) {
		currentTransform = finalPosition;
		currentAction = action;
		Agent.SetDestination(finalPosition);
		Agent.isStopped = false;
	}

	private void Update() {
		if (Agent.isStopped == false) {
			character.SetAnimatorState(moveAnimatorTrigger);
			if (Vector3.Distance(transform.position, currentTransform) < tresholdStopCharacter) {
				Agent.isStopped = true;
				_objectLogic.FreeOccupiedPost();
				currentAction.ExecuteActionAfterMovement(GetComponent<Thinker>(), Time.deltaTime, actionValues.Item1, actionValues.Item2);
				currentTransform = Vector3.zero;
				currentAction = null;
				_objectLogic = null;
				actionValues = new Tuple<float, float>(-1,-1);
			}
		}
	}
}
