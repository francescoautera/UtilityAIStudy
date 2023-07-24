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

        [SerializeField, ReadOnly] private List<Action> _actionsRunning = new();

        private float _lastThinkTime = 0;

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
            _actionsRunning.Add(actionToExecute);
            actionToExecute.Execute(this, deltaTime);

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
    }
}
