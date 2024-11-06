using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{

    private GameLogic gameLogic;
    private Rigidbody controlledRigidBody;
    public GameObject hitVFXPrefab;

    public int TimesHit { get; private set; }
    private int cooldownMillisLeft;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdateCooldown();
    }

    public void Reset()
    {
        ResetRigidBody();
        ResetTransform();
        TimesHit = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Club"))      
            HandleHitByClub(collision);   
    }

    private void Initialize()
    {
        controlledRigidBody = GetComponent<Rigidbody>();
        gameLogic = FindObjectOfType<GameLogic>();
        TimesHit = 0;
        cooldownMillisLeft = 0;
    }

    private void UpdateCooldown()
    {
        if (cooldownMillisLeft > 0)
        {
            cooldownMillisLeft -= (int)(Time.deltaTime * 1000);
        }
        cooldownMillisLeft = Mathf.Max(0, cooldownMillisLeft); 
    }

    private void ResetRigidBody()
    {
        controlledRigidBody.angularVelocity = Vector3.zero;
        controlledRigidBody.velocity = Vector3.zero;
    }

    private void ResetTransform()
    {
        transform.rotation = Quaternion.identity;
        transform.position = new(0, 0.25f, 0);
    }

    private void HandleHitByClub(Collision collision)
    {
        if (cooldownMillisLeft > 0) return;

        cooldownMillisLeft = 1000;
        TimesHit++;
        gameLogic.UpdateOnBallHit();
        TriggerHitVFX(collision.contacts[0].point);
    }

    private void TriggerHitVFX(Vector3 coords)
    {
        GameObject hitVFXInstance = Instantiate(hitVFXPrefab, coords, Quaternion.identity);
        StartCoroutine(HitVFXAnimation(hitVFXInstance));
    }

    private IEnumerator HitVFXAnimation(GameObject hitVFXInstance)
    {
        float timePassed = 0;
        while (timePassed < 250)
        {
            float progress = timePassed / 250;
            hitVFXInstance.transform.localScale = Vector3.Lerp(new(0.01f, 0.01f, 0.01f), new(0.3f, 0.3f, 0.3f), progress);
            timePassed += Time.deltaTime * 1000;
            yield return null;
        }
        Destroy(hitVFXInstance);
    }
}

