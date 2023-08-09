using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI;

namespace StatsConsumeData {

    [CreateAssetMenu(menuName = "Stats/StatsDatabase", fileName = "StatsConsumeDatabase", order = 0)]
    public class StatsConsumeDatabase : SingletonScriptableObject<StatsConsumeDatabase> {

        public List<ActionStatsConsume> ActionStatsConsumes = new List<ActionStatsConsume>();

        public ActionStatsConsume GetActionConsume(Action action) {
            foreach (var actionStatsConsume in ActionStatsConsumes) {
                if (actionStatsConsume.IsTheSameAction(action)) {
                    return actionStatsConsume;
                }
            }
            return null;
        }
    }

}
