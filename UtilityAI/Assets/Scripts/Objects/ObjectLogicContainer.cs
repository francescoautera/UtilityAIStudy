using System;
using System.Collections;
using System.Collections.Generic;
using CharacterData;
using UnityEngine;
using UtilityAI;
using Action = UtilityAI.Action;

namespace Objects {

	public class ObjectLogicContainer : Singleton<ObjectLogicContainer> {

		public List<ObjectLogic> objects = new List<ObjectLogic>();
		public List<Thinker> Thinkers = new List<Thinker>();

		public override void Awake() {
			Thinker.OnThinkerBorn += RegisterElement;
		}

		private void OnDestroy() {
			Thinker.OnThinkerBorn -= RegisterElement;
		}

		public void  SelectRestorer(Action action,Thinker thinker) {
			var objectsCopy = new List<ObjectLogic>();
			
			foreach (ObjectLogic objectParam in objects) {
				if(objectParam.IsOccupied())
					continue;
				
				var objectParameter = objectParam.ObjectRestoreParameters;
				if (objectParameter.CanObjectSatisfyNeeded(action)) {
					objectsCopy.Add(objectParam);
				}
			}

			if (objectsCopy.Count == 0) {
				var character = thinker.GetComponent<Character>();
				if (character.characterJob is not Job.None) {
					thinker.GetComponent<AgentLogic>().MoveToSpecificPosition(action,character.InitPosition);
					return;
				}
				thinker.GetComponent<AgentLogic>().MoveOnRandomPoint(action);
			}
			else {
				var objectRestore = GetMaxRestoreObject(objectsCopy, action);
				objectRestore.IncreaseOccupiedPost(action,thinker);
				thinker.GetComponent<AgentLogic>().Move(objectRestore,action);
			}
		}


		private ObjectLogic GetMaxRestoreObject(List<ObjectLogic> objectsParam,Action action) {

			var index = -1;
			var value = 0f;

			foreach (var objectParam in objectsParam) {
				var objectParameter = objectParam.ObjectRestoreParameters;
				var currentValue = objectParameter.GetValueBasedOnAction(action);
				if ( currentValue > value) {
					index = objectsParam.IndexOf(objectParam);
					value = currentValue;
				}
			}
			
			return objectsParam[index];
		}


		private void RegisterElement(Thinker thinker) {
			Thinkers.Add(thinker);
			thinker.OnObjectSatisfyRequested += SelectRestorer;
			
		}
	}

}
