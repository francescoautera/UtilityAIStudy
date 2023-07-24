using System;
using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;


namespace Objects {


    public class ObjectLogic : MonoBehaviour {
        
        public ObjectRestoreParameters ObjectRestoreParameters;
        private ObjectLogicContainer container;
        
        private void Awake() {
            container = FindObjectOfType<ObjectLogicContainer>();
            container.objects.Add(this);
        }

        private void OnDestroy() {
            container.objects.Remove(this);
        }

    }

}
