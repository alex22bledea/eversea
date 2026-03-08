using UnityEngine;
using NaughtyAttributes;

public class FishingSpotSpawner : MonoBehaviour
{
    [Header("Listening to Event Channels")]
    [SerializeField] private VoidEventChannel NightStartedEventChannel;
    [SerializeField] private VoidEventChannel NightEndedEventChannel;

    [Header("Prefab")]
    [SerializeField] private GameObject fishingSpotPrefab;

    [SerializeField] private RuntimeListSO<FishingSpot> fishingSpotList;

    [SerializeField] private float playerViewRange;
    [MinMaxSlider(0f, 10f)]
    [SerializeField] private Vector2 fishingSpotsCount; 
    [SerializeField] private Vector3Reference playerPosRef;

    private Vector3 lastSpawnPos;

    private bool canSpawn = true;

    private void OnEnable()
    {
        NightStartedEventChannel.OnEventRaised += DisableSpawning;
        NightEndedEventChannel.OnEventRaised += EnableSpawning;
    }

    private void OnDisable()
    {
        NightStartedEventChannel.OnEventRaised -= DisableSpawning;
        NightEndedEventChannel.OnEventRaised -= EnableSpawning;
    }

    private void DisableSpawning() => canSpawn = false;
    private void EnableSpawning() => canSpawn = true;

    private void Start()
    {
        SpawnNewFishingSpots(0.5f, 1.6f);
        SpawnNewFishingSpots();
    }

    private void Update()
    {
        if (canSpawn && Helpers.SqDistance(playerPosRef.Value, lastSpawnPos) > playerViewRange * playerViewRange)
            SpawnNewFishingSpots();
    }

    /// <summary> Spawns new Spots between min and max multiplier of range </summary>
    private void SpawnNewFishingSpots(float minMult = 1f, float maxMult = 1.8f)
    {
        // Removes old, unused fishing spots

        for (int i = fishingSpotList.Items.Count - 1; i >= 0; i--)
            if (Helpers.SqDistance(fishingSpotList.Items[i].transform.position, playerPosRef.Value) > playerViewRange * playerViewRange)
                fishingSpotList.Items[i].CustomDestroy();

        // Calculate how many more fishing spots to add

        int toAdd = UnityEngine.Random.Range((int)fishingSpotsCount.x, (int)fishingSpotsCount.y + 1) - fishingSpotList.Items.Count;

        // Spawns new Fishing Spots outside of the deadZone

        for (int i = 0; i < toAdd; i++)
        {
            float dist = playerViewRange * UnityEngine.Random.Range(minMult, maxMult);
            float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);

            Instantiate(fishingSpotPrefab, new Vector3(playerPosRef.Value.x + dist * Mathf.Sin(angle), 0f, playerPosRef.Value.z + dist * Mathf.Cos(angle)), Quaternion.identity);
        }

        lastSpawnPos = playerPosRef.Value;
    }
}
