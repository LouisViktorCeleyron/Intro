using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OponentInstance : FightingInstance
{
    private OpponentData _oponentData;
    [SerializeField]
    private TextMeshProUGUI _opponentName;
    [SerializeField]
    private GameObject _attackPreview;
    [SerializeField]
    private TextMeshProUGUI _consNameText,_consDescText;
    private Consequence _nextConsequence;

    public void Start()
    {
        _battleManager.opponentInstance = this;
    }
    public void Initialise(OpponentData oponentData)
    {
        //Debug.Log("coucou");
        _oponentData = oponentData;
        if (_oponentData.opponentPrefab != null )
        {
            Instantiate(_oponentData.opponentPrefab, transform);
        }
        _opponentName.text = oponentData.name;  
        Stats.SetHpMax (oponentData.baseHp);
        Stats.SetHp (oponentData.baseHp);
        Stats.SetStrength(oponentData.baseStrengh);
        Stats.SetBulk(oponentData.baseBulk);

        foreach (var status in oponentData.startingStatus)
        {
            UpdateStatus(status);
        }

    }
    public void SetConsequence()
    {
        _nextConsequence = _oponentData.GetRandomConsequence();
        _consDescText.text = _nextConsequence.GetDescription(this);
        _consNameText.text = _nextConsequence.name;
    }
    public void LaunchConsequence(BattleManager battleManager)
    {
        _nextConsequence.CallConsequence(battleManager.opponentInstance, battleManager.playerInstance);
    }

    private void OnMouseEnter()
    {
        _attackPreview.SetActive(true);
    }
    private void OnMouseExit()
    {
        _attackPreview.SetActive(false);
    }

    public override void EndOfBattle()
    {
        _battleManager.EndOfBattle(false);
    }
}
