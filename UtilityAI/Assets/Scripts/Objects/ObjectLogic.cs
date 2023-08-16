
using System.Collections.Generic;
using Objects;
using Sirenix.OdinInspector;
using UnityEngine;
using UtilityAI;

namespace Objects {


    public class ObjectLogic : MonoBehaviour {

        public ObjectRestoreParameters ObjectRestoreParameters;
        private ObjectLogicContainer container;
        public Transform[] characterTransforms;
        [SerializeField, ReadOnly] private int occupiedPost = 0;
        private Action currAction;
        private List<Thinker> Thinkers = new List<Thinker>();


        private void Awake() {
            container = FindObjectOfType<ObjectLogicContainer>();
            container.objects.Add(this);
        }

        private void OnDestroy() {
            container.objects.Remove(this);
        }

        public bool IsOccupied() {
            return occupiedPost == characterTransforms.Length;
        }


        public void IncreaseOccupiedPost(Action action,Thinker thinker) {
            if (currAction == null) {
                currAction = action;
                currAction.OnEndAction += TryFreeOccupiedPost;
            }
            Thinkers.Add(thinker);
            occupiedPost++;
        }

        public void FreeOccupiedPost() => occupiedPost--;

        public Transform GetDestination() => characterTransforms[occupiedPost - 1];

        [Button]
        private void DebugAddTransforms() {
            characterTransforms = transform.GetComponentsInChildren<Transform>();
        }

        private void TryFreeOccupiedPost(Action action, Thinker thinker) {
            if (Thinkers.Contains(thinker)) {
                FreeOccupiedPost();
                Thinkers.Remove(thinker);
            }
            
            if (occupiedPost == 0) {
                currAction.OnEndAction -= TryFreeOccupiedPost;
                currAction = null;
                Thinkers.Clear();
                
            }
        }

    }

}
