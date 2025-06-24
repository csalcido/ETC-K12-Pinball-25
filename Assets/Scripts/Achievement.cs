using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Achievement : MonoBehaviour
{
    private bool isDisplayingAchievement = false;
    public Achievement achievement;
    private int currrentTheme = -1;

    // sound
    public SoundController clapSound;

    //Achievement 1: Magic Eight Balls
    public int pinballTargetCount = 8;
    private bool achievement1Unlocked = false;
    public GameObject magicEightBalls;
    public GameObject magicEightBalls_Done;

    //2: Bumper Sticker
    public int bumperTargetCount = 20;
    private int bumperHitCount = 0;
    private bool achievement2Unlocked = false;
    public GameObject bumperSticker;
    public GameObject bumperSticker_Done;

    //3: Mind the Glass
    public int isSmashed = 0;
    private bool achievement3Unlocked = false;
    public GameObject mindTheGlass;
    public GameObject mindTheGlass_Done;

    //4: On the Right Track
    public int isTunnel = 0;
    private bool achievement4Unlocked = false;
    public GameObject onTheRightTrack;
    public GameObject onTheRightTrack_Done;

    //5: Back on Track
    private bool achievement5Unlocked = false;
    public GameObject backOnTrack;
    public GameObject backOnTrack_Done;

    //6: Target Practice
    public bool isTarget = false;
    private bool achievement6Unlocked = false;
    public GameObject targetPractice;
    public GameObject targetPractice_Done;

    //7: Feed Snake
    public bool isGobble = false;
    private bool achievement7Unlocked = false;
    public GameObject feedSnake;
    public GameObject feedSnake_Done;

    //8: Wrecking Ball
    private bool achievement8Unlocked = false;
    public GameObject wreckingBall;
    public GameObject wreckingBall_Done;

    //9: Chameleon
    public int isColorChanged = 0;
    private bool achievement9Unlocked = false;
    public GameObject chameleon;
    public GameObject chameleon_Done;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currrentTheme = achievement.currrentTheme;
        if (isDisplayingAchievement) return;

        if (!achievement1Unlocked)
        {
            MagicEightBalls();
        }

        if (!achievement2Unlocked)
        {
            BumperSticker();
        }

        if (!achievement3Unlocked)
        {
            MindTheGlass();
        }

        if (!achievement4Unlocked)
        {
            OnTheRightTrack();
        }

        if (!achievement5Unlocked)
        {
            BackOnTrack();
        }

        if (!achievement6Unlocked)
        {
            TargetPractice();
        }

        if (!achievement7Unlocked && (currrentTheme == -1 || currrentTheme == 1))
        {
            FeedSnake();
        }

        if (!achievement8Unlocked)
        {
            WreckingBall();
        }

        if (!achievement9Unlocked)
        {
            Chameleon();
        }

    }

    void MagicEightBalls() {
        GameObject[] pinballs = GameObject.FindGameObjectsWithTag("Ball");
        int currentCount = Mathf.Max(0, pinballs.Length - 2);

        if (currentCount >= pinballTargetCount)
        {
            achievement1Unlocked = true;
            isDisplayingAchievement = true;

            magicEightBalls.SetActive(true);
            magicEightBalls_Done.SetActive(true);
            var cg = magicEightBalls.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(magicEightBalls));
        }
    }

    void BumperSticker() {
        if (bumperHitCount >= bumperTargetCount)
        {
            achievement2Unlocked = true;
            isDisplayingAchievement = true;

            bumperSticker.SetActive(true);
            bumperSticker_Done.SetActive(true);
            var cg = bumperSticker.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(bumperSticker));
        }
    }

    public void RegisterBumperHit()
    {
        if (!achievement2Unlocked)
        {
            bumperHitCount++;
        }
    }

    void MindTheGlass()
    {
        if (isSmashed == 1)
        {
            achievement3Unlocked = true;
            isDisplayingAchievement = true;

            mindTheGlass.SetActive(true);
            mindTheGlass_Done.SetActive(true);
            var cg = mindTheGlass.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(mindTheGlass));
        }
    }

    public void RegisterSmash()
    {
        if (!achievement8Unlocked)
        {
            isSmashed++;
        }
    }

    void OnTheRightTrack() {
        if (isTunnel == 1) {
            achievement4Unlocked = true;
            isDisplayingAchievement = true;

            onTheRightTrack.SetActive(true);
            onTheRightTrack_Done.SetActive(true);
            var cg = onTheRightTrack.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(onTheRightTrack));
        }
    }

    void BackOnTrack() {
        if (isTunnel >= 10)
        {
            achievement5Unlocked = true;
            isDisplayingAchievement = true;

            backOnTrack.SetActive(true);
            backOnTrack_Done.SetActive(true);
            var cg = backOnTrack.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(backOnTrack));
        }
    }
    public void RegisterTunnel() {
        if (!achievement5Unlocked) {
            isTunnel++;
        }
    }

    void TargetPractice() {
        if (isTarget == true) {
            achievement6Unlocked = true;
            isDisplayingAchievement = true;

            targetPractice.SetActive(true);
            targetPractice_Done.SetActive(true);
            var cg = targetPractice.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(targetPractice));
        }
    }

    public void regiserTarget() {
        if (!achievement6Unlocked)
        {
            isTarget = true;
        }
    }

    void FeedSnake() {
        if (isGobble == true)
        {
            achievement7Unlocked = true;
            isDisplayingAchievement = true;

            feedSnake.SetActive(true);
            feedSnake_Done.SetActive(true);
            var cg = feedSnake.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(feedSnake));
        }
    }

    public void registerGobble() {
        if (!achievement7Unlocked)
        {
            isGobble = true;
        }
    }

    void WreckingBall()
    {
        if (isSmashed >= 10)
        {
            achievement8Unlocked = true;
            isDisplayingAchievement = true;

            wreckingBall.SetActive(true);
            wreckingBall_Done.SetActive(true);
            var cg = wreckingBall.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(wreckingBall));
        }
    }

    void Chameleon() {
        if (isColorChanged >= 10)
        {
            achievement9Unlocked = true;
            isDisplayingAchievement = true;

            chameleon.SetActive(true);
            chameleon_Done.SetActive(true);
            var cg = chameleon.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
            StartCoroutine(FadeOutAndDisable(chameleon));
        }
    }

    public void registerColorChanged() {
        if (!achievement9Unlocked)
        {
            isColorChanged++;
        }
    }

    private IEnumerator FadeOutAndDisable(GameObject uiObject, float delay = 3f, float fadeDuration = 1f)
    {
        clapSound.PlaySound();

        yield return new WaitForSeconds(delay);

        CanvasGroup cg = uiObject.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = uiObject.AddComponent<CanvasGroup>();
        }

        float elapsed = 0f;
        float startAlpha = cg.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
        uiObject.SetActive(false);

        isDisplayingAchievement = false;
    }
}
