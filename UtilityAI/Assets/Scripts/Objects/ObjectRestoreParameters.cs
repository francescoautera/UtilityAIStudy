using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Action = UtilityAI.Action;

namespace Objects {

	
	[CreateAssetMenu(menuName = "Objects/ObjectRestore", fileName = "_Object", order = 0)]
	public class ObjectRestoreParameters : ScriptableObject {

		public List<ActionParamsElement> ActionParamsElements = new List<ActionParamsElement>();

		public bool CanObjectSatisfyNeeded(Action action) {
			return ActionParamsElements.Any(actions => actions.Action == action);
		}


		public float GetValueBasedOnAction(Action action) {
			
			return (from actions in ActionParamsElements where actions.Action == action select actions.GetValueBasedOnParameters(0)).FirstOrDefault();
		}


		public Tuple<float, float> GetActionValues(Action action) {
			foreach (var actionParamsElement in ActionParamsElements) {
				if (actionParamsElement.Action == action) {
					return new Tuple<float, float>(actionParamsElement.actionTime, actionParamsElement.actionRestore);
				}
			}
			return new Tuple<float, float>(0, 0);
		}

	}

	[Serializable]
	public class ActionParamsElement {
		
		public Action Action;
		public float actionTime;
		public int actionRestore;

		public float GetValueBasedOnParameters(float parameters) {
			return actionRestore;
		}
	}

}
