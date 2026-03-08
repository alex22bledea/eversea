
using Crest.Examples;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    [SerializeField] private Vector3VariableSO playerPosition;
    [SerializeField] private VoidEventChannel NightStartedEventChannel;
    [SerializeField] private VoidEventChannel NightEndedEventChannel;

    [Space(12)]
    [SerializeField] private RuntimeListSO<FishingSpot> fishingSpotList;

    [Space(12)]
    [SerializeField] private ItemDropList ItemDropList;

    [SerializeField, MinMaxSlider(1, 10)] private Vector2Int fishAvailableRange;

    [SerializeField] private FishFlock fishFlock;

    [Space(12)]
    [SerializeField] private float fishingSpotDescendDuration;
    [Space(12)]

    [SerializeField] private Transform bouyTransform;

    [Space(12)]
    [SerializeField] private Transform topFishPosition;
    [SerializeField] private Transform bottomFishPosition;
    [SerializeField] private Transform bottomBouyPosition;

    [Space(12)]
    [SerializeField] private float bottomBouyH;
    [SerializeField] private float topBouyH;

    [Space(12)]
    [SerializeField] private FishingPlayerDock playerDock;

    [Space(12)]
    [SerializeField] private float dailySpawnChance;
    [SerializeField] private float maxPlayerDistForAnim;
    [SerializeField] private float maxPlayerDistForFishFlock;

    private Stack<ItemData> itemDropStack = new Stack<ItemData>();

    public int FishAvailable { get; private set; }
    private bool hadProperDestroy = false;

    public bool IsActive { get; private set; }

    private bool isDespawning = false;
    private bool isSpawning = false;

    private void Awake()
    {
        fishingSpotList.Add(this);
        IsActive = false;
    }

    private void Start()
    {
        if (!IsActive) /// Because docks start active!
            playerDock.DisableDock();

        TryToSpawn();
    }

    private void Update()
    {
        if (Helpers.SqDistance(fishFlock.transform.position, playerPosition.Value) <= maxPlayerDistForFishFlock * maxPlayerDistForFishFlock)
        {
            if (!fishFlock.isActiveAndEnabled)
            {
                fishFlock.gameObject.SetActive(true);
                fishFlock.RestartAnim();
            }
        }
        else if (fishFlock.isActiveAndEnabled)
            fishFlock.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        NightStartedEventChannel.OnEventRaised += NightStartedEventChannel_OnEventRaised;
        NightEndedEventChannel.OnEventRaised += TryToSpawn;
    }

    private void OnDisable()
    {
        NightStartedEventChannel.OnEventRaised -= NightStartedEventChannel_OnEventRaised;
        NightEndedEventChannel.OnEventRaised -= TryToSpawn;
    }

    private void NightStartedEventChannel_OnEventRaised()
    {
        if (IsActive)
            StartCoroutine(DespawnFishingSpot());
    }

    private void TryToSpawn()
    {
        float r = UnityEngine.Random.value;

        bool shouldBeActive = r <= dailySpawnChance;

        if(IsActive && !shouldBeActive)
        {
            StartCoroutine(DespawnFishingSpot());
        }
        else if(!IsActive && shouldBeActive)
        {
            StartCoroutine(SpawnFishingSpot());
        }
    }

    public ItemData GetNextItem()
    {
        if (itemDropStack.Count > 0)
            return itemDropStack.Peek();
        else
        {
            Debug.LogError("Fishing spot item drop stack is empty but is being accessed", this);
            return null;
        }
    }

    private void GenerateItemDrops()
    {
        itemDropStack.Clear();

        FishAvailable = FishAvailable = UnityEngine.Random.Range(fishAvailableRange.x, fishAvailableRange.y + 1);

        for (int i = 1; i <= FishAvailable; i++)
            itemDropStack.Push(ItemDropList.GetRandomItem());
    }

    private IEnumerator SpawnFishingSpot()
    {
        if(isSpawning)
            yield break;

        isSpawning = true;

        Vector3 startFishPos = bottomFishPosition.position;
        Vector3 endFishPos = topFishPosition.position;

        bouyTransform.position = bottomBouyPosition.position;
        bouyTransform.rotation = Quaternion.identity;

        BoatAlignNormal bouyBoatAlignNormal = bouyTransform.GetComponent<BoatAlignNormal>();

        // bool playAnimation = Helpers.SqDistance(playerPosition.Value, transform.position) <= maxPlayerDistForAnim * maxPlayerDistForAnim;

        float time = 0;
        while (time < 1f)
        {
            time += 1 / fishingSpotDescendDuration * Time.deltaTime;
            fishFlock.transform.position = Vector3.Lerp(startFishPos, endFishPos, time);
            bouyBoatAlignNormal._bottomH = Mathf.Lerp(bottomBouyH, topBouyH, time);

            yield return null;
        }

        fishFlock.transform.position = endFishPos;

        yield return null; /// Skip 1 frame

        IsActive = true;
        playerDock.EnableDock();

        GenerateItemDrops();

        isSpawning = false;
    }

    private IEnumerator DespawnFishingSpot()
    {
        if (isDespawning)
            yield break;

        isDespawning = true;

        IsActive = false;
        playerDock.DisableDock();

        if (FishingMinigamesManager.Instance.SelectedFishingSpot == this && FishingMinigamesManager.Instance.IsPlayingMinigame)
        {
            FishAvailable = 1;
            FishingMinigamesManager.Instance.CancellMinigame();
        }
        else
            FishAvailable = 0;

        itemDropStack.Clear();

        Vector3 startFishPos = topFishPosition.position;
        Vector3 endFishPos = bottomFishPosition.position;

        BoatAlignNormal bouyBoatAlignNormal = bouyTransform.GetComponent<BoatAlignNormal>();

        // bool playAnimation = Helpers.SqDistance(playerPosition.Value, transform.position) <= maxPlayerDistForAnim * maxPlayerDistForAnim;

        float time = 0;
        while (time < 1f)
        {
            time += 1 / fishingSpotDescendDuration * Time.deltaTime;
            fishFlock.transform.position = Vector3.Lerp(startFishPos, endFishPos, time);
            bouyBoatAlignNormal._bottomH = Mathf.Lerp(topBouyH, bottomBouyH, time);

            yield return null;
        }

        fishFlock.transform.position = endFishPos;

        isDespawning = false;
    }

    public void DecreaseFishAvailable()
    {
        if (itemDropStack.Count > 0)
            itemDropStack.Pop();

        FishAvailable--;

        if (FishAvailable <= 0)
            StartCoroutine(DespawnFishingSpot());
    }

    public void CustomDestroy()
    {
        if (hadProperDestroy)
            return;

        hadProperDestroy = true;

        fishingSpotList.Remove(this);

        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        CustomDestroy();
    }
}
