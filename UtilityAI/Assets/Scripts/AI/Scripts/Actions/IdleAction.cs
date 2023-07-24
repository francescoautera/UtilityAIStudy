namespace UtilityAI
{
    public class IdleAction : Action
    {
        public override void Execute(Thinker thinker, float deltaTime) { }

        public override bool IsCompleted()
        {
            return true;
        }
    }
}
