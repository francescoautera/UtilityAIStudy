using System;
using UnityEngine;
using System.Collections;
using Objects;

namespace UtilityAI
{
    public class DrinkAction : Action {
        
        
        public float drinkTime;
        public float drinkRestore;
        private ObjectLogic currentObject;
        
        private Coroutine _coroutine;

        public override void Execute(Thinker thinker, float deltaTime,bool needObject)
        {
            if (!needObject) {
                _coroutine = thinker.StartCoroutine(EatCoroutine(thinker));
            }
            else {
                currentObject = GetObject(thinker, this);
                thinker.GetComponent<AgentLogic>().Move(currentObject,this);
            }
        }
        
        private ObjectLogic GetObject(Thinker thinker, Action action) {
            return ObjectLogicContainer.Instance.GetObject(action, thinker);
        }
        

        public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime) {
            _coroutine = thinker.StartCoroutine(EatCoroutine(thinker, currentObject.ObjectRestoreParameters));
        }



        private IEnumerator EatCoroutine(Thinker thinker,ObjectRestoreParameters objectRestoreParameters = null)
        {
            var character = thinker.GetComponent<Character>();
            if (objectRestoreParameters != null) {
                var values =objectRestoreParameters.GetActionValues(this);
                drinkTime = values.Item1;
                drinkRestore = values.Item2;
            }
            
            if(character != null)
            {
                Debug.Log("Start drinking");
                yield return new WaitForSeconds(drinkTime);
                character.Thirst = Mathf.Clamp(character.Thirst + drinkRestore, 0, 100);
                Debug.Log("End drinking");
            }
            _coroutine = null;
            currentObject = null;
        }

        public override bool IsCompleted()
        {
            return _coroutine == null;
        }
    }
}
