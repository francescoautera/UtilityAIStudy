
using System;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UtilityAI;

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
        private ActionInfo actionInfo;
        [Title("Camera")]
        [SerializeField] private CinemachineVirtualCameraBase cameraCharacter;
        [SerializeField] private Vector3 positionBuildingCamera;
        [SerializeField,ReadOnly] Vector3 initPositionCamera;

        private AudioListener audioCharacter;
        
        [Title("Stats")]    
        [Range(0, 100)] public float MaxValueStat = 100;
        [Range(0, 100), ReadOnly] public float Health = 100;
        [Range(0, 100), ReadOnly] public float Hunger = 100;
        [Range(0, 100), ReadOnly] public float Thirst = 100;
        [Range(0, 100), ReadOnly] public float Fun = 100;
        [Range(0, 100), ReadOnly] public float Sleepy = 100;

        public Vector3 InitPosition => initPosition;

        private void Awake() {
            Health = MaxValueStat;
            Hunger = MaxValueStat;
            Thirst = MaxValueStat;
            Fun = MaxValueStat;
            Sleepy = MaxValueStat;
            
            initPositionCamera = cameraCharacter.transform.localPosition;
            
            if (characterJob is not Job.None) {
                initPosition = transform.position;
            }
            actionInfo = GetComponentInChildren<ActionInfo>();
            var thinker = GetComponent<Thinker>();
            thinker.OnActionEndedFirst += CloseActionInfo;
            audioCharacter = cameraCharacter.GetComponent<AudioListener>();
            audioCharacter.enabled = false;
        }

        public void ResetCamera() {
            cameraCharacter.Priority = 10;
            audioCharacter.enabled = false;
        }

        public void SetIdle() {
            SetAnimatorState("idle",true);
        }


        public void SetAnimatorState(string animatorState,bool value) {
            characterAnimator.SetBool(animatorState,value);
        }
        

        public void SetCharacterInfo() {
            cameraCharacter.Priority = 11;
            audioCharacter.enabled = true;
        }

        private void CloseActionInfo(Thinker thinker) {
            actionInfo.FinishEvent();
        }

        public void ActiveActionInfo(float actionTimer) => actionInfo.Init(actionTimer);

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.layer == LayerMask.NameToLayer("Building")) {
                cameraCharacter.transform.localPosition = positionBuildingCamera;
            }
        }

        private void OnTriggerExit(Collider other) {
            if(other.gameObject.layer == LayerMask.NameToLayer("Building")) {
                cameraCharacter.transform.localPosition = initPositionCamera;
            }
        }
    }

}