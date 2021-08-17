using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject playerRocket;
    private Vector3 rocketSpawningCheckpoint;
    public void SetRocketSpawningCheckpoint(Vector3 inPoint)
    {
        this.rocketSpawningCheckpoint = inPoint;
    }

    private void Awake()
    {
        int levelLoaders = GameObject.FindObjectsOfType<LevelLoader>().Length;

        if(levelLoaders > 1)
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void RestartLevel()
    {
        if (GameObject.FindObjectOfType<CollisionHandler>().IsCheckpointActivated)
        {
            GameObject.Destroy(GameObject.FindObjectOfType<CollisionHandler>().gameObject);
            GameObject player = Instantiate<GameObject>(this.playerRocket, this.rocketSpawningCheckpoint, Quaternion.identity);
            GameObject.FindWithTag("Virtual Camera").GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            GameObject.FindObjectOfType<CollisionHandler>().IsCheckpointActivated = true;
        }
        else
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(currentScene);
        }
    }

    public void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentScene + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            this.RestartGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);

    }
}
