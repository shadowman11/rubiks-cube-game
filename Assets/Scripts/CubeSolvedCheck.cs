using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeSolvedCheck : MonoBehaviour
{
    private GameTimer gameTimer;

    public Transform cubeRoot;
    public List<Transform> cubies = new List<Transform>();

    private List<Vector3> startLocalPositions = new List<Vector3>();
    private List<Quaternion> startLocalRotations = new List<Quaternion>();

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
            startLocalPositions.Add(cubie.localPosition);
            startLocalRotations.Add(cubie.localRotation);
        }
<<<<<<< HEAD

        timer = checkDelay;
=======
>>>>>>> 05917176b493b3af60500d3353b4d32917db7d2b
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
<<<<<<< HEAD
            if (Vector3.Distance(cubies[i].localPosition, startLocalPositions[i]) > positionTolerance)
                return false;

            if (Quaternion.Angle(cubies[i].localRotation, startLocalRotations[i]) > rotationTolerance)
=======
            if (Quaternion.Angle(cubies[i].rotation, startRotations[i]) > rotationTolerance)
            {
                firstMove = true;
>>>>>>> 05917176b493b3af60500d3353b4d32917db7d2b
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
