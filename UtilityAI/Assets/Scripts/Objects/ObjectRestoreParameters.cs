using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = UtilityAI.Action;

namespace Objects {

	
	[CreateAssetMenu(menuName = "Objects/ObjectRestore", fileName = "_Object", order = 0)]
	public class ObjectRestoreParameters : SerializedScriptableObject {
		
	
		public List<ActionParamsElement> ActionParamsElements = new List<ActionParamsElement>();
		public string animTrigger;

		public bool CanObjectSatisfyNeeded(Action action) {
			return ActionParamsElements.Any(actionParameter => actionParameter.Action.GetType() == action.GetType());
		}


		public float GetValueBasedOnAction(Action action) {
			
			foreach (var actionParameter in ActionParamsElements) {
				if (actionParameter.Action.GetType() == action.GetType()) {
					return actionParameter.actionRestore;
				}
			}
			return -1;
		}


		public Tuple<float, float> GetActionValues(Action action) {
			foreach (var actionParameter in ActionParamsElements) {
				if (actionParameter.Action.GetType() == action.GetType()) {
					return new Tuple<float, float>(actionParameter.actionTime, actionParameter.actionRestore);
				}
			}
			return new Tuple<float, float>(0, 0);
		}

	}

	[Serializable]
	public class ActionParamsElement {
		
		[SerializeReference]
		public Action Action;
		public float actionTime;
		public int actionRestore;

		public ActionParamsElement(Action action) {
			Action = action;
		}

		public float GetValueBasedOnParameters(float parameters) {
			return actionRestore;
		}
	}

}
