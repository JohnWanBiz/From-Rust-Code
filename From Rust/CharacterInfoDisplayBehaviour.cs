using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using TMPro;

public class CharacterInfoDisplayBehaviour : MonoBehaviour //displays character info on the chararcter sheet
{ 
    [SerializeField]
    Text characterNameText; //name of the character

	[SerializeField]
	Text playerNameText; //name of the controller

	[SerializeField]
	TextMeshProUGUI combatPointsText; //net combat points

	[SerializeField]
    TextMeshProUGUI explorationPointsText; //net exploration points

	[SerializeField]
	int [] activeAbilities; //arrays for the abilities (int is just a placeholder) 

	[SerializeField]
	int [] passiveAbilities;//arrays for the abilities (int is just a placeholder) 

    CharacterAsset asset;

	private int playerNum;
	public int PlayerNum { get {return playerNum; } }

	private CharacterName charaName;
	public CharacterName CharaName { get {return charaName; } }

	void Start ()
	{
        SubscribeToEventBus();
	}
		
	public void SetCharacterInfo(CharacterAsset asset, int playerNumber) //finds the character and assigns the proper texts
    {
		this.asset = asset;
		gameObject.SetActive(true);
		playerNum = playerNumber;
		charaName = asset.CharacterName;
        characterNameText.text = asset.DisplayName.ToUpper();
        StartCoroutine(WaitAndSetCharacterInfo());
    }

    IEnumerator WaitAndSetCharacterInfo()
    {
        while (Player.GetPlayerOwnerOfCharacter(CharacterManager.GetCharacterByName(asset.CharacterName)) == null)
        {
            yield return new WaitForEndOfFrame();
        }
        playerNameText.text = "Controlled by " + Player.GetPlayerOwnerOfCharacter(CharacterManager.GetCharacterByName(asset.CharacterName)).playerName;
        combatPointsText.text = "<sprite=0>: " + asset.CP; //displays current power
        explorationPointsText.text = "<sprite=1>  : " + asset.EP; //displays current power
        Player.GetLocalPlayer().RequestPlayerSync(); //syncs data with other players
    }
		
	void UpdateInfo(object source, EventArgs args)
	{
		combatPointsText.text = "<sprite=0>: " + CharacterManager.GetCharacterByName(charaName).GetCardObject().GetComponent<CardDataPopulator>().CPText.text; //displays current power
		explorationPointsText.text = "<sprite=1>  : " + CharacterManager.GetCharacterByName(charaName).GetCardObject().GetComponent<CardDataPopulator>().EPText.text; //displays current power
	}
		
    void SubscribeToEventBus()
    {
		EventBus.ClientInstance.Subscribe(EventConstants.UpdateCharacterSheet(charaName.ToString()), UpdateInfo);
    }

}