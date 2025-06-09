using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scrambler : MonoBehaviour
{
    [Header("Scramble Settings")]
    [Tooltip("Number of random face turns")]
    public int scrambleMoves = 20;
    [Tooltip("Seconds to wait for each 90° turn to finish")]
    public float moveDelay = 0.6f;

    private CubeState cubeState;
    private ReadCube readCube;
    private GameTimer gameTimer;
    private bool timerStopped = false;

    void Awake()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube  = FindObjectOfType<ReadCube>();
        gameTimer = FindObjectOfType<GameTimer>();
    }

    void Start()
    {
        // only auto-scramble in timed mode
        if (ModeIndicator.isTimed)
            StartCoroutine(ScrambleAndStartTimer());
    }

    private IEnumerator ScrambleAndStartTimer()
    {
        // let the cube & rays fully initialize
        yield return new WaitForSeconds(0.2f);

        // seed the ray-based state so faces[4] is valid
        readCube.ReadState();
        yield return null;

        // cache the three pivots the player can turn
        var pivots = new List<Transform>
        {
            cubeState.up[4].transform.parent,
            cubeState.front[4].transform.parent,
            cubeState.right[4].transform.parent
        };

        for (int i = 0; i < scrambleMoves; i++)
        {
            // pick one pivot at random
            var pivot = pivots[Random.Range(0, pivots.Count)];

            // rotate its face 90° around the face-normal
            pivot.Rotate(pivot.forward, 90f, Space.World);

            // wait for it to visibly finish
            yield return new WaitForSeconds(moveDelay);

            // rebuild cubeState lists via raycasts
            readCube.ReadState();
        }

        // now start the timer
        gameTimer.StartTimer();
    }

    void Update()
    {
        // stop the timer exactly once when solved
        if (ModeIndicator.isTimed && !timerStopped && IsSolved())
        {
            gameTimer.StopTimer();
            timerStopped = true;
        }
    }

    private bool IsSolved()
    {
        return CheckFace(cubeState.up)
            && CheckFace(cubeState.front)
            && CheckFace(cubeState.right);
    }

    private bool CheckFace(List<GameObject> face)
    {
        if (face == null || face.Count < 9) return false;
        char first = face[4].name[0];
        foreach (var cube in face)
            if (cube.name[0] != first)
                return false;
        return true;
    }
}
