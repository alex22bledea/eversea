using System;
using System.Collections.Generic;
using UnityEngine;

public class CircleMinigame : BaseFishingMinigame
{
    [Header("Components")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform line;
    [SerializeField] private RectTransform target;

    [Space(12)]
    [SerializeField] private AnimationCurve failureSpeedCurve;

    private CircleMinigameSetupSO setup;
    private float lastFailureTime;

    [Serializable]
    private class DictionaryOfFishRarityAndCircleSetup : SerializableDictionary<FishRaritySO, CircleMinigameSetupSO> { };

    [SerializeField] private DictionaryOfFishRarityAndCircleSetup fishRarityToSetup;

    [SerializeField] private FishRaritySO baseFishRarity;

    [Header("Test")]
    [SerializeField] private RectTransform targetLine1;
    [SerializeField] private RectTransform targetLine2;

    private void Update()
    {
        if ( !isStarted)
            return;

        float angle = setup.lineAngularSpeed * Time.deltaTime;
        if (lastFailureTime + setup.failedCatchTimeWait > Time.time)
        {
            float timeNormalized = (Time.time - lastFailureTime) / setup.failedCatchTimeWait;
            angle *= failureSpeedCurve.Evaluate(timeNormalized);
        }
        
        line.localEulerAngles += new Vector3(0f, 0f, angle);

        while (line.localEulerAngles.z > 360f)
            line.localEulerAngles -= new Vector3(0f, 0f, 360f);
    }

    private void OnEnable()
    {
        input.MinigameInteractEvent += OnInteract;
    }

    private void OnDisable()
    {
        input.MinigameInteractEvent -= OnInteract;
    }

    private void OnInteract()
    {
        if ( !isStarted)
            return;

        if (lastFailureTime + setup.failedCatchTimeWait > Time.time)
            return;

        float dif = target.localEulerAngles.z - line.localEulerAngles.z;
        float angleDist = Mathf.Min(Mathf.Abs(dif), Mathf.Abs(dif + 360f), Mathf.Abs(dif - 360f));

        if (angleDist < setup.targetHalfAngularWidth)
        {
            compMeter.Increase(setup.succesfullCatchBonus);
            SpawnTarget(true);
        }
        else
        {
            lastFailureTime = Time.time;
            SpawnTarget(false);
        }
    }

    private void PrepareTarget()
    {
        target.sizeDelta = new Vector2(target.sizeDelta.x, setup.targetRectHeight);

        float newAngle = 180f;

        target.localEulerAngles = new Vector3(0f, 180f, newAngle);

        targetLine1.localEulerAngles = target.localEulerAngles + new Vector3(0f, 0f, -setup.targetHalfAngularWidth);
        targetLine2.localEulerAngles = target.localEulerAngles + new Vector3(0f, 0f, setup.targetHalfAngularWidth);
    }

    private void SpawnTarget(bool lastTargetHit)
    {
        target.sizeDelta = new Vector2(target.sizeDelta.x, setup.targetRectHeight);

        float newAngle = line.localEulerAngles.z + UnityEngine.Random.Range(lastTargetHit ? setup.successMinSpawnDist : setup.failiureMinSpawnDist, 300f - setup.targetHalfAngularWidth);

        if (newAngle > 360f)
            newAngle -= 360f;

        target.localEulerAngles = new Vector3(0f, 180f, newAngle);

        targetLine1.localEulerAngles = target.localEulerAngles + new Vector3(0f, 0f, -setup.targetHalfAngularWidth);
        targetLine2.localEulerAngles = target.localEulerAngles + new Vector3(0f, 0f, setup.targetHalfAngularWidth);
    }

    public override void PrepareMinigame()
    {
        setup = fishRarityToSetup[baseFishRarity];

        base.PrepareMinigame();

        target.sizeDelta = new Vector2(target.sizeDelta.x, setup.targetRectHeight);

        line.localEulerAngles = new Vector3(0f, 180f, 90f);

        PrepareTarget();
    }

    public override void StartMinigame()
    {
        base.StartMinigame();

        SpawnTarget(true);
        lastFailureTime = 0f;
    }
}
