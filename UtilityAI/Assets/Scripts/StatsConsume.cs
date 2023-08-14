using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using StatsConsumeData;
using UnityEngine;
using UtilityAI;


namespace CharacterData {
    [RequireComponent(typeof(Character))]
    public class StatsConsume : MonoBehaviour {
        
        [Title("StatsConsume")]
        public float HealthGainedPerSecond = 0;
        public float HungerConsumePerSecond = 2;
        public float ThirstConsumePerSecond = 4;
        public float FunConsumePerSecond = 1;
        public float SleepyConsumePerSecond = 2;
        public bool randomizeStatsAtStart;
        [ShowIf("randomizeStatsAtStart")] public Vector2 consumeMinMax = new Vector2(1,5);

        [Title("StatsModifier")]
        [SerializeField,ReadOnly] float modifierHungerConsume = 1f;
        [SerializeField,ReadOnly] float modifierThirstConsume = 1f;
        [SerializeField, ReadOnly] private float modifierFunConsume = 1f;
        [SerializeField, ReadOnly] private float modifierSleepyConsume = 1f;

        [Title("HealthModifier")]
        [SerializeField] private AnimationCurve hungerBasedHealthModifier = default;
        [SerializeField] private AnimationCurve thirstBasedHealthModifier = default; 
        [SerializeField] private AnimationCurve sleepBasedHealthModifier = default;
        [SerializeField] private AnimationCurve funBasedHealthModifier = default;


        
        private Character _character = null;
        private Thinker _thinker = null;

        private void Awake() {
            _character = GetComponent<Character>();
            _thinker = GetComponent<Thinker>();
            _thinker.OnActionExecuted += ModifyMultiplierStats;
            
            if (randomizeStatsAtStart) {
                HungerConsumePerSecond = GetValueRandomize();
                ThirstConsumePerSecond = GetValueRandomize();
                FunConsumePerSecond = GetValueRandomize();
                SleepyConsumePerSecond = GetValueRandomize();
            }

        }

        private void Update() {
            var deltaTime = Time.deltaTime;

            var inverseValueHunger = Mathf.InverseLerp(0, _character.MaxValueStat, _character.Hunger);
            var inverseValueThirst = Mathf.InverseLerp(0, _character.MaxValueStat, _character.Thirst);
            var inverseValueSleep = Mathf.InverseLerp(0, _character.MaxValueStat, _character.Sleepy);
            var inverseValueFun = Mathf.InverseLerp(0, _character.MaxValueStat, _character.Fun);
            
            HealthGainedPerSecond =
                hungerBasedHealthModifier.Evaluate(inverseValueHunger) +
                thirstBasedHealthModifier.Evaluate(inverseValueThirst)+
                sleepBasedHealthModifier.Evaluate(inverseValueSleep) +
                funBasedHealthModifier.Evaluate(inverseValueFun);

            _character.Health = Mathf.Clamp(_character.Health + HealthGainedPerSecond * deltaTime, 0, _character.MaxValueStat);
            _character.Hunger = Mathf.Clamp(_character.Hunger - HungerConsumePerSecond * deltaTime * modifierHungerConsume, 0, _character.MaxValueStat);
            _character.Thirst = Mathf.Clamp(_character.Thirst - ThirstConsumePerSecond * deltaTime * modifierThirstConsume, 0, _character.MaxValueStat);
            _character.Fun = Mathf.Clamp(_character.Fun - FunConsumePerSecond * deltaTime * modifierFunConsume, 0, _character.MaxValueStat);
            _character.Sleepy = Mathf.Clamp(_character.Sleepy - SleepyConsumePerSecond * deltaTime * modifierSleepyConsume, 0, _character.MaxValueStat);
        }

        private void ModifyMultiplierStats(Action action) {
            var actionConsume = StatsConsumeDatabase.Instance.GetActionConsume(action);
            if (actionConsume == null) {
                return;
            }
            modifierThirstConsume = actionConsume.thirstConsume;
            modifierHungerConsume = actionConsume.hungerConsume;
            modifierFunConsume = actionConsume.funConsume;
            modifierSleepyConsume = actionConsume.sleepyConsume;

        }


        private float GetValueRandomize() => Random.Range(consumeMinMax.x, consumeMinMax.y);
    }

}
