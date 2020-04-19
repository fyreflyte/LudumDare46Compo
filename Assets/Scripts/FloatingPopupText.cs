////////////////////////////////////////////////////////////////////////////////////////
//        Copyright Jason Woerner 2018                                                //
////////////////////////////////////////////////////////////////////////////////////////
//
// Generic script that animates "floating" text
// Typically used to denote damage or completion of tasks
//
// This must be added to a prefab that has a Text or TextMeshPro component

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum FloatingFadeMethod { Alpha, Drop, Shrink, AlphaAndDrop, None };

public class FloatingPopupText : MonoBehaviour
{
    public Text floatingText;
    public TextMeshProUGUI proFloatingText;
    private int BASE_SPEED = 200;
    private int maxPlane = 0;  // Sets a maximum plane to which the text will ascend (useful when using a random speed)
    private float mySpeed;
    private float initYPosition;
    private float mainCanvasYScale;
    private Canvas mainCanvas;  // Note that if you are using multiple canvases, this will need to set explicitly
    private CanvasGroup myCanvasGroup;
    bool started = false;

    float timeCreated;
    float fadeStartTime;
    float fadeTotalTime;
    public Color startingColor;
    Color fadedColor;
    bool hasFade = false;
    FloatingFadeMethod fadeType;

    public Image prefixImage;
    public Image suffixImage;

    // Use this for initialization
    void Start()
    {
        mainCanvas = (Canvas)FindObjectOfType(typeof(Canvas));
        mainCanvasYScale = mainCanvas.transform.localScale.y;
        myCanvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the text and apply any fade-out effects
        if (started)
        {
            transform.Translate(Vector3.up * Time.deltaTime * mySpeed * mainCanvasYScale);
            if (hasFade && (Time.time - timeCreated) > fadeStartTime)
                UpdateFadeOut();
        }
        if (maxPlane > 0 && transform.position.y - initYPosition > maxPlane * mainCanvasYScale)
            Destroy(gameObject);
    }

    public void StartFloating(string displayedText, float speedMult = 1, float randomXRange = 0, float randomSpeedMult = 1, float textTime = 2, float maxPlaneDistance = 0)
    {
        timeCreated = Time.time;
        mySpeed = BASE_SPEED * speedMult;
        float locationShift = 0;
        if (floatingText != null)
            floatingText.text = displayedText;
        if (proFloatingText != null)
            proFloatingText.text = displayedText;
        initYPosition = transform.position.y;
        if (randomXRange > 0)
            locationShift = Random.Range(0, randomXRange);

        if (randomSpeedMult > 1)
            mySpeed *= Random.Range(1, randomSpeedMult);

        started = true;
        //Debug.Log("STARTING COLOR " + startColor);
        //float ttWidth = (gameObject.GetComponents<RectTransform>()[0].rect.width * mainCanvas.transform.localScale.x) / 2;
        //int sWidth = (int)(mainCanvas.GetComponent<RectTransform>().rect.width * mainCanvas.transform.localScale.x);
        //if (transform.position.x >= sWidth - ttWidth) // We use width * 2 because the pivot point is at the lower left of the image, not in the center
        //    transform.position = new Vector2(sWidth - ttWidth, transform.position.y);

        // TODO: Add support for a pool system
        Destroy(gameObject, textTime);
    }

    public void SelectFadeMethod(FloatingFadeMethod fadeMethod, float fadeTime = 2f, float fadeStart = 1f)
    {
        //Debug.Log("Fade method selected");
        hasFade = true;
        fadeType = fadeMethod;
        fadeStartTime = fadeStart;
        fadeTotalTime = fadeTime;
        // Alpha fade
        // **Currently alpha fades by default**
        //startingColor = proFloatingText.color;
        //fadedColor = startingColor;
        //fadedColor.a = 0;
    }

    // TODO: Code different fade types
    private void UpdateFadeOut()
    {
        // Color fade % = (current time - time started) / total time
        float colorFadePercent = (Time.time - (timeCreated + fadeStartTime)) / (fadeTotalTime - fadeStartTime);
        //Debug.Log(colorFadePercent);
        myCanvasGroup.alpha = 1f - colorFadePercent;
        // Old method
        //proFloatingText.color = Color.Lerp(startingColor, fadedColor, colorFadePercent);
    }
}
