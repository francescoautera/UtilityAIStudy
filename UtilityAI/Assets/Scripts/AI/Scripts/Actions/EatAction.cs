using UnityEngine;
using System.Collections;
using CharacterData;
using Objects;

namespace UtilityAI
{
    public class EatAction : Action
    {
        public float EatTime;
        public float eatRestore;
        private Coroutine _coroutine;
        private bool isInAction;
        private bool isActionWithObject;
        

        public override void Execute(Thinker thinker, float deltaTime, bool needObject = false) {
           
            if (!needObject) {
                _coroutine = thinker.StartCoroutine(EatCoroutine(thinker));
                isInAction = true;
            }
            isActionWithObject = true;

        }
        
        
        public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime, float actionTimer=-1, float actionRestore=-1) {
            _coroutine = thinker.StartCoroutine(EatCoroutine(thinker,actionRestore,actionTimer ));
        }

        private IEnumerator EatCoroutine(Thinker thinker,float actionRestore = -1 ,float actionTimer = -1)
        {
            var character = thinker.GetComponent<Character>();
            var eatTimerActual = EatTime;
            var eatRestoreActual = eatRestore;
            
            if (actionRestore!= -1) {
                eatTimerActual = actionTimer;
                eatRestoreActual = actionRestore;
            }
            
            if(character != null)
            {
                yield return new WaitForSeconds(eatTimerActual);
                character.Hunger = Mathf.Clamp(character.Hunger + eatRestoreActual, 0, 100);
            }
            
            OnEndAction?.Invoke(this,thinker);
        }

        public override bool IsCompleted()
        {
            return !isActionWithObject;
        }

        public override IEnumerator GetEnumerator(Thinker thinker) {
            return EatCoroutine(thinker);
        }
    }

}
