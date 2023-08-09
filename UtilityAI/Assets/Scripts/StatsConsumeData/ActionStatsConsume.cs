using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI;


[CreateAssetMenu(menuName = "Stats/StatsConsume", fileName = "_Consume", order = 0)]
public class ActionStatsConsume : ScriptableObject {
    
    [SerializeReference] public Action actionRef;
    public float hungerConsume;
    public float thirstConsume;


    public bool IsTheSameAction(Action action) {
        return action.GetType() == actionRef.GetType();
    }
}
