namespace UtilityAI
{
    public class IdleAction : Action
    {
        public override void Execute(Thinker thinker, float deltaTime,bool needObject) { }

        public override void ExecuteActionAfterMovement(Thinker thinker, float deltaTime) {
            throw new System.NotImplementedException();
        }

        public override bool IsCompleted()
        {
            return true;
        }
    }
}
