using System.Collections;
using CharacterData;

namespace UtilityAI
{
    public class IdleAction : Action {
        
     
        private bool endAction;

        public override void Execute(Thinker thinker, float deltaTime, bool needObject = false) {
            endAction = !needObject;
        }

        public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime, float actionRestore=-1, float actionTimer=-1) {
            var character = thinker.GetComponent<Character>();
            if (character.characterJob is not Job.None) {
                character.SetIdle();
            }
            endAction = true;
        }

        IEnumerator IdleCor() {
            yield return null;
        }

        public override bool IsCompleted() {
            return endAction;
        }

        public override IEnumerator GetEnumerator(Thinker thinker) {
            return IdleCor();
        }
    }
}
