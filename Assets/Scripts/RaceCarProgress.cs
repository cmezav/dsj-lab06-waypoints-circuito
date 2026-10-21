using UnityEngine;

public class RaceCarProgress : MonoBehaviour
{
    public string racerName = "Auto";
    public int lapsToFinish = 3;
    public Collider middleGate;
    public Collider finishGate;

    public int CompletedLaps { get; private set; }
    public int FinishPosition { get; private set; }
    public bool Finished { get; private set; }

    private bool passedMiddle;
    private static int finishOrder;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetFinishOrder() => finishOrder = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (Finished) return;

        if (other == middleGate)
        {
            passedMiddle = true;
        }
        else if (other == finishGate && passedMiddle)
        {
            passedMiddle = false;
            CompletedLaps++;
            Debug.Log($"{racerName}: vuelta {CompletedLaps}/{lapsToFinish}");

            if (CompletedLaps >= lapsToFinish)
            {
                Finished = true;
                finishOrder++;
                FinishPosition = finishOrder;
                Debug.Log($"POSICIÓN {finishOrder}: {racerName} terminó la carrera");
            }
        }
    }
}