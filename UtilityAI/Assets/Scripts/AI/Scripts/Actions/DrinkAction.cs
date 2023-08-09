using System;
using UnityEngine;
using System.Collections;
using CharacterData;
using Objects;

namespace UtilityAI
{
    public class DrinkAction : Action {
        
        
        public float drinkTime;
        public float drinkRestore;
       
        
        private Coroutine _coroutine;

        public override void Execute(Thinker thinker, float deltaTime,bool needObject = false)
        {
            if (!needObject) {
                _coroutine = thinker.StartCoroutine(DrinkCoroutine(thinker));
            }
            
        }
        

        public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime, float actionTimer = -1, float actionRestore = -1) {
            _coroutine = thinker.StartCoroutine(DrinkCoroutine(thinker,actionRestore,actionTimer));
        }



        private IEnumerator DrinkCoroutine(Thinker thinker,float actionRestore = -1, float actionTimer = -1)
        {
            
            var character = thinker.GetComponent<Character>();
            var drinkTimerActual = drinkTime;
            var drinkRestoreActual = drinkRestore;
            
            if (actionRestore!= -1) {
                drinkTimerActual = actionTimer;
                drinkRestoreActual = actionRestore;
            }
            
            if(character != null)
            {
                yield return new WaitForSeconds(drinkTimerActual);
                character.Thirst = Mathf.Clamp(character.Thirst + drinkRestoreActual, 0, 100);
            }
            _coroutine = null;
        }

        public override bool IsCompleted()
        {
            return _coroutine == null;
        }
    }
}
