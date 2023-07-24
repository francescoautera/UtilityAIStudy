using UnityEngine;
using System.Collections;

namespace UtilityAI
{
    public class EatAction : Action
    {
        public float EatTime;

        private Coroutine _coroutine;

        public override void Execute(Thinker thinker, float deltaTime)
        {
            _coroutine = thinker.StartCoroutine(EatCoroutine(thinker));
        }

        private IEnumerator EatCoroutine(Thinker thinker)
        {
            var character = thinker.GetComponent<Character>();
            if(character != null)
            {
                Debug.Log("Start eating");
                yield return new WaitForSeconds(EatTime);
                character.Hunger = Mathf.Clamp(character.Hunger + 50, 0, 100);
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
