using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChoicesScript : MonoBehaviour {

    [Tooltip("The audio clip that playes when the button is highlighted")]
    public AudioClip audioClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called when one of the choice buttons is highlighted
    /// </summary>
    public void ButtonHighlighted()
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void yesButtonClicked()
    {
        MovementBehaviour character = GlobalReferencesBehaviour.instance.SceneData.controller.GetComponent<ControllerBehaviour>().ControllerLogic.CurrentChar;

        IChoiceLogic choiceLogic = new YesChoiceLogic();

        if(character)
            character.GetComponent<ExecuteChoicesBehaviour>().ExecuteChoices.ExecuteChoice(choiceLogic);
        
        gameObject.SetActive(false);

    }

    public void noButtonClicked()
    {
        MovementBehaviour character = GlobalReferencesBehaviour.instance.SceneData.controller.GetComponent<ControllerBehaviour>().ControllerLogic.CurrentChar;

        IChoiceLogic choiceLogic = new NoChoiceLogic();

        if (character)
            character.GetComponent<ExecuteChoicesBehaviour>().ExecuteChoices.ExecuteChoice(choiceLogic);
        
        gameObject.SetActive(false);
    }
}
