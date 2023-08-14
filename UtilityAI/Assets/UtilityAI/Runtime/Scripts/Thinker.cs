using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace UtilityAI
{
    public enum ThinkTime
    {
        Update,
        FixedUpdate,
        Manual
    }

    public class Thinker : SerializedMonoBehaviour
    {
        public Brain BrainData = null;
        public ThinkTime ThinkTime = ThinkTime.Update;
        [NonSerialized] public Action<Action, Thinker> OnObjectSatisfyRequested;
        [NonSerialized] public  Action<Action> OnActionExecuted;
        public static Action<Thinker> OnThinkerBorn;
        [NonSerialized] public Action<Thinker> OnActionEndedFirst;

        [ReadOnly,SerializeReference] private List<Action> _actionsRunning = new();
        
        private float _lastThinkTime = 0;

        public bool isObjectSatisfyAction;

        private void Start() {
            OnThinkerBorn?.Invoke(this);
        }

        private void Update()
        {
            if(ThinkTime != ThinkTime.Update)
            {
                return;
            }
            TryThink();
        }

        private void FixedUpdate()
        {
            if (ThinkTime != ThinkTime.FixedUpdate)
            {
                return;
            }
            TryThink();
        }

        public bool TryThink()
        {
            if (!CanThink())
            {
                return false;
            }
            Think();
            return true;
        }

        private void Think()
        {
            var deltaTime = Time.time - _lastThinkTime;
            var actionToExecute = BrainData.GetBestAction(this);
            if (!IsActionOnList(actionToExecute)) {
                actionToExecute.OnEndAction += RemoveAction;
                RemoveActions();
                _actionsRunning.Add(actionToExecute);
                OnActionExecuted?.Invoke(actionToExecute);
                if (isObjectSatisfyAction) {
                    OnObjectSatisfyRequested?.Invoke(actionToExecute,this);
                    actionToExecute.Execute(this,deltaTime,isObjectSatisfyAction);
                }
                else {
                    actionToExecute.Execute(this, deltaTime,isObjectSatisfyAction);
                }
            }
            _lastThinkTime = Time.time;
        }

        private bool CanThink()
        {
            bool isOccupied = false;
            for(int i = _actionsRunning.Count - 1; i >= 0; i--)
            {
                if (_actionsRunning[i].NeedsToBeWaited())
                {
                    isOccupied = true;
                }
                if (_actionsRunning[i].IsCompleted())
                {
                    _actionsRunning.RemoveAt(i);
                }
            }
            return !isOccupied;
        }

        private void RemoveActions() {
            for(int i = 0 ; i < _actionsRunning.Count; i++)
            {
                if (_actionsRunning[i].MustActionBeStopped()) {
                    OnActionEndedFirst?.Invoke(this);
                    _actionsRunning[i].OnEndAction -= RemoveAction;
                    _actionsRunning.RemoveAt(i);
                }
            }
        }

        private void RemoveAction(Action action) {
            action.OnEndAction -= RemoveAction;
            Debug.Log(action.Title + " " + gameObject.name);
            foreach (var actions in _actionsRunning) {
                if (actions.GetType() == action.GetType()) {
                    _actionsRunning.Remove(action);
                    Think();
                    return;
                }
            }
        }
        

        private bool IsActionOnList(Action action) {
            foreach (var actions in _actionsRunning) {
                if (actions.GetType() == action.GetType()) {
                    return true;
                }
            }
            return false;
        }
    }
}
