using System.Collections;
using CharacterData;
using UnityEngine;

namespace UtilityAI {

	public class SleepAction : Action {
		
		public float sleepTimer;
		public float sleepRestore;
		private bool isInAction;
		private bool needObject;
        
		public override void Execute(Thinker thinker, float deltaTime, bool needObject = false) {
			if (!needObject) {
				thinker.StartCoroutine(SleepCoroutine(thinker,deltaTime));
			}
			isInAction = true;
		}

		public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime, float actionTimer = -1, float actionRestore = -1) {
			thinker.StartCoroutine(SleepCoroutine( thinker, actionTimer, actionRestore));
		}

		private IEnumerator SleepCoroutine(Thinker thinker, float actionTimer = -1, float actionRestore = -1) {
			var character = thinker.GetComponent<Character>();
			var sleepTimerActual = sleepTimer;
			var sleepRestoreActual = sleepRestore;
            
			if (actionRestore!= -1) {
				sleepTimerActual = actionTimer;
				sleepRestoreActual = actionRestore;
			}
            
			if(character != null) {
				
				yield return new WaitForSeconds(sleepTimerActual);
				character.Sleepy = Mathf.Clamp(character.Sleepy + sleepRestoreActual, 0, 100);
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
			return SleepCoroutine(thinker);
		}
	}

}