using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : Manager
{
    [SerializeField]
    private GameObject _runUIBand;
    [SerializeField]
    private TextMeshProUGUI _stairsText, _goldText,_healthText;
    [SerializeField]
    private GameObject _feedbackUI, _packIcon;
    [SerializeField]
    private GatchaUIMaster _gatchaUI;
    [SerializeField]
    private RewardMaster _rewardMaster;
    [SerializeField]
    private ElementPackMaster _packUIMaster;

    private JourneyManager _journeyManager;
    private PlayerManager _playerManager;
    private AudioManager _audioManager;

    public bool IsUIOnFront()
    {
        return 
            _packUIMaster.gameObject.activeInHierarchy ||
            _gatchaUI.gameObject.activeInHierarchy ||
            _rewardMaster.gameObject.activeInHierarchy;
    }
    public override void ManagerPreAwake()
    {
        _journeyManager = ManagerManager.GetManager<JourneyManager>();
        _playerManager = ManagerManager.GetManager<PlayerManager>();
        _audioManager = ManagerManager.GetManager<AudioManager>();
    }
    public override void ManagerPostAwake()
    {
        _playerManager.onGoldChanged.Subscribe(UpdateGold);
        _playerManager.onHpChanged.Subscribe(UpdateHealth);
        _journeyManager.onFloorUp.Subscribe(UpdateStairs);
    }

    public void DisplayRunUIBand(bool isActive = true)
    {
        _runUIBand.SetActive(isActive);
        DisplayPackIcon(isActive);

    }
    public void DisplayPackIcon(bool value)
    {
        _packIcon.SetActive(value);
    }
    public void DisplayBattleFeedbackUI(bool isActive)
    {
        _feedbackUI.SetActive(isActive);
    }

    public void ActivatePackUI(bool toDelete = false)
    {
        if(toDelete)
        {
            _packUIMaster.elementDeletePackSubMaster.use = true;
        }
        _packUIMaster.gameObject.SetActive(true);
        _packIcon.SetActive(false);
    }
    public void DesactivatePackUI()
    {
        _packUIMaster.gameObject.SetActive(false);
        _packIcon.SetActive(true);
        _packUIMaster.elementDeletePackSubMaster.use = false;
        _packUIMaster.elementDeletePackSubMaster.Reset();

    }
    public void InitGatchaUIButton(GameObject button = null)
    {
        _gatchaUI.Init(button);
    }
    public void ActivateGatchaUI(bool value)
    {
        _gatchaUI.gameObject.SetActive(value);
        if(value)
        {
            //_audioManager.LoadTempMusic("Motus");
        }
        else
        {
            _audioManager.ContinueMusic();
        }
    }
    public void ActivateRewardMaster(bool value)
    {
        //J'ai mis le changement de scene direct dans le bouton
        _rewardMaster.gameObject.SetActive(value);
        if(value)
        {
            _rewardMaster.Init();
        }
    }

    public void UpdateStairs(int i)
    {
        _stairsText.text = i.ToString("00");
    }
    public void UpdateGold(int i)
    {
        _goldText.text = i.ToString("000");
    }
    public void UpdateHealth(int i)
    {
        _healthText.text = $"{i.ToString("00")}/{_playerManager.HpMax}";
    }
    
    
    public override void ManagerOnEachSceneLeft(Scene scene)
    {
        DisplayRunUIBand(false);
        DisplayBattleFeedbackUI(false);
        DesactivatePackUI();
    }


}
