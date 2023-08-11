
using Objects;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Objects {


    public class ObjectLogic : MonoBehaviour {
        
        public ObjectRestoreParameters ObjectRestoreParameters;
        private ObjectLogicContainer container;
        public Transform[] characterTransforms;
        [SerializeField, ReadOnly] private int occupiedPost = 0;

        
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


        public void IncreaseOccupiedPost() => occupiedPost++;

        public void FreeOccupiedPost() => occupiedPost--;

        public Transform GetDestination() => characterTransforms[occupiedPost - 1];

        [Button]
        private void DebugAddTransforms() {
            characterTransforms = transform.GetComponentsInChildren<Transform>();
        }
    }

}
