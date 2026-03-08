using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "new EnemyStats", menuName = "ScriptableObjects/Enemies/Stats")]
public class EnemyStatsSO : ScriptableObject
{
    [MinMaxSlider(0f, 100000f)] public Vector2 SqRange;
    [MinMaxSlider(0f, 30f)] public Vector2 AttackCooldown;
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float EnginePower { get; private set; }
    [field: SerializeField] public float TurnPower { get; private set; }
    
}
