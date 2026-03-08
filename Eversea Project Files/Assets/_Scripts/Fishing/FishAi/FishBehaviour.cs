using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class FishBehaviour : MonoBehaviour
{
    [SerializeField] private float rayCastDist;
    [SerializeField] private LayerMask fishLayer;
    [SerializeField, MinValue(0f), MaxValue(100f)] private float fishEvadeChance = 30;
    [SerializeField] private float evadeCooldown = 0.2f;

    [SerializeField, MinMaxSlider(0f, 10f)]
    private Vector2 speedRange;
    private float speed;

    [Space(12)]
    [SerializeField] private float rotationDuration = 1f;

    private FishFlock flock;

    private Vector3 startingPos;
    private Vector3 targetPos; // Obs: Is in local space
    private float moveTime;
    private float mult;

    private float evadeTimer;

    private bool isRotating = false;

    private void Update()
    {
        evadeTimer -= Time.deltaTime;

        if (isRotating)
            return;

        // Move Towards target 

        moveTime += mult * Time.deltaTime;

        transform.localPosition = Vector3.Lerp(startingPos, targetPos, moveTime);

        if (moveTime >= 1f)
            GetNewTargetPos();

        // Check if colliding with a fish

        if (evadeTimer < 0f && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayCastDist, fishLayer))
        {
            evadeTimer = evadeCooldown;

            if (UnityEngine.Random.Range(0f, 100f) < fishEvadeChance)
                GetNewTargetPos();
        }
    }

    public void Init(FishFlock flock, Vector3 spawnPos)
    {
        this.flock = flock;
        transform.localPosition = spawnPos;

        GetNewTargetPos();
    }

    public void GetNewTargetPos()
    {
        speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);

        startingPos = transform.localPosition;

        targetPos = flock.GetRandomPointInsideSphere();

        float dist = Vector3.Distance(startingPos, targetPos);
        float duration = dist / speed;
        mult = 1 / duration;

        moveTime = 0f;

        StartCoroutine(LookAtTargetPos());
    }

    private IEnumerator LookAtTargetPos()
    {
        isRotating = true;

        float time = 0;

        Quaternion startingRotation = transform.localRotation;

        Quaternion newRotation = Quaternion.LookRotation(targetPos - transform.localPosition);

        while (time < 1f)
        {
            time += 1 / rotationDuration * Time.deltaTime;

            transform.localRotation = Quaternion.Slerp(startingRotation, newRotation, time);

            yield return null;
        }

        isRotating = false;
    }
}
