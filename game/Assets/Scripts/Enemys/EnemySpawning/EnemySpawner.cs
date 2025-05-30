using UnityEngine;
using System.Collections;
// <summary>
// EnemySpawner is responsible for spawning enemies in waves with increasing difficulty.

// !!! need make more wawe like you can use player score to start wave
public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject SecondEnemyPrefab;

    [Header("Wave settings")]
    [SerializeField] private int waveEnemyCount = 1;
    [SerializeField] private float EnemyMinSpawnDelay = 0.1f;
    [SerializeField] private float EnemyMaxSpawnDelay = 0.3f;
    [SerializeField] private int waveEnemyCounStepAdd = 1;
    [SerializeField] private float WaveStartDelay = 5f;
    [SerializeField] private float SpawnRadius = 30f;
    [Header("Wave Difficulty Settings")]
    [SerializeField] private int BabyWaveCount = 20;
    [SerializeField] private float WaveStartDelayStepAdd = 6f; // Delay between waves increases by this amount each time
    [SerializeField] private float WaveStartDelayStepSub = 1f;

    [Header("References")]
    [SerializeField] private PlayerScore PlayerScore;

    private int currentWave = 0;
    private int CurrentPlayerScore = 0; // reward score for spiky is 5 and Bird is 10

    private bool WaveCanStart = true;
    private bool shouldIncreaseDifficulty = true;
    private bool isWaitingForNextWave = false;

    private void DifficultyIncrease()
    {
        waveEnemyCount += waveEnemyCounStepAdd; // Increase enemy count
        if (currentWave < BabyWaveCount)
        {
            WaveStartDelay += WaveStartDelayStepAdd; // Increase delay between waves
        }
        else
        {
            WaveStartDelay -= WaveStartDelayStepSub; // Decrease delay between waves
        }
    }

    void Update()
    {
        CurrentPlayerScore = PlayerScore.CurrentScore;
        //Debug.Log($"Current all Player Score: {PlayerScore.SumScore}");
        /*Debug.Log($"Current Player Score: {CurrentPlayerScore}");
        Debug.Log($"Current Wave: {currentWave}");
        Debug.Log($"Needed scpre to start wave: {(waveEnemyCount -1) * 5}");*/
        if (CurrentPlayerScore >= 5 * (waveEnemyCount -1)) // has minimal score to start wave. Like start wave if has minimal rewards score from enemies
        {
            if (WaveCanStart)
            {
                WaveCanStart = false;
                StartCoroutine(SpawnWave());
                isWaitingForNextWave = false;
            }
            else if (!isWaitingForNextWave)
            {
                StartCoroutine(StartWave());
                isWaitingForNextWave = true;
            }
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(WaveStartDelay);
        WaveCanStart = true;
        shouldIncreaseDifficulty = true;
    }

    private void SpawnEnemy(Vector3 position)
    {
        if (Random.Range(1, 10) > 3) // 70% chance to spawn the first enemy type
            Instantiate(EnemyPrefab, position, Quaternion.identity);
        else // 30% chance to spawn the second enemy type
        {
            Instantiate(SecondEnemyPrefab, position, Quaternion.identity);
        }
    }

    private IEnumerator SpawnWave()
    {
        currentWave += 1;
        PlayerScore.ResetScore();

        for (int i = 0; i < waveEnemyCount; i++)
        {
            SpawnEnemy(GetRandomSpawnPosition());
            yield return new WaitForSeconds(GetRandomSpawnDelay());
        }
        // Increase difficulty after the wave ends
        if (shouldIncreaseDifficulty)
        {
            DifficultyIncrease();
            shouldIncreaseDifficulty = false;
        }
    }

    private float GetRandomSpawnDelay()
    {
        return Random.Range(EnemyMinSpawnDelay, EnemyMaxSpawnDelay);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Random.Range(0f, SpawnRadius);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        return transform.position + offset;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
    }
}
