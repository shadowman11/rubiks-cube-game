using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeSolvedCheck : MonoBehaviour
{
    private GameTimer gameTimer;

    public List<Transform> cubies = new List<Transform>();

    private List<Vector3> startPositions = new List<Vector3>();
    private List<Quaternion> startRotations = new List<Quaternion>();

    public float positionTolerance = 0.001f;
    public float rotationTolerance = 0.1f;
    public bool firstMove;

    private bool winTriggered = false;

    void Start()
    {
        firstMove = false;
        gameTimer = FindAnyObjectByType<GameTimer>();
        foreach (Transform cubie in cubies)
        {
            startPositions.Add(cubie.position);
            startRotations.Add(cubie.rotation);
        }
    }

    void Update()
    {
        if (!winTriggered && IsCubeSolved() && firstMove)
        {
            winTriggered = true;
            WinMenu.hasWon = true;
            gameTimer.StopTimer();
        }
    }

    bool IsCubeSolved()
    {
        if (PivotRotation.moving) return false;

        for (int i = 0; i < cubies.Count; i++)
        {
            if (Quaternion.Angle(cubies[i].rotation, startRotations[i]) > rotationTolerance)
            {
                firstMove = true;
                return false;
            }
            if (Vector3.Distance(cubies[i].position, startPositions[i]) > positionTolerance)
            {
                return false;
            }
        }

        return true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
