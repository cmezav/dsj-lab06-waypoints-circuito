using UnityEngine;

public class RaceHUD : MonoBehaviour
{
    public RaceCarProgress[] cars;

    private GUIStyle textStyle;

    private void OnGUI()
    {
        if (cars == null || cars.Length == 0) return;

        if (textStyle == null)
        {
            textStyle = new GUIStyle(GUI.skin.label);
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.white;
        }

        GUI.Box(new Rect(12, 12, 340, 60 + cars.Length * 27), "RESULTADOS DE LA CARRERA");

        for (int i = 0; i < cars.Length; i++)
        {
            RaceCarProgress car = cars[i];
            if (car == null) continue;

            string position = car.Finished ? car.FinishPosition + "°" : "En carrera";
            string row = $"{car.racerName}: {car.CompletedLaps}/{car.lapsToFinish} vueltas  |  {position}";
            GUI.Label(new Rect(25, 42 + i * 27, 320, 25), row, textStyle);

            if (car.FinishPosition == 1)
            {
                GUI.Box(new Rect(12, 90 + cars.Length * 27, 340, 45),
                    $"🏆 GANADOR: {car.racerName}");
            }
        }
    }
}