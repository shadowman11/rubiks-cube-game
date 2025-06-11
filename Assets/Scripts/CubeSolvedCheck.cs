using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeSolvedCheck : MonoBehaviour
{
    private GameTimer gameTimer;

    public Transform cubeRoot;
    public List<Transform> cubies = new List<Transform>();

    private List<Vector3> startLocalPositions = new List<Vector3>();
    private List<Quaternion> startLocalRotations = new List<Quaternion>();

    public float checkDelay = 2f; // seconds to wait before checking
    private float timer = 0f;
    public float positionTolerance = 0.001f;
    public float rotationTolerance = 0.1f;

    private bool winTriggered = false;

    void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();

        foreach (Transform cubie in cubies)
        {
            startLocalPositions.Add(cubie.localPosition);
            startLocalRotations.Add(cubie.localRotation);
        }

        timer = checkDelay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (!winTriggered && IsCubeSolved() && timer <= 0f)
        {
            Debug.Log("🎉 Cube is solved!");
            winTriggered = true;
            WinMenu.hasWon = true;
            gameTimer.StopTimer();
        }
    }

    bool IsCubeSolved()
    {
        for (int i = 0; i < cubies.Count; i++)
        {
            if (Vector3.Distance(cubies[i].localPosition, startLocalPositions[i]) > positionTolerance)
                return false;

            if (Quaternion.Angle(cubies[i].localRotation, startLocalRotations[i]) > rotationTolerance)
                return false;
        }

        return true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
