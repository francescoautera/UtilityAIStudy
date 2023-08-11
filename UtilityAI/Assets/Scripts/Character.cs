using System;
using Sirenix.OdinInspector;
using UnityEngine;


namespace CharacterData {

    public enum Job {
        None,
        Supplier,
        Priest,
        Bartender
    }

    public class Character : MonoBehaviour {
        
        public Job characterJob;
        private Vector3 initPosition;
        [SerializeField] private Animator characterAnimator;
        
        [Title("Stats")]    
        [Range(0, 100)] public float MaxValueStat = 100;
        [Range(0, 100), ReadOnly] public float Health = 100;
        [Range(0, 100), ReadOnly] public float Hunger = 100;
        [Range(0, 100), ReadOnly] public float Thirst = 100;

        public Vector3 InitPosition => initPosition;

        private void Awake() {
            Health = MaxValueStat;
            Hunger = MaxValueStat;
            Thirst = MaxValueStat;
            
            if (characterJob is not Job.None) {
                initPosition = transform.position;
            }
        }


        public void SetIdle() {
            SetAnimatorState("idle");
        }


        public void SetAnimatorState(string animatorState) {
            characterAnimator.SetTrigger(animatorState);
        }

    }

}