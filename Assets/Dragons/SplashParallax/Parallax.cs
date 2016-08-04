using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class Parallax : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{


    public float speed;
    private float dragonsSpeed;

    [Space(10)]
    public Image fader;
    public float fadeSpeed;

    [Space(10)]
    public RectTransform root;
    public RectTransform background;

    [Space(10)]
    public RectTransform yellowDragon;
    public RectTransform blueDragon;
    public RectTransform redDragon;

    Vector2 A2blue;
    Vector2 A2red;

    private void Awake()
    {
        dragonsSpeed = speed * 0.17f;

        Vector2 yellowSizeDelta = yellowDragon.sizeDelta.Divide(background.sizeDelta);
        Vector2 yellowPosDelta = yellowDragon.anchoredPosition.Divide(background.sizeDelta);

        Vector2 blueSizeDelta = blueDragon.sizeDelta.Divide(background.sizeDelta);
        Vector2 bluePosDelta = blueDragon.anchoredPosition.Divide(background.sizeDelta);

        Vector2 redSizeDelta = redDragon.sizeDelta.Divide(background.sizeDelta);
        Vector2 redPosDelta = redDragon.anchoredPosition.Divide(background.sizeDelta);


        background.sizeDelta = new Vector2(Screen.height * CalculateFactor(background), Screen.height);

        

        root.position = new Vector2(0.0F, Screen.height / 2.0F);
        background.position = new Vector2(0.0F, Screen.height / 2.0F);

        yellowDragon.sizeDelta = background.sizeDelta.Multiply(yellowSizeDelta);
        yellowDragon.anchoredPosition = background.sizeDelta.Multiply(yellowPosDelta);

        blueDragon.sizeDelta = background.sizeDelta.Multiply(blueSizeDelta);
        //blueDragon.anchoredPosition = background.sizeDelta.Multiply(bluePosDelta);
        A2blue = background.sizeDelta.Multiply(bluePosDelta);

        float C2blue = A2blue.x - Screen.width * 0.5F;
        float Tblue= C2blue / speed;
        Vector2 A1blue = new Vector2(dragonsSpeed * Tblue + A2blue.x, A2blue.y);
        //A1blue.x *= 1.1F;

        blueDragon.anchoredPosition = A1blue;



        redDragon.sizeDelta = background.sizeDelta.Multiply(redSizeDelta);
        //redDragon.anchoredPosition = background.sizeDelta.Multiply(redPosDelta);
        A2red = background.sizeDelta.Multiply(redPosDelta);

        float C2red = A2red.x - root.sizeDelta.x * 0.5F;
        float Tred = C2red / speed;
        Vector2 A1red = new Vector2(dragonsSpeed * Tred + A2red.x, A2red.y);
        //A1red.x *= 1.2F;

        redDragon.anchoredPosition = A1red;

    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }


    private IEnumerator FadeIn()
    {
        while (fader.color.a > 0.1F)
        {
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, fader.color.a - fadeSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartCoroutine(MoveBackground());
    }

    private IEnumerator MoveBackground()
    {
        while (background.position.x > -(background.sizeDelta.x - Screen.width))
        {
            background.transform.position = Vector2.MoveTowards(background.transform.position, background.transform.position + background.transform.right * -2.0F, speed * Time.deltaTime);
            
            blueDragon.anchoredPosition = Vector2.MoveTowards(blueDragon.anchoredPosition, blueDragon.anchoredPosition + new Vector2(-blueDragon.right.x, 0), dragonsSpeed * Time.deltaTime);
            redDragon.anchoredPosition = Vector2.MoveTowards(redDragon.anchoredPosition, redDragon.anchoredPosition + new Vector2(-redDragon.right.x, 0), dragonsSpeed * Time.deltaTime);
            yellowDragon.anchoredPosition = Vector2.MoveTowards(yellowDragon.anchoredPosition, yellowDragon.anchoredPosition + new Vector2(-yellowDragon.right.x, 0), dragonsSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (fader.color.a < 1.0F)
        {
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, fader.color.a + fadeSpeed * 2);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        root.gameObject.SetActive(false);
        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        while (fader.color.a > 0.1F)
        {
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, fader.color.a - fadeSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        root.parent.gameObject.SetActive(false);
    }


    private float CalculateFactor(RectTransform transform)
    {
        Sprite sprite = transform.GetComponent<Image>().sprite;
        return sprite.rect.size.x / sprite.rect.size.y;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (block) return;

        block = true;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private bool block = false;
}

public static class Extentions
{
    public static Vector2 Divide(this Vector2 a, Vector2 b)
    {
        return new Vector2(a.x / b.x, a.y / b.y);
    }

    public static Vector2 Multiply(this Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }
}
