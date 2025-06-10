// Scrambler.cs
// -------------
// Simplified scramble using manual face-pivot animation (no PivotRotation)
// Only rotates Up, Front, Right faces one at a time, smoothly over duration.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrambler : MonoBehaviour
{
    [Tooltip("Number of moves to scramble")] public int scrambleMoves = 20;
    [Tooltip("Duration in seconds of each face turn")] public float turnDuration = 1.0f;
    [Tooltip("Delay after each turn before next move")] public float delayBetweenMoves = 0.2f;

    private CubeState cubeState;
    private ReadCube readCube;
    private GameTimer gameTimer;

    void Start()
    {
        cubeState = Object.FindAnyObjectByType<CubeState>();
        readCube  = Object.FindAnyObjectByType<ReadCube>();
        gameTimer = Object.FindAnyObjectByType<GameTimer>();

        // Prime cube state
        readCube.ReadState();

        if (ModeIndicator.isTimed)
            StartCoroutine(ScrambleRoutine());
    }

    private IEnumerator ScrambleRoutine()
    {
        var faces = new List<System.Func<(List<GameObject> face, Vector3 axis)>>
        {
            // Up face: axis = pivot.up
            () =>
            {
                var face = cubeState.up;
                var axis = face[4].transform.parent.up;
                return (face, axis);
            },
            // Front face: axis = pivot.forward
            () =>
            {
                var face = cubeState.front;
                var axis = face[4].transform.parent.forward;
                return (face, axis);
            },
            // Right face: axis = pivot.right
            () =>
            {
                var face = cubeState.right;
                var axis = face[4].transform.parent.right;
                return (face, axis);
            }
        };

        for (int i = 0; i < scrambleMoves; i++)
        {
            // pick random move
            var entry = faces[Random.Range(0, faces.Count)]();
            var face = entry.face;
            var axis = entry.axis;
            float angle  = (Random.value > 0.5f) ? 90f : -90f;

            // animate the face turn
            yield return StartCoroutine(AnimateFaceTurn(face, axis, angle, turnDuration));

            // small delay
            yield return new WaitForSeconds(delayBetweenMoves);
        }

        // start timer when done
        gameTimer.StartTimer();
    }

    private IEnumerator AnimateFaceTurn(List<GameObject> face, Vector3 axis, float angle, float duration)
    {
        // Pickup: reparent face cubes under pivot
        cubeState.Pickup(face);
        Transform pivot = face[4].transform.parent;

        Quaternion startRot = pivot.rotation;
        Quaternion endRot   = pivot.rotation * Quaternion.AngleAxis(angle, axis);

        float time = 0f;
        while (time < duration)
        {
            pivot.rotation = Quaternion.Slerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        pivot.rotation = endRot;

        // PutDown: reparent back
        cubeState.PutDown(face, pivot.parent);
        readCube.ReadState();
    }
}
