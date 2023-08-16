using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CharacterData;
using Objects;

namespace UtilityAI
{
    public class DrinkAction : Action {
        
        
        public float drinkTime;
        public float drinkRestore;
        public bool IsActionWithObject;
         

        public override void Execute(Thinker thinker, float deltaTime,bool needObject = false)
        {
            if (!needObject) {
                thinker.StartCoroutine(DrinkCoroutine(thinker));
            }
            IsActionWithObject = true;
        }
        

        public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime, float actionTimer = -1, float actionRestore = -1) {
           thinker.StartCoroutine(DrinkCoroutine(thinker,actionRestore,actionTimer));
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
            OnEndAction?.Invoke(this,thinker);
        }
        
        

        public override bool IsCompleted()
        {
            return !IsActionWithObject;
        }

        public override IEnumerator GetEnumerator(Thinker thinker) {
            return DrinkCoroutine(thinker);
        }

    }
}
