using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using System;

namespace UtilityAI
{
    [CreateAssetMenu(fileName = "new_utility_ai_brain", menuName = "Utility AI/Brain")]
    public class Brain : SerializedScriptableObject
    {
        [ListDrawerSettings(ListElementLabelName = "_title")]
        public Action[] Actions = Array.Empty<Action>();

        public Action GetBestAction(Thinker thinker)
        {
            return Actions.OrderByDescending(a => a.GetScore(thinker)).ElementAt(0);
        }
    }
}
