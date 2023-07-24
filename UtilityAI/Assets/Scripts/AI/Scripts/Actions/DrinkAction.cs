using UnityEngine;
using System.Collections;

namespace UtilityAI
{
    public class DrinkAction : Action
    {
        public float DrinkTime;

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
                Debug.Log("Start drinking");
                yield return new WaitForSeconds(DrinkTime);
                character.Thirst = Mathf.Clamp(character.Thirst + 70, 0, 100);
                Debug.Log("End drinking");
            }
            _coroutine = null;
        }

        public override bool IsCompleted()
        {
            return _coroutine == null;
        }
    }
}
