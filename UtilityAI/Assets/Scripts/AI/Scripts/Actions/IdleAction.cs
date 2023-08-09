namespace UtilityAI
{
    public class IdleAction : Action {
        
     
        private bool endAction;

        public override void Execute(Thinker thinker, float deltaTime, bool needObject = false) {
            endAction = !needObject;
        }

        public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime, float actionRestore=-1, float actionTimer=-1) {
            endAction = true;
        }

        public override bool IsCompleted() {
            return endAction;
        }
    }
}
