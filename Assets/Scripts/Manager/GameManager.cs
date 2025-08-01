using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // [Header("Player in Game")]
    private GameObject playerOne;
    private GameObject playerTwo;

    [Header("Spawn Positions")]
    [SerializeField] private Transform playerOneSpawn;
    [SerializeField] private Transform playerTwoSpawn;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject playerOnePrefab;
    [SerializeField] private GameObject playerTwoPrefab;

    [Header("Player Joystick")]
    [SerializeField] private SimpleJoystick joystickOne;
    [SerializeField] private SimpleJoystick joystickTwo;

    [Header("Player Skill Button")]
    [SerializeField] private Button playerOneSkillButton;
    [SerializeField] private Button playerTwoSkillButton;
    [SerializeField] private Image playerOneSkillButtonIcon;
    [SerializeField] private Image playerTwoSkillButtonIcon;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject resultWIN1;
    [SerializeField] GameObject resultWIN2;
    [SerializeField] GameObject resultLOSE1;
    [SerializeField] GameObject resultLOSE2;
    [SerializeField] GameObject resultGUI;


    [SerializeField] TextMeshProUGUI timerSuddenDeath;

    [Header("Spike")]
    [SerializeField] private GameObject spikeAbove;
    [SerializeField] private GameObject spikeBelow;
    [SerializeField] private float moveDuration = 1f;

    [Header("Timer")]
    [SerializeField] float remainingTime;

    [Header("Volume")]
    [SerializeField] Volume vol;
    Vignette vignette;

    private PlayerController playerInfo1;
    private PlayerController playerInfo2;
    private bool p1Win;
    private bool p2Win;
    private bool isGameOver = false;

    [Header("Winning Condition")]
    [SerializeField] private TextMeshProUGUI winText1;
    [SerializeField] private TextMeshProUGUI winText2;
    [SerializeField] private TextMeshProUGUI resetText;
    [SerializeField] private TextMeshProUGUI backText;
    [SerializeField] private GameObject resetButton;
    [SerializeField] private GameObject backButton;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // winText1.gameObject.SetActive(false);
        // winText2.gameObject.SetActive(false);
        // resetText.gameObject.SetActive(false);
        // backText.gameObject.SetActive(false);
        // resetButton.SetActive(false);
        // backButton.SetActive(false);
        resultGUI.SetActive(false);
        timerSuddenDeath.gameObject.SetActive(false);
        vol.profile.TryGet(out vignette);
        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"health of p1 : {playerInfo1.GetHealth()}");
        Debug.Log($"health of p2 : {playerInfo2.GetHealth()}");
        // vignette.intensity.value = 1f;
        TimerCounting();
        WinningCondition();
    }

    private void SpawnPlayers()
    {
        if (playerOne != null) Destroy(playerOne);
        if (playerTwo != null) Destroy(playerTwo);

        playerOne = Instantiate(playerOnePrefab, playerOneSpawn.position, Quaternion.identity);
        playerTwo = Instantiate(playerTwoPrefab, playerTwoSpawn.position, Quaternion.identity);

        playerInfo1 = playerOne.GetComponent<PlayerController>();
        playerInfo1._joystick = joystickOne;

        playerInfo2 = playerTwo.GetComponent<PlayerController>();
        playerInfo2._joystick = joystickTwo;

        playerOneSkillButton.onClick.RemoveAllListeners();
        playerTwoSkillButton.onClick.RemoveAllListeners();

        // Add UseSkill button listeners
        if (playerOne.TryGetComponent<PlayerSkillController>(out var skillController1))
        {
            playerOneSkillButton.onClick.AddListener(skillController1.UseSkill);
            skillController1._skillButtonIcon = playerOneSkillButtonIcon;
        }

        if (playerTwo.TryGetComponent<PlayerSkillController>(out var skillController2))
        {
            playerTwoSkillButton.onClick.AddListener(skillController2.UseSkill);
            skillController2._skillButtonIcon = playerTwoSkillButtonIcon;
        }
    }

    private void TimerCounting()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            timerSuddenDeath.gameObject.SetActive(true);
            ExtraTime();
            //masuk ke extra time
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void WinningCondition()
    {
        if (isGameOver) return;

        if (playerInfo1.GetHealth() <= 0 && playerInfo2.GetHealth() > 0)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.musicEndgame);

            p1Win = false;
            p2Win = true;
            isGameOver = true;
            Debug.Log("Player 2 Wins");
            winText1.text = "LOSE";
            winText2.text = "WIN";
            resultGUI.SetActive(true);
            timerText.gameObject.SetActive(false);

            resultLOSE1.SetActive(true);
            resultWIN2.SetActive(true);

            resultLOSE2.SetActive(false);
            resultWIN1.SetActive(false);

            Destroy(playerOne);
            Destroy(playerTwo);
            // winText1.gameObject.SetActive(true);
            // winText2.gameObject.SetActive(true);
            // resetButton.SetActive(true);
            // resetText.gameObject.SetActive(true);
            // backText.gameObject.SetActive(true);
            // resetButton.SetActive(true);
            // backButton.SetActive(true);
        }
        else if (playerInfo2.GetHealth() <= 0 && playerInfo1.GetHealth() > 0)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.musicEndgame);

            p1Win = true;
            p2Win = false;
            isGameOver = true;
            Debug.Log("Player 1 Wins");
            winText1.text = "WIN";
            winText2.text = "LOSE";
            resultGUI.SetActive(true);
            timerText.gameObject.SetActive(false);

            resultLOSE2.SetActive(true);
            resultWIN1.SetActive(true);

            resultLOSE1.SetActive(false);
            resultWIN2.SetActive(false);

            Destroy(playerOne);
            Destroy(playerTwo);

            // winText1.gameObject.SetActive(true);
            // winText2.gameObject.SetActive(true);
            // resetButton.SetActive(true);
            // resetText.gameObject.SetActive(true);
            // backText.gameObject.SetActive(true);
            // resetButton.SetActive(true);
            // backButton.SetActive(true);
        }
        else if (playerInfo1.GetHealth() <= 0 && playerInfo2.GetHealth() <= 0)
        {
            //tetep ada draw condition kah?
            timerText.gameObject.SetActive(false);
            ExtraTime();
        }
    }

    private void ExtraTime()
    {
        StartCoroutine(MoveSpikeOverTime());
    }

    private IEnumerator MoveSpikeOverTime()
    {
        Vector3 startPos = spikeAbove.transform.position;
        Vector3 endPos = new Vector3(startPos.x, 3f, startPos.z);

        Vector3 startPos1 = spikeBelow.transform.position;
        Vector3 endPos1 = new Vector3(startPos1.x, -3f, startPos1.z);

        float elapsed = 0f;
        float countdown = 20f;
        float displayCountdown = countdown;
        float vignetteBase = 0.3f;
        float vignetteMax = 0.6f;

        timerSuddenDeath.gameObject.SetActive(true);

        AudioManager.instance.StartLoopMusic(AudioManager.instance.suddenDeathAlarm);

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / moveDuration);
            spikeAbove.transform.position = Vector3.Lerp(startPos, endPos, t);
            spikeBelow.transform.position = Vector3.Lerp(startPos1, endPos1, t);

            if (countdown > 0 && elapsed >= (10f - countdown))
            {
                displayCountdown = Mathf.FloorToInt(countdown);
                countdown -= 1;
            }

            if (vignette != null)
            {
                float pulse = Mathf.Sin(Time.time * 10f) * 0.2f + 0.6f;
                vignette.intensity.value = Mathf.Clamp(pulse, vignetteBase, vignetteMax);
            }

            float scalePulse = Mathf.Sin(Time.time * 4f) * 0.1f + 1f;
            timerSuddenDeath.rectTransform.localScale = Vector3.one * scalePulse;

            float colorPulse = Mathf.Sin(Time.time * 6f) * 0.5f + 0.5f; // 0 to 1
            // Color pulseColor = Color.Lerp(new Color(1f, 0.2f, 0.2f), new Color(1f, 0f, 0f), colorPulse); // Light red → deep red
            // timerSuddenDeath.color = pulseColor;

            timerSuddenDeath.text = Mathf.CeilToInt(displayCountdown).ToString();

            yield return null;
        }

        // Final state
        spikeAbove.transform.position = endPos;
        spikeBelow.transform.position = endPos1;
        // if (vignette != null) vignette.intensity.value = 0.6f;

        timerSuddenDeath.rectTransform.localScale = Vector3.one;
        timerSuddenDeath.text = "0";

        AudioManager.instance.StopLoopMusic();

        timerSuddenDeath.gameObject.SetActive(false);
    }



    public void RestartGame()
    {
        // winText1.gameObject.SetActive(false);
        // winText2.gameObject.SetActive(false);
        // resetButton.SetActive(false);
        // resetText.gameObject.SetActive(false);
        // backText.gameObject.SetActive(false);
        // resetButton.SetActive(false);
        // backButton.SetActive(false);
        // resultGUI.SetActive(false);
        // p1Win = false;
        // p2Win = false;
        // isGameOver = false;

        // Vector3 resetPosAbove = new Vector3(0, 6f, 0);
        // spikeAbove.transform.position = resetPosAbove;

        // Vector3 resetPosBelow = new Vector3(0, -6f, 0);
        // spikeBelow.transform.position = resetPosBelow;

        // remainingTime = 10f;

        // SpawnPlayers();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}

