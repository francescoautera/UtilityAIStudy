using Sirenix.OdinInspector;
using UnityEngine;

namespace UtilityAI
{
    public enum Operator
    {
        Sum,
        Subtract,
        Multiply,
        Divide,
        Pow
    }

    public class Weight
    {
        [SerializeField] private string _title = "Weight";
        [HorizontalGroup, HideLabel, InlineProperty, Required]
        public Parameter Parameter = null;
        [HorizontalGroup, HideLabel, InlineProperty, Required]
        public AnimationCurve Curve = default;
        public Operator Operator = Operator.Sum;

        [SerializeField, ReadOnly] private float _lastValue = 0f;

        public float Evaluate(Thinker thinker)
        {
            var evaluationValue = Curve.Evaluate(Parameter.GetValue(thinker));
            _lastValue = evaluationValue;
            return evaluationValue;
        }
    }
}
