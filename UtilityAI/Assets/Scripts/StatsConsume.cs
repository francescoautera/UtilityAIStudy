using UnityEngine;

[RequireComponent(typeof(Character))]
public class StatsConsume : MonoBehaviour
{
    public float HealthGainedPerSecond = 0;
    public float HungerConsumePerSecond = 2;
    public float ThirstConsumePerSecond = 4;

    [SerializeField] private AnimationCurve hungerBasedHealthModifier = default;
    [SerializeField] private AnimationCurve thirstBasedHealthModifier = default;

    private Character _character = null;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        HealthGainedPerSecond = 
            hungerBasedHealthModifier.Evaluate(_character.Hunger) + 
            thirstBasedHealthModifier.Evaluate(_character.Thirst);

        _character.Health = Mathf.Clamp(_character.Health + HealthGainedPerSecond * deltaTime, 0, 100);
        _character.Hunger = Mathf.Clamp(_character.Hunger - HungerConsumePerSecond * deltaTime, 0, 100);
        _character.Thirst = Mathf.Clamp(_character.Thirst - ThirstConsumePerSecond * deltaTime, 0, 100);
    }
}
