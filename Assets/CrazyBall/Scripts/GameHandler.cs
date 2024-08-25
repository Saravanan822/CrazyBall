using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public List<Image> pads;
    public List<Image> currentPads;
    public float randomTimer = 0;
    public float currentTimer = 0;
    float padMover = 200;
    bool firstPadIns;
    bool secondPadIns;
    public BallScript ball;
    public bool GameEnd;
    public float score = 0;
    public int bestScore;
    public float scoreMultiplier = 0.05f;
    public static GameHandler instance;
    public Text currScoreText;
    public Text bestScoreText;
    public string bestScorePlayerPref = "BestScore";
    public string totalCoinsPlayerPref = "TotalCoins";

    public float padInstantiateTimerMin = 1;
    public float padInstantiateTimerMax = 3;

    // special powers

    public Image coin;
    public Image FlyingBall;
    public Image SpecialPad;
    public float specialPowerTimer;
    public bool specialPowerShowing;

    public int coinsCollected;
    public Text coinsText;
    public int coinNeededForRevive = 1;

    public int flyingBallCollected;
    public Text flyingBallCountText;
    public GameObject flyingBallIns;
    public float flyTimer = 0;

    public int specialPadCollected;
    public Text specialPadCountText;
    public GameObject specialPadIns;

    // PadImages

    public Sprite greenPad;
    public Sprite bluePad;
    public Sprite redPad;
    public Sprite orangePad;
    public Sprite blackPad;

    // GameEndUi

    public GameObject GameEndPanel;
    public Text GameReviveCoinsText;

    //Audio


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentPads = new List<Image>();
        bestScore = PlayerPrefs.GetInt(bestScorePlayerPref, 0);
        coinsCollected = PlayerPrefs.GetInt(totalCoinsPlayerPref, 0);
        bestScoreText.text = bestScore.ToString();
        coinsText.text = coinsCollected.ToString();
    }

    void Update()
    {
        if (GameEnd && !GameEndPanel.activeSelf)
        {
            PlayerPrefs.SetInt(bestScorePlayerPref, bestScore);
            PlayerPrefs.SetInt(totalCoinsPlayerPref, coinsCollected);
            GameEndPanel.SetActive(true);
            if(coinNeededForRevive <= coinsCollected)
            {
                GameReviveCoinsText.text = coinNeededForRevive.ToString();
                GameEndPanel.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                GameEndPanel.transform.GetChild(0).gameObject.SetActive(true);
            }
            return;
        }
        if (GameEnd) return;

        SetDifficultyLevel(((int)score));

        HandleScores();

        if (currentTimer >= randomTimer)
        {
            HandlePadInitilize();
            currentTimer = 0;
            randomTimer = Random.Range(padInstantiateTimerMin, padInstantiateTimerMax);
        }
        currentTimer += Time.deltaTime;
        specialPowerTimer += Time.deltaTime;

        if (!specialPowerShowing && specialPowerTimer > 5f)
        {
            specialPowerTimer = 0;
            specialPowerShowing = true;
            GetRandomSpecialPower();
        }

        HandlePadMovements();

        if (ball.isFlyingBall)
        {
            flyTimer -= Time.deltaTime;
            if (flyTimer <= 0)
            {
                DisableFlyBall();
            }
        }

        
    }
    
    void HandleScores()
    {
        score += scoreMultiplier;
        currScoreText.text = ((int)score).ToString();

        if (score >= bestScore)
        {

            bestScore = ((int)score);
            bestScoreText.text = bestScore.ToString();
        }
    }
    void HandlePadMovements()
    {
        for (int i = currentPads.Count - 1; i >= 0; i--)
        {
            {
                currentPads[i].transform.position += Vector3.up * Time.deltaTime * padMover;
                if (currentPads[i].transform.position.y > Screen.height * 1.1f)
                {
                    currentPads[i].gameObject.SetActive(false);
                    currentPads.RemoveAt(i);
                }
            }
        }
    }
    void HandlePadInitilize()
    {
        var currPad = GetPad();
        currPad.transform.position = RandomPositionGenerator();
        currentPads.Add(currPad);

        if (!firstPadIns)
        {
            firstPadIns = true;
            ball.transform.position = new Vector3(currPad.transform.position.x, ball.transform.position.y, 0);
            ball.gameObject.SetActive(true);
            currPad.transform.GetComponent<Pad>().type = Pad.PadType.Bouncy;
            currPad.sprite = bluePad;

        }
        else if (!secondPadIns)
        {
            secondPadIns = true;
           
            currPad.transform.GetComponent<Pad>().type = Pad.PadType.Normal;
            currPad.sprite = greenPad;

        }
        else
        {
            currPad.transform.GetComponent<Pad>().type = GetPadType(currPad);
        }
    }
    Vector3 RandomPositionGenerator()
    {
        float randPosX = Random.Range(Screen.width * 0.1f, Screen.width * 0.9f);

        return new Vector3(randPosX, ball.bottomBase.transform.position.y, 0);
    }
    Vector3 RandomPositionGeneratorForSpecialPowers()
    {
        float randPosX = Random.Range(Screen.width * 0.1f, Screen.width * 0.9f);
        float randPosY = Random.Range(Screen.height * 0.5f, Screen.height * 0.9f);

        return new Vector3(randPosX, randPosY, 0);
    }

    Image GetPad()
    {
        foreach (var pad in pads)
        {
            if (!pad.gameObject.activeSelf && !currentPads.Contains(pad))
            {
                pad.gameObject.SetActive(true);
                return pad;
            }
        }
        return pads[0];
    }
    Pad.PadType GetPadType(Image image) {

        int randomVal = Random.Range(0, 101);

        if (randomVal % 7 == 0)
        {
            image.sprite = redPad;
            return Pad.PadType.Avoidable;
        }
        else if (randomVal % 5 == 0)
        {
            image.sprite = orangePad;
            return Pad.PadType.Blinks;
        }
        else if (randomVal % 3 == 0)
        {
            image.sprite = bluePad;
            return Pad.PadType.Bouncy;
        }
        else
        {
            image.sprite = greenPad;
            return Pad.PadType.Normal;
        }

    }
    void SetDifficultyLevel(int score)
    {
        if(score < 250)
        {
            padInstantiateTimerMin = 1.5f;
            padInstantiateTimerMax = 2.5f;
            padMover = 200;
            scoreMultiplier = 0.1f;
        }
        else if (score < 500)
        {
            padInstantiateTimerMin = 1.3f;
            padInstantiateTimerMax = 2.3f;
            padMover = 250;
            scoreMultiplier = 0.15f;
        }
        else if(score < 750)
        {
            padInstantiateTimerMin = 1.1f;
            padInstantiateTimerMax = 2.1f;
            padMover = 300;
            scoreMultiplier = 0.2f;
        }
        else if(score < 1000)
        {
            padInstantiateTimerMin = 0.9f;
            padInstantiateTimerMax = 1.7f;
            padMover = 350;
            scoreMultiplier = 0.25f;
        }
        else if(score < 1500)
        {
            padInstantiateTimerMin = 0.7f;
            padInstantiateTimerMax = 1.5f;
            padMover = 400;
            scoreMultiplier = 0.3f;
        }
        else if (score < 3000)
        {
            padInstantiateTimerMin = 0.5f;
            padInstantiateTimerMax = 1.3f;
            padMover = 450;
            scoreMultiplier = 0.35f;
        }
        else if (score < 5000)
        {
            padInstantiateTimerMin = 0.3f;
            padInstantiateTimerMax = 1f;
            padMover = 500;
            scoreMultiplier = 0.4f;
        }
    }
    void GetRandomSpecialPower()
    {
        int randomVal = Random.Range(1, 22);

        if (randomVal % 5 == 0)
        {
            FlyingBall.transform.position = RandomPositionGeneratorForSpecialPowers();
            FlyingBall.gameObject.SetActive(true);
        }
        else if (randomVal % 3 == 0)
        {
            SpecialPad.transform.position = RandomPositionGeneratorForSpecialPowers();
            SpecialPad.gameObject.SetActive(true);
        }
        else
        {
            coin.transform.position = RandomPositionGeneratorForSpecialPowers();
            coin.gameObject.SetActive(true);
        }
    }
    public void HandleSpecialPads()
    {
        if (specialPadCollected == 0) return;
        specialPadIns.SetActive(true);
    }
    public void HandleFlyingBall()
    {
        if (flyingBallCollected == 0) return;
        flyingBallIns.SetActive(true);
    }
    public void UseSpecialPads(Vector3 pos)
    {
        specialPadCollected--;
        specialPadCountText.text = specialPadCollected.ToString();

        var currPad = GetPad();
        currPad.transform.position = pos;
        currentPads.Add(currPad);
        
        currPad.transform.GetComponent<Pad>().type = Pad.PadType.Normal;
        currPad.sprite = blackPad;
       
    }
    public void UseFlyingBall()
    {
        flyingBallCollected--;
        flyingBallCountText.text = flyingBallCollected.ToString();

        if (ball.isFlyingBall) flyTimer += 5;
        else flyTimer = 5;

        ball.rb.gravityScale = 0;
        ball.rb.velocity = Vector3.zero;
        ball.isFlyingBall = true;
        ball.transform.GetComponent<Image>().enabled = true;
        ball.animator.SetBool("Fly", true);

    }
    public void DisableFlyBall()
    {
        ball.rb.gravityScale = 75;
        ball.isFlyingBall = false;
        ball.transform.GetComponent<Image>().enabled = false;
        ball.animator.SetBool("Fly",false);
    }
    public void SwitchToPosterScene()
    {
        Invoke("WaitAndLoad", 0.5f);
       
    }
    public void WaitAndLoad()
    {
        SceneManager.LoadScene(0);
    }
    public void ReviveGameUsingCoin()
    {
        coinsCollected -= coinNeededForRevive;
        coinsText.text = coinsCollected.ToString();

        coinNeededForRevive = (2 * coinNeededForRevive);

        GameEnd = false;

        GameEndPanel.transform.GetChild(1).gameObject.SetActive(false);
        GameEndPanel.SetActive(false);

        ball.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.9f, 0);
        ball.rb.bodyType = RigidbodyType2D.Dynamic;
    }

}
