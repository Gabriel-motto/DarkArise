using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Cinemachine;

public class CharacterManager : MonoBehaviour
{
    public static Vector2 lastCheckPointPos = new Vector2(-3, -4);
    public GameObject[] playerPrefabs;
    public int characterIndex;
    public CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Awake()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject charac = Instantiate(playerPrefabs[characterIndex], lastCheckPointPos, Quaternion.identity);
        virtualCamera.m_Follow = charac.transform;
        foreach (GameObject player in playerPrefabs)
        {
            player.SetActive(false);
        }
        playerPrefabs[characterIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeNext();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangePrevious();
        }
    }

    public void ChangeNext()
    {
        playerPrefabs[characterIndex].SetActive(false);
        characterIndex++;
        if (characterIndex == playerPrefabs.Length)
        {
            characterIndex = 0;
        }
        playerPrefabs[characterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }

    public void ChangePrevious()
    {
        playerPrefabs[characterIndex].SetActive(false);
        characterIndex--;
        if (characterIndex == -1)
        {
            characterIndex = playerPrefabs.Length - 1;
        }
        playerPrefabs[characterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }
}
