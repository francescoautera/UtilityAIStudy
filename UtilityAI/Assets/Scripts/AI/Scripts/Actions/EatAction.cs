using UnityEngine;
using System.Collections;
using Objects;

namespace UtilityAI
{
    public class EatAction : Action
    {
        public float EatTime;
        public float eatRestore;
        private ObjectLogic currentObject;
        private Coroutine _coroutine;

        public override void Execute(Thinker thinker, float deltaTime, bool needObject) {
           
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
            throw new System.NotImplementedException();
        }

        private IEnumerator EatCoroutine(Thinker thinker)
        {
            var character = thinker.GetComponent<Character>();
            if(character != null)
            {
                Debug.Log("Start eating");
                yield return new WaitForSeconds(EatTime);
                character.Hunger = Mathf.Clamp(character.Hunger + eatRestore, 0, 100);
                Debug.Log("End eating");
            }
            _coroutine = null;
        }

        public override bool IsCompleted()
        {
            return _coroutine == null;
        }
    }
}
