using UnityEngine;

public class CactusFight : MonoBehaviour
{
    public GameObject firstCactus;
    public GameObject secondCactus;

    private Animator firstCactusAnimator;
    private Animator secondCactusAnimator;

    private AudioSource firstCactusAudioSource;
    private AudioSource secondCactusAudioSource;

    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        float distance = GetDistanceBetweenCacti();
        if (distance <= 0.5f)
            SetCactiState(true);
        else
            SetCactiState(false);

    }
    private void InitializeComponents()
    {
        firstCactusAnimator = firstCactus.GetComponent<Animator>();
        secondCactusAnimator = secondCactus.GetComponent<Animator>();

        firstCactusAudioSource = firstCactus.GetComponent<AudioSource>();
        secondCactusAudioSource = secondCactus.GetComponent<AudioSource>();
    }
    private float GetDistanceBetweenCacti() =>
        Vector3.Distance(firstCactus.transform.position, secondCactus.transform.position);

    private void SetCactiState(bool isAttacking)
    {
        firstCactusAnimator.SetBool("isAttacking", isAttacking);
        secondCactusAnimator.SetBool("isAttacking", isAttacking);

        SetAudioState(firstCactusAudioSource, isAttacking);
        SetAudioState(secondCactusAudioSource, isAttacking);
    }
    private void SetAudioState(AudioSource cactusAudioSource, bool isAttacking)
    {
        if (isAttacking && !cactusAudioSource.isPlaying)
            cactusAudioSource.Play();
        else if (!isAttacking && cactusAudioSource.isPlaying)
            cactusAudioSource.Stop();

    }
}



