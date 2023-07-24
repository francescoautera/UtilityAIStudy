using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace UtilityAI
{
    [Serializable]
    public abstract class Action
    {
        [SerializeField] private string _title = "Action";

        [ListDrawerSettings(ListElementLabelName = "_title")]
        public Weight[] Weights = Array.Empty<Weight>();

        [SerializeField] protected bool WaitForComplete = true;

        public abstract void Execute(Thinker thinker, float deltaTime);

        public virtual bool NeedsToBeWaited()
        {
            return WaitForComplete && !IsCompleted();
        }

        public abstract bool IsCompleted();

        public float GetScore(Thinker thinker)
        {
            var totalValue = 0f;

            foreach(var weight in Weights)
            {
                var value = weight.Evaluate(thinker);
                switch(weight.Operator)
                {
                    case Operator.Sum:
                        totalValue += value;
                        break;
                    case Operator.Subtract:
                        totalValue -= value;
                        break;
                    case Operator.Multiply:
                        totalValue *= value;
                        break;
                    case Operator.Divide:
                        totalValue /= value;
                        break;
                    case Operator.Pow:
                        totalValue = MathF.Pow(totalValue, value);
                        break;
                }
            }
            return totalValue;
        }
    }
}
