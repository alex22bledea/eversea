using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FishingMinigames/CircleSetup", fileName = "new CircleMinigameSetup")]
[Serializable]
public class CircleMinigameSetupSO : ScriptableObject
{
    [Header("Line")]
    public float lineAngularSpeed;

    [Space(12)]
    public float succesfullCatchBonus;
    public float failedCatchTimeWait;

    [Header("Target")]
    public float targetHalfAngularWidth;
    public float targetRectHeight;
    public float successMinSpawnDist;
    public float failiureMinSpawnDist;
}
