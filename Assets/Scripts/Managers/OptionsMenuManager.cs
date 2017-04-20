using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class OptionsMenuManager : MonoBehaviour {

	public GameObject optionsMenu;
	public RectTransform menuContent;
	
	public OptionsMenuButton buttonPrefab;
	public delegate string TextUpdate();

	public AudioMixer mixer;

	public HiddenText[] hiddenCreditsTexts = new HiddenText[3];

	public void MainMenu() {
		ClearMenu();
		CreateButton(GetQualitySettingsText(), NextQualityLevel, GetQualitySettingsText);
		CreateButton(GetMusicVolume(), IncrementMusicVolume, GetMusicVolume);
		CreateButton(GetSFXVolume(), IncrementSFXVolume, GetSFXVolume);
		CreateButton("Set Controls", ControlsMenu);
		CreateButton("Credits", CreditsMenu);
		CreateButton("Close Options Menu", CloseOptionsMenu);
	}

	public void OpenOptionsMenu() {
		optionsMenu.SetActive(true);
		MainMenu();
	}

	public void CloseOptionsMenu() {
		optionsMenu.SetActive(false);
		ClearMenu();
	}

	void ClearMenu() {
		Transform[] UIChildren = menuContent.GetComponentsInChildren<Transform>();
		foreach (Transform child in menuContent) {
			 Destroy(child.gameObject);
		 }
	}

	OptionsMenuButton CreateButton(string defaultText, UnityAction action) {
		return CreateButton(defaultText, action, null);
	}

	OptionsMenuButton CreateButton(string defaultText, UnityAction action, TextUpdate textUpdate ) {
		OptionsMenuButton b = Instantiate(buttonPrefab, menuContent);
		b.text.text = defaultText;
		b.button.onClick.AddListener( action );
		if(textUpdate != null)
			b.button.onClick.AddListener( delegate { b.text.text = textUpdate(); Debug.Log(textUpdate()); } );
		b.transform.ResetTransform();

		return b;
	}


	//Video
	void NextQualityLevel() {
		int nextLevel = (QualitySettings.GetQualityLevel() + 1) % QualitySettings.names.Length;
		QualitySettings.SetQualityLevel(nextLevel, true);
	}
	string GetQualitySettingsText() {
		int currentLevel = QualitySettings.GetQualityLevel() + 1;
		int levelCount = QualitySettings.names.Length;
		return string.Format("Quality ({0}/{1}): {2}", currentLevel, levelCount, QualitySettings.names[ QualitySettings.GetQualityLevel() ]);
	}

	//Audio
	void IncrementMusicVolume() {
		IncrementVolume("MusicVolume");
	}
	void IncrementSFXVolume() {
		IncrementVolume("SFXVolume");
	}
	void IncrementVolume(string param) {
		float curVolume;
		mixer.GetFloat(param, out curVolume);

		curVolume = dB2Linear(curVolume);
		curVolume = (curVolume + 0.1f) % 1f;
		curVolume = Mathf.RoundToInt(curVolume * 10f) / 10f;
		curVolume = Linear2DB(curVolume);

		mixer.SetFloat(param, curVolume);
	}
	string GetLinearVolumeFromParam(string title, string param) {
		float volume;
		mixer.GetFloat(param, out volume);
		volume = dB2Linear(volume);

		return title + (volume * 100).ToString("F0") + "%";
	}
	string GetMusicVolume() {
		return GetLinearVolumeFromParam("Music: ", "MusicVolume");
	}
	string GetSFXVolume() {
		return GetLinearVolumeFromParam("SFX: ", "SFXVolume");
	}
	public static float dB2Linear (float dB) {
		return Mathf.Pow(10, dB / 20f);
	}
	public static float Linear2DB(float t) {
		return Mathf.Max(-80f, 20f * Mathf.Log10(t)); //log(0) = -Infinity
	}

	//Control
	public void ControlsMenu() {
		ClearMenu();
		for(int i = 0; i < 4; i++) {
			int pNumber = i+0;
			CreateButton("Player "+(pNumber+1), delegate { PlayerControlMenu(pNumber); });
		}
		CreateButton("Reset All Inputs", cInput.ResetInputs);
		CreateButton("Back to Main Menu", MainMenu);
	}
	public void PlayerControlMenu(int playerNumber) {
		ClearMenu();

		foreach(string inputName in VirtualControlManager.inputNames) {
			OptionsMenuButton b = CreateButton(InputNameText(inputName, playerNumber), delegate { cInput.ChangeKey(inputName+playerNumber); });
			b.alwaysUpdateTitle = true;
			b.textUpdate = delegate { return InputNameText(inputName, playerNumber); };
		}

		//OptionsMenuButton b = CreateButton("Set Controls for Player "+(playerNumber+1), delegate { });
		//b.button.onClick.AddListener( delegate { SetControlsForPlayer(playerNumber, b); } );

		//CreateButton("Reset All Controls", delegate { cInput.ResetInputs(); PlayerControlMenu(playerNumber); });

		CreateButton("Back", ControlsMenu);
	}
	public void SetControlsForPlayer(int player, OptionsMenuButton button) {
		StartCoroutine(ControlWizard(player, button));
	}
	public IEnumerator ControlWizard(int playerNumber, OptionsMenuButton button) {
		VirtualControlManager.SetupDefaultControls(playerNumber);

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		foreach (string inputName in VirtualControlManager.inputNames) {
			cInput.ChangeKey(inputName+playerNumber);
			button.text.text = string.Format("Press key/button for {0}", inputName);

			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();

			yield return new WaitUntil( () => !cInput.scanning );
		}

		button.text.text = "Set controls for player " + (playerNumber+1);
	}
	string InputNameText(string inputName, int playerNumber) {
		if(cInput.scanning)
			return "Scanning...";
		return string.Format("{0} - {1}", inputName, cInput.GetText(inputName+playerNumber));
	}

	//Credits
	public void CreditsMenu() {
		ClearMenu();
		CreateButton("Michael Starling", delegate { }, delegate { return GetHiddenText(0); });
		CreateButton("Jacob Sy", delegate { }, delegate { return GetHiddenText(1); });
		CreateButton("Jesse Pan", delegate { }, delegate { return GetHiddenText(2); });
		CreateButton("Placeholder Music\nViper Phase I", delegate { });
		CreateButton("Back to Main Menu", MainMenu);
	}
	string GetHiddenText(int textIndex) {
		if(textIndex < 0 || hiddenCreditsTexts.Length < textIndex-1)
			return string.Empty;

		if(hiddenCreditsTexts[textIndex] != null && hiddenCreditsTexts[textIndex].hiddenText != null && hiddenCreditsTexts[textIndex].hiddenText.Length > 0)
			return hiddenCreditsTexts[textIndex].hiddenText[Random.Range(0, hiddenCreditsTexts[textIndex].hiddenText.Length)];
		else
			return string.Empty;

	}

	[System.Serializable]
	public class HiddenText {
		public string name;
		public string[] hiddenText;
	}

}