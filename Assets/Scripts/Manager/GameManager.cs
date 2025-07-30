using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("UI")]
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Spike")]
    [SerializeField] private GameObject spikeAbove;
    [SerializeField] private GameObject spikeBelow;
    [SerializeField] private float moveDuration = 1f;

    [Header("Timer")]
    [SerializeField] float remainingTime;

    private PlayerController playerInfo1;
    private PlayerController playerInfo2;
    private bool p1Win;
    private bool p2Win;
    private bool isGameOver = false;

    [Header("Winning Condition")]
    [SerializeField] private TextMeshProUGUI winText1;
    [SerializeField] private TextMeshProUGUI winText2;
    [SerializeField] private GameObject resetButton;



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
        winText1.gameObject.SetActive(false);
        winText2.gameObject.SetActive(false);
        resetButton.SetActive(false);
        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"health of p1 : {playerInfo1.getHealth()}");
        Debug.Log($"health of p2 : {playerInfo2.getHealth()}");
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

        if (playerInfo1.getHealth() <= 0 && playerInfo2.getHealth() > 0)
        {
            p1Win = false;
            p2Win = true;
            isGameOver = true;
            Debug.Log("Player 2 Wins");
            winText1.text = "LOSE";
            winText2.text = "WIN";
            winText1.gameObject.SetActive(true);
            winText2.gameObject.SetActive(true);
            resetButton.SetActive(true);
        }
        else if (playerInfo2.getHealth() <= 0 && playerInfo1.getHealth() > 0)
        {
            p1Win = true;
            p2Win = false;
            isGameOver = true;
            Debug.Log("Player 1 Wins");
            winText1.text = "WIN";
            winText2.text = "LOSE";
            winText1.gameObject.SetActive(true);
            winText2.gameObject.SetActive(true);
            resetButton.SetActive(true);
        }
        else if (playerInfo1.getHealth() <= 0 && playerInfo2.getHealth() <= 0)
        {
            //tetep ada draw condition kah?
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
        Vector3 endPos = new Vector3(startPos.x, 2.5f, startPos.z);

        Vector3 startPos1 = spikeBelow.transform.position;
        Vector3 endPos1 = new Vector3(startPos.x, -2.5f, startPos.z);

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            spikeAbove.transform.position = Vector3.Lerp(startPos, endPos, t);

            float t1 = Mathf.Clamp01(elapsed / moveDuration);
            spikeBelow.transform.position = Vector3.Lerp(startPos1, endPos1, t1);

            yield return null;
        }

        spikeAbove.transform.position = endPos;
    }

    public void RestartGame()
    {
        winText1.gameObject.SetActive(false);
        winText2.gameObject.SetActive(false);
        resetButton.SetActive(false);
        p1Win = false;
        p2Win = false;
        isGameOver = false;

        Vector3 resetPosAbove = new Vector3(0, 6f, 0);
        spikeAbove.transform.position = resetPosAbove;

        Vector3 resetPosBelow = new Vector3(0, -6f, 0);
        spikeBelow.transform.position = resetPosBelow;

        remainingTime = 10f;

        SpawnPlayers();
    }
}

