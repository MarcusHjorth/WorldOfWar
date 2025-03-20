using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
  public GameObject[] characters;
  public int selectedCharacter = 0;
  public String _sceneName = "Elwynn Forest";
  public TMP_Text label;

  void Start()
  {
      if (characters == null || characters.Length == 0)
      {
          return;
      }

      // Deactivate all characters
      foreach (var character in characters)
      {
          character.SetActive(false);
      }

      // Activate only the first character
      characters[selectedCharacter].SetActive(true);
      label.text = characters[selectedCharacter].name;
      
  }

  public void NextCharacter()
  {
      characters[selectedCharacter].SetActive(false);
      selectedCharacter = (selectedCharacter + 1) % characters.Length;
      characters[selectedCharacter].SetActive(true);
      label.text = characters[selectedCharacter].name;
  }

  public void PreviousCharacter()
  {
      characters[selectedCharacter].SetActive(false);
      selectedCharacter--;
      if (selectedCharacter < 0)
      {
          selectedCharacter += characters.Length;
      }
      characters[selectedCharacter].SetActive(true);
      label.text = characters[selectedCharacter].name;
  } 
  public void StartGame()
  {
      PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
      SceneManager.LoadScene(_sceneName);
  }

  
}
