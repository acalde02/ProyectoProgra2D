using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] AnimationCurve experienceCurve;
    
    [Header("Interface Settings")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;
    
    
    [SerializeField] GameObject player;


    private LvlControler _lvlManager;
    private int _gatheredExperience;
    private int _experienceToNextLevel;
    private int _currentLevel;
    

    private void Start()
    {
        // Get the reference to the LvlControler component
        _lvlManager = player.GetComponent<LvlControler>();
    
        // Initialize the current values
        _currentLevel = _lvlManager.Getnivel();
        _gatheredExperience = _lvlManager.ReturnGatheredExperience();
        _experienceToNextLevel = _lvlManager.ReturnTotalExperience();

        // Update the UI immediately
        UpdateInterface();
    }


    public void UpdateInterface()
    {
        // Fetch updated values from LvlControler
        _currentLevel = _lvlManager.Getnivel(); // Ensure this always reflects the current level
        _gatheredExperience = _lvlManager.ReturnGatheredExperience();
        _experienceToNextLevel = _lvlManager.ReturnTotalExperience();
        
        

        // Update UI
        levelText.text = _currentLevel.ToString();
        if (_currentLevel <= 3)
        {
            experienceText.text = $"{_gatheredExperience} exp / {_experienceToNextLevel} exp";
            experienceFill.fillAmount = (float)_gatheredExperience / _experienceToNextLevel;
        }
        else
        {
            experienceText.text = $"{_gatheredExperience} exp";
            experienceFill.fillAmount = (float)_gatheredExperience / _experienceToNextLevel;
        }
        
        
    }

    

}
