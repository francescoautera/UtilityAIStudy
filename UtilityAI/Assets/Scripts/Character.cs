using System;
using Sirenix.OdinInspector;
using UnityEngine;


namespace CharacterData {

    public class Character : MonoBehaviour {
        [Range(0, 100)] public float MaxValueStat = 100;
        [Range(0, 100), ReadOnly] public float Health = 100;
        [Range(0, 100), ReadOnly] public float Hunger = 100;
        [Range(0, 100), ReadOnly] public float Thirst = 100;

        private void Awake() {
            Health = MaxValueStat;
            Hunger = MaxValueStat;
            Thirst = MaxValueStat;
        }

    }

}