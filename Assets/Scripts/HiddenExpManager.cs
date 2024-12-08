using UnityEngine;

public class HiddenExpManager : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] private AnimationCurve experienceCurve; // Experience curve to determine required EXP
    [SerializeField] private int maxLevel = 10; // Maximum level for spawn rate increases

    private int _currentLevel = 0;
    private int _currentExp = 0;
    private int _expToNextLevel;

    public delegate void SpawnRateIncrease();
    public static event SpawnRateIncrease OnSpawnRateIncreased;

    private void Start()
    {
        _expToNextLevel = CalculateExpToNextLevel();
        Debug.Log($"EXP required for level {_currentLevel + 1}: {_expToNextLevel}");
    }

    public void AddExp(int amount)
    {
        _currentExp += amount;
        Debug.Log($"Hidden EXP: {_currentExp} / {_expToNextLevel}");

        if (_currentExp >= _expToNextLevel && _currentLevel < maxLevel)
        {
            _currentExp -= _expToNextLevel; // Carry over remaining EXP
            _currentLevel++;
            OnSpawnRateIncreased?.Invoke();

            Debug.Log($"Spawn rate increased! New Level: {_currentLevel}");
            _expToNextLevel = CalculateExpToNextLevel();
        }
    }

    private int CalculateExpToNextLevel()
    {
        // Use the curve to determine the EXP required for the next level
        float curveValue = experienceCurve.Evaluate((float)_currentLevel / maxLevel);
        return Mathf.CeilToInt(curveValue * 100); // Scale the curve value to your desired range
    }
}