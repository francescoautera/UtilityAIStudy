using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI;

namespace Objects {

	public class ObjectLogicContainer : Singleton<ObjectLogicContainer> {

		public List<ObjectLogic> objects = new List<ObjectLogic>();
		

		public ObjectLogic GetObject(Action action,Thinker thinker) {
			var objectsCopy = new List<ObjectLogic>();
			
			foreach (ObjectLogic objectParam in objects) {
				var objectParameter = objectParam.ObjectRestoreParameters;
				if (objectParameter.CanObjectSatisfyNeeded(action)) {
					objectsCopy.Add(objectParam);
				}
			}
			
			return objectsCopy.Count == 0 ? objects[0] : GetMaxRestoreObject(objectsCopy, action);
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

	}

}
