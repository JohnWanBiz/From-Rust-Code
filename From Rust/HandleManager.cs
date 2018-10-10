using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;

public class HandleManager : MonoBehaviour //manages the placement of the handles as well as their function
{
	[SerializeField]
	GameObject characterSheet; //character sheet object to be assigned

	[SerializeField]
	GameObject extender; //sprite that attaches to the character sheet to allow for it to extend past the other handles

    [SerializeField]
	bool expanded = false;
    public bool Expanded { get { return expanded; } }

	int playerNum;

	CharacterName charaName;
    public CharacterName CharacterName { get { return charaName; } }

    public bool CanBeToggled;

	//variables for handle/extender/sheet expand & collapse
	float distance = 2.065f;
	const float duration = 0.7f;
	//sheet
	const float sheetExpaX = -9.22f;
	const float sheetCollX = -14.2f;
	float sheetPlaceY = -0.77f;
	//handle
	const float handlePlaceX = -10.2f;
	float handlePlaceY = -.2f;
	//extender
	const float extdrPlaceX = -15f;
	float extdrPlaceY = 0.2f;


	void Start()
	{
		SubscribeToEventBus();
		playerNum = characterSheet.GetComponent<CharacterInfoDisplayBehaviour>().PlayerNum;
		charaName = characterSheet.GetComponent<CharacterInfoDisplayBehaviour>().CharaName; //fetches assigned character name
		Placing(); //places the handles and extender in the right place
        CanBeToggled = true;
	}


	public void OnClick() //when the handle is clicked 
	{
        if (CanBeToggled)
        {
            expanded = !expanded;
            if (expanded)
            {
                EventBus.ClientInstance.Broadcast(EventConstants.SheetOpenClient, this, new CharacterNameArgs(charaName)); //broadcasting Character names
                Expand();
            }
            else
            {
                Collapse();
            }
        }		
	}


	void Expand() //opens up the chara sheets 
	//get this to close other chara sheets that are open 
	{
		FindObjectOfType<AudioManager>().PostEvent("HeroDrawer");
		characterSheet.transform.DOMove(new Vector3(sheetExpaX, characterSheet.transform.position.y), duration).SetEase(Ease.OutSine); //character sheet
	}


	void Collapse() //collapses the character sheet
	{
		FindObjectOfType<AudioManager>().PostEvent("HeroDrawer"); 
		characterSheet.transform.DOMove(new Vector3(sheetCollX, characterSheet.transform.position.y), duration).SetEase(Ease.OutSine); //character sheet
	}


	void Placing() //placing the handles at the start of the game
	{
		characterSheet.transform.position = new Vector3(sheetCollX, sheetPlaceY); //handle placement
		this.transform.position = new Vector3(handlePlaceX, handlePlaceY - (distance * playerNum)); //handle placement
		extender.transform.position = new Vector3(extdrPlaceX, extdrPlaceY - (distance * playerNum)); //extender placement
	}


	void OnSheetOpened(object source, EventArgs args) //a check that runs when a character sheet is open and a name is broadcast
	{ 
        if (!CharacterManager.GetCharacterByName(charaName).isHidden)
        {
            CharacterNameArgs castArgs = (CharacterNameArgs)args; //character name being made into a CharacterName Argument
            if (castArgs.CharacterName != charaName) //checks if the broadcasted name is the name of the sheet
            {
                Collapse(); //tells the sheet to collapse if it's not being broadcast
            }
        }

	}


	void SubscribeToEventBus()
	{
		EventBus.ClientInstance.Subscribe(EventConstants.SheetOpenClient, OnSheetOpened); //listening for SheetOpen
	}
		
}
