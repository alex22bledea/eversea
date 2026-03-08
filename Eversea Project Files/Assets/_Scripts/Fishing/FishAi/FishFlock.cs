
using UnityEngine;
using NaughtyAttributes;

public class FishFlock : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float height;

    [SerializeField] private FishBehaviour[] fishPrefabArray;

    [SerializeField, MinMaxSlider(0f, 50f)]
    private Vector2 fishCountRange;

    [SerializeField, MinMaxSlider(0.5f, 1.5f)]
    private Vector2 fishScaleRange;

    [SerializeField] private bool drawGizmos = false;

    private int fishCount;
    private FishBehaviour[] fishBehaviourArray;

    private void Start()
    {
        fishCount = (int)UnityEngine.Random.Range(fishCountRange.x, fishCountRange.y);
        fishBehaviourArray = new FishBehaviour[fishCount];

        for (int i = 0; i < fishCount; i++)
            fishBehaviourArray[i] = SpawnNewFish();
    }

    public void RestartAnim()
    {
        foreach (FishBehaviour fish in fishBehaviourArray)
            fish.GetNewTargetPos();
    }

    private FishBehaviour SpawnNewFish()
    {
        FishBehaviour fish = Instantiate(fishPrefabArray.GetRandomElement(), transform);

        fish.Init(this, GetRandomPointInsideSphere());

        fish.transform.localScale *= UnityEngine.Random.Range(fishScaleRange.x, fishScaleRange.y);

        return fish;
    }

    public Vector3 GetRandomPointInsideSphere()
    {
        Vector2 posXZ = UnityEngine.Random.insideUnitCircle * radius;

        float posY = UnityEngine.Random.Range(-height / 2, height / 2);

        return new Vector3(posXZ.x, posY, posXZ.y);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(transform.position, new Vector3(2 * radius, height, 2 * radius));
        }
    }
}
