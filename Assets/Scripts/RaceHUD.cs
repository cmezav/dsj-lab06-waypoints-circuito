using System;
using UnityEngine;

public class RaceHUD : MonoBehaviour
{
    public RaceCarProgress[] cars;

    private GUIStyle rowStyle;
    private GUIStyle titleStyle;

    private void OnGUI()
    {
        if (cars == null || cars.Length == 0) return;

        if (rowStyle == null)
        {
            rowStyle = new GUIStyle(GUI.skin.label);
            rowStyle.fontSize = 17;
            rowStyle.normal.textColor = Color.white;

            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = 20;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = Color.white;
        }

        RaceCarProgress[] ordered = (RaceCarProgress[])cars.Clone();

        Array.Sort(ordered, (a, b) =>
        {
            if (a == null) return 1;
            if (b == null) return -1;

            if (a.Finished && b.Finished)
                return a.FinishPosition.CompareTo(b.FinishPosition);

            if (a.Finished) return -1;
            if (b.Finished) return 1;

            return b.CompletedLaps.CompareTo(a.CompletedLaps);
        });

        float width = 390f;
        float height = 65f + ordered.Length * 37f;

        GUI.Box(new Rect(15, 15, width, height), "");
        GUI.Label(new Rect(30, 25, width - 30, 30),
            "RESULTADOS DE LA CARRERA", titleStyle);

        for (int i = 0; i < ordered.Length; i++)
        {
            RaceCarProgress car = ordered[i];
            if (car == null) continue;

            string status = car.Finished
                ? $"{car.FinishPosition}° LUGAR"
                : $"Vuelta {car.CompletedLaps}/{car.lapsToFinish}";

            GUI.Label(
                new Rect(30, 65 + i * 37, width - 30, 30),
                $"{car.racerName,-15}  {status}",
                rowStyle
            );
        }

        foreach (RaceCarProgress car in ordered)
        {
            if (car != null && car.FinishPosition == 1)
            {
                GUI.Box(
                    new Rect(15, 25 + height, width, 48),
                    $"GANADOR: {car.racerName}"
                );
                break;
            }
        }
    }
}