using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "ScriptableObjects/FishingMinigames/HoldBarSetup", fileName = "new HoldBarSetup")]
public class HoldBarSetupSO : ScriptableObject
{
    [Header("Background")]
    public float height;

    [Header("Box")]
    public float boxHalfHeight;
    public float boxMaxSpeed;
    public float boxAccel;
    public float boxRange;

    [Space(12)]
    public float inRangeMult;

    [Space(12)]
    [Header("Target")]
    public float targetMaxSpeed;
    public float targetAccel;
    [MinMaxSlider(0f, 1f)]
    public Vector2 waitTime;
    [MinMaxSlider(0f, 300f)]
    public Vector2 travelDist;
}
