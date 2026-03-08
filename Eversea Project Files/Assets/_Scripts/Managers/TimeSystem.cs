using Unity.VisualScripting;
using UnityEngine;

public class TimeSystem : Singleton<TimeSystem>
{
    [Header("Raises events")]
    [SerializeField] private VoidEventChannel NightStartedEventChannel;
    [SerializeField] private VoidEventChannel NightEndedEventChannel;

    [Space(12), Header("Time Values (in minutes)")]
    [SerializeField] private FloatVariableSO startTime;
    [field: SerializeField] public float NightStartTime { get; private set; }
    [field: SerializeField] public float NightEndTime { get; private set; }

    [Space(12), Header("Sun Movement and Lighting")]
    [SerializeField] private float sunriseTime;
    [SerializeField] private float sunsetTime;
    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;
    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private AnimationCurve lightChangeCurve;
    [SerializeField] private float maxSunLightIntensity;
    [SerializeField] private float maxMoonLightIntensity;

    [Space(12)]
    [SerializeField] private FloatVariableSO baseTimeScale;
    [SerializeField] private FloatVariableSO fishingTimeMultiplier;

    private float currentTimeScale;

    private const float DAY_DURATION = 1440;
    public int daysSurvived { get; private set; }
    private float crTime;

    protected override void Awake()
    {
        base.Awake();

        currentTimeScale = baseTimeScale.Value;

        crTime = startTime.Value % DAY_DURATION;
    }

    private void Start()
    {
        FishingMinigamesManager.Instance.OnFishingGameStarted += FishingMinigamesManager_OnFishingGameStarted;
        FishingMinigamesManager.Instance.OnFishingMinigameEnded += FishingMinigamesManager_OnFishingMinigameEnded;
    }

    private void Update()
    {
        float prevTime = crTime;
        crTime += Time.deltaTime * currentTimeScale;

        if (NightStartTime.IsInBetween(prevTime, crTime))
            NightStartedEventChannel.RaiseEvent();

        if (NightEndTime.IsInBetween(prevTime, crTime))
            NightEndedEventChannel.RaiseEvent();

        while (crTime > DAY_DURATION)
        {
            crTime -= DAY_DURATION;
            daysSurvived++;
        }

        RotateSun();
        UpdateLightSettings();
    }

    private void FishingMinigamesManager_OnFishingMinigameEnded()
    {
        currentTimeScale = baseTimeScale.Value;
    }

    private void FishingMinigamesManager_OnFishingGameStarted()
    {
        currentTimeScale = baseTimeScale.Value * fishingTimeMultiplier.Value;
    }

    public float GetNightDuration() => DAY_DURATION + NightEndTime - NightStartTime;

    public float GetTotalTimePassed() => daysSurvived * DAY_DURATION + crTime;

    public float GetCurrentTime() => crTime;

    /// Sun Movement:

    float CalculateTimeDifference(float fromTime, float toTime)
    {
        float difference = toTime - fromTime;

        if (difference < 0)
            difference += DAY_DURATION;

        return difference;
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (crTime >= sunriseTime && crTime <= sunsetTime)
        {
            float sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            float timeSinceSunrise = CalculateTimeDifference(sunriseTime, crTime);

            float percentage = timeSinceSunrise / sunriseToSunsetDuration;

            sunLightRotation = Mathf.Lerp(0, 180, percentage);
        }
        else
        {
            float sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            float timeSinceSunset = CalculateTimeDifference(sunsetTime, crTime);

            float percentage = timeSinceSunset / sunsetToSunriseDuration;

            sunLightRotation = Mathf.Lerp(180, 360, percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }
}
