using UnityEngine;
using UtilityAI;

public class ConstantParameter : Parameter
{
    [SerializeField] private float _value;

    public override float GetValue(Thinker thinker)
    {
        return _value;
    }
}
