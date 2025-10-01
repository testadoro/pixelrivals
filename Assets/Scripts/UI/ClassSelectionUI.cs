using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassSelectionUI : MonoBehaviour
{
    [Header("Class Selection")]
    public PlayerClassSO[] availableClasses;
    public Button[] classButtons;
    public Image[] classIcons;
    public TextMeshProUGUI[] classNames;
    
    [Header("Skin Selection")]
    public Button[] skinButtons;
    public Image[] skinPreviews;
    public TextMeshProUGUI[] skinNames;
    
    [Header("Confirmation")]
    public Button startGameButton;
    public TextMeshProUGUI selectedClassText;
    public TextMeshProUGUI selectedSkinText;
    
    private PlayerClassSO selectedClass;
    private int selectedSkinIndex = 0;
    
    private void Start()
    {
        InitializeClassButtons();
        UpdateStartButton();
    }
    
    private void InitializeClassButtons()
    {
        for (int i = 0; i < availableClasses.Length && i < classButtons.Length; i++)
        {
            int index = i;
            PlayerClassSO classData = availableClasses[i];
            
            if (classButtons[i] != null)
            {
                classButtons[i].onClick.AddListener(() => SelectClass(index));
            }
            
            if (classIcons[i] != null && classData.classIcon != null)
            {
                classIcons[i].sprite = classData.classIcon;
            }
            
            if (classNames[i] != null)
            {
                classNames[i].text = classData.className;
            }
        }
        
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }
    }
    
    public void SelectClass(int classIndex)
    {
        if (classIndex < 0 || classIndex >= availableClasses.Length)
            return;
        
        selectedClass = availableClasses[classIndex];
        selectedSkinIndex = 0;
        
        if (selectedClassText != null)
        {
            selectedClassText.text = $"Selected: {selectedClass.className}";
        }
        
        UpdateSkinSelection();
        UpdateStartButton();
    }
    
    private void UpdateSkinSelection()
    {
        if (selectedClass == null || selectedClass.availableSkins == null)
        {
            HideSkinButtons();
            return;
        }
        
        // Initialize skin buttons
        for (int i = 0; i < selectedClass.availableSkins.Length && i < skinButtons.Length; i++)
        {
            int index = i;
            SkinData skin = selectedClass.availableSkins[i];
            
            if (skinButtons[i] != null)
            {
                skinButtons[i].gameObject.SetActive(true);
                skinButtons[i].onClick.RemoveAllListeners();
                skinButtons[i].onClick.AddListener(() => SelectSkin(index));
            }
            
            if (skinNames[i] != null)
            {
                skinNames[i].text = skin.skinName;
            }
        }
        
        // Hide unused buttons
        for (int i = selectedClass.availableSkins.Length; i < skinButtons.Length; i++)
        {
            if (skinButtons[i] != null)
            {
                skinButtons[i].gameObject.SetActive(false);
            }
        }
        
        SelectSkin(0);
    }
    
    public void SelectSkin(int skinIndex)
    {
        selectedSkinIndex = skinIndex;
        
        if (selectedSkinText != null && selectedClass != null)
        {
            selectedSkinText.text = $"Skin: {selectedClass.availableSkins[skinIndex].skinName}";
        }
    }
    
    private void HideSkinButtons()
    {
        foreach (var button in skinButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
    
    private void UpdateStartButton()
    {
        if (startGameButton != null)
        {
            startGameButton.interactable = (selectedClass != null);
        }
    }
    
    private void StartGame()
    {
        if (selectedClass == null)
            return;
        
        // Store selection for game start
        GameManager.Instance.SetPlayerClass(selectedClass, selectedSkinIndex);
        
        // Load game scene (this would normally load the arena scene)
        Debug.Log($"Starting game with {selectedClass.className} - {selectedClass.availableSkins[selectedSkinIndex].skinName}");
        
        // For now, just hide this UI
        gameObject.SetActive(false);
    }
}
