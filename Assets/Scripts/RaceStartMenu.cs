using System.Collections;
using UnityEngine;

public class RaceStartMenu : MonoBehaviour
{
    public FollowTracker[] cars = new FollowTracker[3];
    public RaceHUD raceHUD;

    private readonly float[] selectedSpeeds = new float[3];
    private int selectedLaps = 3;
    private string countdown = "";
    private bool started;

    private GUIStyle titleStyle;
    private GUIStyle textStyle;
    private GUIStyle numberStyle;
    private GUIStyle smallButtonStyle;
    private GUIStyle playButtonStyle;
    private GUIStyle countdownStyle;

    private Texture2D panelTexture;
    private Texture2D greenTexture;
    private Texture2D playSymbol;

    private void Awake()
    {
        for (int i = 0; i < cars.Length && i < 3; i++)
        {
            if (cars[i] == null) continue;

            selectedSpeeds[i] = cars[i].speed;

            RaceCarProgress progress = cars[i].GetComponent<RaceCarProgress>();
            if (progress != null)
                selectedLaps = progress.lapsToFinish;

            if (cars[i].tracker != null)
            {
                FollowWP trackerMovement =
                    cars[i].tracker.GetComponent<FollowWP>();

                if (trackerMovement != null)
                    trackerMovement.enabled = false;
            }

            cars[i].enabled = false;
        }

        if (raceHUD != null)
            raceHUD.enabled = false;
    }

    private void OnGUI()
    {
        if (started && countdown == "") return;

        CreateStyles();

        // Mantiene el menú proporcionado aunque cambie el tamaño de Game.
        float scale = Mathf.Min(
            Screen.width / 960f,
            Screen.height / 540f
        );

        Matrix4x4 previousMatrix = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(
            new Vector3(
                (Screen.width - 960f * scale) / 2f,
                (Screen.height - 540f * scale) / 2f,
                0f
            ),
            Quaternion.identity,
            new Vector3(scale, scale, 1f)
        );

        if (started)
            DrawCountdown();
        else
            DrawSetup();

        GUI.matrix = previousMatrix;
    }

    private void DrawSetup()
    {
        // Fondo oscuro y panel central.
        GUI.color = new Color(0f, 0f, 0f, 0.65f);
        GUI.DrawTexture(new Rect(0, 0, 960, 540), panelTexture);
        GUI.color = Color.white;

        GUI.DrawTexture(new Rect(180, 42, 600, 456), panelTexture);

        GUI.Label(new Rect(220, 65, 520, 42),
            "CONFIGURAR CARRERA", titleStyle);
        GUI.Label(new Rect(220, 106, 520, 30),
            "Elige la velocidad y las vueltas antes de salir", textStyle);

        for (int i = 0; i < 3; i++)
        {
            float y = 158 + i * 66;
            string carName = "Auto " + (i + 1);

            if (cars != null && i < cars.Length && cars[i] != null)
            {
                RaceCarProgress progress =
                    cars[i].GetComponent<RaceCarProgress>();

                if (progress != null)
                    carName = progress.racerName;
            }

            GUI.Label(new Rect(225, y, 265, 38),
                carName + "  |  Velocidad", textStyle);

            if (GUI.Button(new Rect(525, y, 45, 38),
                "−", smallButtonStyle))
                selectedSpeeds[i] = Mathf.Max(8f, selectedSpeeds[i] - 1f);

            GUI.Label(new Rect(585, y, 55, 38),
                selectedSpeeds[i].ToString("0"), numberStyle);

            if (GUI.Button(new Rect(650, y, 45, 38),
                "+", smallButtonStyle))
                selectedSpeeds[i] = Mathf.Min(28f, selectedSpeeds[i] + 1f);
        }

        GUI.Label(new Rect(225, 360, 265, 38),
            "Vueltas de la carrera", textStyle);

        if (GUI.Button(new Rect(525, 360, 45, 38),
            "−", smallButtonStyle))
            selectedLaps = Mathf.Max(1, selectedLaps - 1);

        GUI.Label(new Rect(585, 360, 55, 38),
            selectedLaps.ToString(), numberStyle);

        if (GUI.Button(new Rect(650, 360, 45, 38),
            "+", smallButtonStyle))
            selectedLaps = Mathf.Min(10, selectedLaps + 1);

        bool ready = cars != null && cars.Length >= 3
            && cars[0] != null && cars[1] != null && cars[2] != null;

        GUI.enabled = ready;

        if (GUI.Button(new Rect(350, 425, 260, 55),
            "", playButtonStyle))
            StartCoroutine(StartRace());

        GUI.enabled = true;

        // Símbolo ▶ dibujado por el script, sin importar imágenes.
        GUI.DrawTexture(new Rect(395, 438, 30, 30), playSymbol);
        GUI.Label(new Rect(433, 433, 135, 40),
            "PLAY", titleStyle);

        if (!ready)
            GUI.Label(new Rect(220, 398, 520, 24),
                "Asigna los tres autos en el Inspector.", textStyle);
    }

    private void DrawCountdown()
    {
        GUI.color = new Color(0f, 0f, 0f, 0.55f);
        GUI.DrawTexture(new Rect(0, 0, 960, 540), panelTexture);
        GUI.color = Color.white;

        GUI.Label(new Rect(0, 175, 960, 190),
            countdown, countdownStyle);
    }

    private IEnumerator StartRace()
    {
        started = true;

        for (int i = 0; i < 3; i++)
        {
            FollowTracker car = cars[i];
            car.speed = selectedSpeeds[i];

            RaceCarProgress progress =
                car.GetComponent<RaceCarProgress>();

            if (progress != null)
                progress.lapsToFinish = selectedLaps;

            if (car.tracker != null)
            {
                FollowWP trackerMovement =
                    car.tracker.GetComponent<FollowWP>();

                if (trackerMovement != null)
                    trackerMovement.speed = selectedSpeeds[i] * 0.85f;
            }
        }

        for (int number = 3; number >= 1; number--)
        {
            countdown = number.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        countdown = "¡GO!";

        for (int i = 0; i < 3; i++)
        {
            FollowTracker car = cars[i];

            if (car.tracker != null)
            {
                FollowWP trackerMovement =
                    car.tracker.GetComponent<FollowWP>();

                if (trackerMovement != null)
                    trackerMovement.enabled = true;
            }

            car.enabled = true;
        }

        if (raceHUD != null)
            raceHUD.enabled = true;

        yield return new WaitForSecondsRealtime(0.8f);
        countdown = "";
    }

    private void CreateStyles()
    {
        if (titleStyle != null) return;

        panelTexture = SolidTexture(new Color(0.10f, 0.13f, 0.16f));
        greenTexture = SolidTexture(new Color(0.23f, 0.70f, 0.23f));
        playSymbol = CreatePlaySymbol();

        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 29;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.white;

        textStyle = new GUIStyle(GUI.skin.label);
        textStyle.fontSize = 19;
        textStyle.normal.textColor = Color.white;
        textStyle.alignment = TextAnchor.MiddleLeft;

        numberStyle = new GUIStyle(textStyle);
        numberStyle.fontSize = 24;
        numberStyle.fontStyle = FontStyle.Bold;
        numberStyle.alignment = TextAnchor.MiddleCenter;

        smallButtonStyle = new GUIStyle(GUI.skin.button);
        smallButtonStyle.fontSize = 25;
        smallButtonStyle.fontStyle = FontStyle.Bold;

        playButtonStyle = new GUIStyle(GUI.skin.button);
        playButtonStyle.normal.background = greenTexture;
        playButtonStyle.hover.background = greenTexture;
        playButtonStyle.active.background = greenTexture;

        countdownStyle = new GUIStyle(titleStyle);
        countdownStyle.fontSize = 110;
        countdownStyle.alignment = TextAnchor.MiddleCenter;
    }

    private Texture2D SolidTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private Texture2D CreatePlaySymbol()
    {
        Texture2D texture = new Texture2D(32, 32);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                int tip = 26 - Mathf.Abs(y - 16);

                texture.SetPixel(x, y,
                    x >= 5 && x <= tip
                        ? Color.white
                        : Color.clear);
            }
        }

        texture.Apply();
        return texture;
    }
}