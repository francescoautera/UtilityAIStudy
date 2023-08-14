using System.Collections;
using CharacterData;
using UnityEngine;

namespace UtilityAI {

	public class FunAction : Action {
		
		public float funTimer;
		public float funRestore;
		private bool isInAction;
		private bool needObject;
        
		public override void Execute(Thinker thinker, float deltaTime, bool needObject = false) {
			if (!needObject) {
				thinker.StartCoroutine(FunCoroutine(thinker,deltaTime));
			}
			isInAction = true;
		}

		public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime, float actionTimer = -1, float actionRestore = -1) {
			thinker.StartCoroutine(FunCoroutine( thinker, actionTimer, actionRestore));
		}

		private IEnumerator FunCoroutine(Thinker thinker, float actionTimer = -1, float actionRestore = -1) {
			var character = thinker.GetComponent<Character>();
			var funTimerActual = funTimer;
			var funRestoreActual = funRestore;
            
			if (actionRestore!= -1) {
				funTimerActual = actionTimer;
				funRestoreActual = actionRestore;
			}
            
			if(character != null)
			{
				yield return new WaitForSeconds(funTimerActual);
				character.Fun = Mathf.Clamp(character.Sleepy + funRestoreActual, 0, 100);
			}
			OnEndAction?.Invoke(this);
            
			if (!needObject) {
				isInAction = false;
			}
            
		}

		public override bool IsCompleted() {
			return !isInAction;
		}

		public override IEnumerator GetEnumerator(Thinker thinker) {
			return FunCoroutine(thinker);
		}
		
		
	}

}