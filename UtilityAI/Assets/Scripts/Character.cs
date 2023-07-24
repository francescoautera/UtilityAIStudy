using Sirenix.OdinInspector;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Range(0, 100), ReadOnly]
    public float Health = 100;
    [Range(0, 100), ReadOnly]
    public float Hunger = 100;
    [Range(0, 100), ReadOnly]
    public float Thirst = 100;
}
