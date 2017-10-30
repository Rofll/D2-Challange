using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GoogleMobileAds.Api;
using UnityEngine.SocialPlatforms;

public class From : MonoBehaviour
{
    public From2List[] from2list;
    public From3List[] from3list;
    public From4List[] from4list;

    public Heroes[] heroes;

    public SpriteRenderer Main;
    public SpriteRenderer MainRamka;
    public GameObject Panel_1;
    public GameObject Panel_2;
    public GameObject Panel_3;
    public GameObject HeroPanel_1;
    public GameObject HeroPanel_2;
    public GameObject HeroPanel_3;
    public SpriteRenderer[] items;
    public Sprite[] sprites;
    public Sprite[] skills;
    public Sprite Recipe;
    public GameObject Score;
    public GameObject scoreUp;
    public GameObject KillStreak;
    public Text Row;
    public Font Nosifer;
    public Font StandartFont;

    public Slider timer;    // Слайдер времени таймер

    float time_combo;
    int combo;
    int score;
    int row;
    int a;
    int b;
    int ch;
    int number;
    int[] hint;
    GameObject[] chooseItems;
    int chooseItemsCount;
    int rampageCounter = 0;
    int gameCounter;

    int tmp = 0;

    int RowMax = 0;                                 // макс кол-во в ряд
    int DoubleKill, TripleKill, UltraKill, Rampage; // кол-во стриков

    int lastScore = 0;
    int lastLvl;

    public GameObject GameOverMenu;

    public Text RowText;
    public Text DoubleKillText;
    public Text TripleKillText;
    public Text UltraKillText;
    public Text RampageText;

    /* нажатие на кнопку с предметом */
    private int clicked;            // проверка для невозможности добавлять предметов больше, чем кол-во ячеек
    private bool FirstClick;        // флаг для записи ячеек в массив 
    private int temp;               // размер этого массива
    private GameObject[] target;    // массив ячеек
    private Vector3[] pos;          // массив расположения кнопок до их перемещения
    private GameObject button;      // кнопка с предметом
    private bool flag = false;
    private bool beginscore;

    private CameraShaker ShakeCam;  // тряска камеры при неверном ответе

    private bool correct = false;
    private bool isRight = true;
    private bool firstBlood;
    private bool gameOver = false;
    private bool buttonScoreDivide = false;
    private bool hintFlag = false;

    private Animator animatorScoreUp;
    private Animator animatorScore;
    private Animator aminatorKillStreak;
    private Animator animatorGameOverPanel;

    private AudioSource audiosource;
    #region Audioclips
    public AudioClip music;

    public AudioClip doublem;
    public AudioClip firstm;
    public AudioClip firstf;
    public AudioClip godm;
    public AudioClip godf;
    public AudioClip domm;
    public AudioClip domf;
    public AudioClip megam;
    public AudioClip monsterm;
    public AudioClip monsterf;
    public AudioClip rampagem;
    public AudioClip rampagef;
    public AudioClip spreem;
    public AudioClip spreef;
    public AudioClip triplem;
    public AudioClip ultram;
    public AudioClip ultraf;
    public AudioClip unstopm;
    public AudioClip unstopf;
    public AudioClip wickedm;
    public AudioClip wickedf;
    public AudioClip questcomplite;
    public AudioClip endGame;
    public AudioClip wrongItem;
    public AudioClip hintSound;
    public AudioClip hintParticlesSound;

    public AudioClip correctSound;
    #endregion
    public GameObject timerParticles;

    public Animator wrong_2;
    public Animator wrong_3;
    public Animator wrong_4;
    public Animator wrong_5;
    public Animator wrong_6;

    public GameObject[] HintParticles;
    public Button HIntButton;

    public GameObject exitPanel;
    public Animator animatorExitPanel;

    public string[] achName;
    public SpriteRenderer achievementSprite;
    public Sprite[] achieventSprites;
    public Animator achievementAnimator;

    private int itemsCount;
    private int heroesCount;
    private int itemsHerousCount;
    int scoreCounter;

    public Image GameOverAchImg;
    public Text GameOverAchString;

    public Slider experience;
    public Text LvlText;
    public SpriteRenderer lvlSprite;
    public Sprite[] lvlSprites;
    public Text mmrText;
    private int lastMMR;
    private string[] mmrAvarage;

    //public GameObject LevelLightParticle;
    public ParticleSystem HintButtonParticle;
    public ParticleSystem LevelLightParticle;
    private ParticleSystem.MainModule LevelLightParticle_1;

    public Text userName;
    private const string LeaderBoard = "CgkIu8LriKAEEAIQAg";        // гугл таблица лидеров
    private const string ExperienceBoard = "CgkIu8LriKAEEAIQMQ";    // гугл таблица опыта

    private string adRetryUnit = "ca-app-pub-1717383870946924/4041056296";
    private InterstitialAd interstitialRetry;
    private string adVideoPenalty = "ca-app-pub-1717383870946924/2564323093";
    private InterstitialAd interstitial;
    private bool PenaltyVideoShowed = false;
    private string adVideoId = "ca-app-pub-1717383870946924/3107549893";
    private RewardBasedVideoAd rewardBasedVideo;
    private bool VideoIsShowed = false;     // флаг для проверки (посмотрел ли юзер видео до конца)

    public Image avatarka;                  // аватар на панеле гейм овера
    public GameObject ContVideoBtn;         // Продолжить посмотрев рекламу
    public GameObject ContPenaltyBtn;       // Продолжить со штрафом посмотрев рекламу
    public Sprite ActiveContVideoBtn, DefaultContVidBtn, DefaultActiveBtn, DefaultInActiveBtn;
    int musicSettings;
    int soundSettings;
    int vibrationSettings;

    int GameMode;
    int LastTime;

    private void Start()
    {
        GameOverMenu.SetActive(false);
        ShakeCam = GetComponent<CameraShaker>();
        animatorScoreUp = scoreUp.GetComponent<Animator>();
        animatorScore = Score.GetComponent<Animator>();
        aminatorKillStreak = KillStreak.GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        animatorGameOverPanel = GameOverMenu.GetComponentInChildren<Animator>();
        animatorExitPanel = exitPanel.GetComponent<Animator>();

        if (Social.localUser.authenticated)
            StartCoroutine(LoadUserAvatar());

        if (PlayerPrefs.HasKey(achName[5]))
            gameCounter = PlayerPrefs.GetInt(achName[5]);
        else
            gameCounter = 0;

        gameCounter++;
        itemsHerousCount = 0;

        if (gameCounter == 1)
            scoreCounter = 0;
        else
            scoreCounter = PlayerPrefs.GetInt(achName[13]);

        mmrAvarage = new string[9] {"mmrAvarage_0", "mmrAvarage_1", "mmrAvarage_2", "mmrAvarage_3", "mmrAvarage_4", "mmrAvarage_5",
                                      "mmrAvarage_6", "mmrAvarage_7", "mmrAvarage_8"};

        firstBlood = true;
        row = 0;
        combo = 0;

        musicSettings = PlayerPrefs.GetInt("Music");
        soundSettings = PlayerPrefs.GetInt("Sound");
        vibrationSettings = PlayerPrefs.GetInt("Vibration");

        if (musicSettings != 0)
            audiosource.PlayOneShot(music);

        if (PlayerPrefs.HasKey("avarageDone"))
            lastMMR = PlayerPrefs.GetInt("avarageDone");

        if (PlayerPrefs.GetInt("Restart") == 1 && PlayerPrefs.GetInt(achName[34]) != 1)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress("CgkIu8LriKAEEAIQFA", 100.0f, (bool success) => {
                    if (success)
                    {
                        PlayerPrefs.SetInt(achName[34], 1);
                        AchievementAnimation(22);

                        PlayerPrefs.SetInt("LastAchImg", 22);
                        PlayerPrefs.SetString("LastAchString", achName[34]);

                        PlayerPrefs.Save();
                    }
                });
            }
        }

        PlayerPrefs.SetInt("Restart", 0);

        if (PlayerPrefs.HasKey("itemsCount"))
            itemsCount = PlayerPrefs.GetInt("itemsCount");
        else
            itemsCount = 0;

        if (PlayerPrefs.HasKey("heroesCount"))
            heroesCount = PlayerPrefs.GetInt("heroesCount");
        else
            heroesCount = 0;

        LevelLightParticle_1 = LevelLightParticle.main;

        if (PlayerPrefs.GetInt("GameMode") == 0)
            GameMode = 83;
        else if (PlayerPrefs.GetInt("GameMode") == 1)
            GameMode = 113;
        else
            GameMode = 196;

        if (PlayerPrefs.HasKey(achName[38]))
            LastTime = PlayerPrefs.GetInt(achName[38]);
        else
            LastTime = 0;


        RequestRevardedVideo();             // реклама на Продолжении без штрафа
        RequestInterstitial();              // реклама на Продолжении со штрафом
        if (gameCounter % 5 == 0)           // Retry реклама раз в пять игр
            RequestInterstitialRetry();
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (isRight)
            {
                chooseItemsCount = 0;
                clicked = 0;
                FirstClick = true;
                flag = false;
                beginscore = true;

                randomItem();
                Panel();
                MainElement();
                lowItems();

                time_combo = 5f;

                animatorScore.Play("Score");
                tmp = score - number;

                #region HintButton

                if (score < 1000 && combo != 0 && combo % 2 == 0)
                    HintActive();

                else if (score < 10000 && combo != 0 && combo % 4 == 0)
                    HintActive();

                else if (combo != 0 && combo % 8 == 0)
                    HintActive();

                #endregion

                isRight = false;
            }

            if (!audiosource.isPlaying && !gameOver && musicSettings != 0)
                audiosource.PlayOneShot(music);

            if (time_combo > 0)
                time_combo -= Time.deltaTime;

            if (beginscore)
            {
                Score.GetComponent<Text>().text = "Score: " + tmp;

                if (tmp + 10 <= score)
                    tmp += 10;

                else if (tmp + 5 <= score)
                    tmp += 5;

                else if (tmp + 1 <= score)
                    tmp++;

                else
                    beginscore = false;
            }

            GameOver();
        }


        if (PlayerPrefs.GetFloat(achName[38]) != 1800000)
        {
            if (PlayerPrefs.GetFloat(achName[35]) != 3600)
            {
                float timeSpend = Time.deltaTime;
                float timeSpendAtAll = PlayerPrefs.GetFloat(achName[38]);

                if (timeSpendAtAll + timeSpend < 3600)
                {
                    PlayerPrefs.SetFloat(achName[38], timeSpendAtAll + timeSpend);
                }

                else
                {
                    PlayerPrefs.SetFloat(achName[35], 3600);
                    AchievementAnimation(29);

                    PlayerPrefs.SetInt("LastAchImg", 29);
                    PlayerPrefs.SetString("LastAchString", achName[35]);
                }
            }

            else if (PlayerPrefs.GetFloat(achName[36]) != 18000)
            {
                float timeSpend = Time.deltaTime;
                float timeSpendAtAll = PlayerPrefs.GetFloat(achName[38]);

                if (timeSpendAtAll + timeSpend < 18000)
                {
                    PlayerPrefs.SetFloat(achName[38], timeSpendAtAll + timeSpend);
                }

                else
                {
                    PlayerPrefs.SetFloat(achName[36], 18000);
                    AchievementAnimation(13);

                    PlayerPrefs.SetInt("LastAchImg", 13);
                    PlayerPrefs.SetString("LastAchString", achName[36]); 
                }
            }

            else if (PlayerPrefs.GetFloat(achName[37]) != 36000)
            {
                float timeSpend = Time.deltaTime;
                float timeSpendAtAll = PlayerPrefs.GetFloat(achName[38]);

                if (timeSpendAtAll + timeSpend < 36000)
                {
                    PlayerPrefs.SetFloat(achName[38], timeSpendAtAll + timeSpend);
                }

                else
                {
                    PlayerPrefs.SetFloat(achName[37], 36000);
                    AchievementAnimation(41);

                    PlayerPrefs.SetInt("LastAchImg", 41);
                    PlayerPrefs.SetString("LastAchString", achName[37]);
                }
            }

            else
            {
                float timeSpend = Time.deltaTime;
                float timeSpendAtAll = PlayerPrefs.GetFloat(achName[38]);

                if (timeSpendAtAll + timeSpend < 180000)
                {
                    PlayerPrefs.SetFloat(achName[38], timeSpendAtAll + timeSpend);
                }

                else
                {
                    PlayerPrefs.SetFloat(achName[38], 180000);
                    AchievementAnimation(10);

                    PlayerPrefs.SetInt("LastAchImg", 10);
                    PlayerPrefs.SetString("LastAchString", achName[38]);
                }
            }
        }
    }

    private void RequestRevardedVideo()
    {
        rewardBasedVideo = RewardBasedVideoAd.Instance;
        AdRequest request = new AdRequest.Builder().Build();
        rewardBasedVideo.LoadAd(request, adVideoId);
        rewardBasedVideo.OnAdRewarded += OnVideoAdRewarded;     
        rewardBasedVideo.OnAdClosed += OnAdClosedVideo;         
    }
    private void OnAdClosedVideo(object obj, EventArgs e)       // если чел недосмотрел рекламу
    {
        RequestRevardedVideo();
    }
    private void OnVideoAdRewarded(object obj, EventArgs e)     // если досмотрел рекламу
    {
        VideoIsShowed = true;
        ContVideoBtn.GetComponent<Image>().sprite = ActiveContVideoBtn;
    }

    private void RequestInterstitial()
    {
        interstitial = new InterstitialAd(adVideoPenalty);

        AdRequest requestInt = new AdRequest.Builder().Build();
        interstitial.OnAdFailedToLoad += RetryToLoad;
        interstitial.LoadAd(requestInt);
    }
    private void RetryToLoad(object o, EventArgs e)
    {
        RequestInterstitial();
    }

    private void RequestInterstitialRetry()
    {
        interstitialRetry = new InterstitialAd(adRetryUnit);

        AdRequest requestInt = new AdRequest.Builder().Build();
        interstitialRetry.LoadAd(requestInt);
    }

    private void randomItem()
    {
        if (PlayerPrefs.GetInt("GameMode") == 0)
            ch = 0;
        else if (PlayerPrefs.GetInt("GameMode") == 1)
            ch = 1;
        else
            ch = (int)UnityEngine.Random.Range(0.0f, 1.9f);

        if (ch == 0)
        {
            a = (int)UnityEngine.Random.Range(0.0f, 2.9f);
            if (a == 0)
            {
                b = (int)UnityEngine.Random.Range(0.0f, from2list.Length);
                if (from2list[b].available)
                {
                    from2list[b].available = false;
                    itemsHerousCount++;
                }
                else
                    if (itemsHerousCount != 196)
                    randomItem();
            }

            else if (a == 1)
            {
                b = (int)UnityEngine.Random.Range(0.0f, from3list.Length);
                if (from3list[b].available)
                {
                    from3list[b].available = false;
                    itemsHerousCount++;
                }
                else
                    if (itemsHerousCount != 196)
                    randomItem();
            }
            else
            {
                b = (int)UnityEngine.Random.Range(0.0f, from4list.Length);
                if (from4list[b].available)
                {
                    from4list[b].available = false;
                    itemsHerousCount++;
                }
                else
                    if (itemsHerousCount != 196)
                    randomItem();
            }
        }

        else
        {
            a = (int)UnityEngine.Random.Range(0.0f, 112.9f);

            if (!heroes[a].available)
            {
                heroes[a].available = true;
                itemsHerousCount++;
            }
            else
                if (itemsHerousCount != 196)
                randomItem();
        }
    }

    private void Panel()
    {
        if (ch == 0)
        {
            if (a == 0)
            {
                Panel_1.SetActive(true);
                Panel_2.SetActive(false);
                Panel_3.SetActive(false);
                HeroPanel_1.SetActive(false);
                HeroPanel_2.SetActive(false);
                HeroPanel_3.SetActive(false);
            }
            else if (a == 1)
            {
                Panel_1.SetActive(false);
                Panel_2.SetActive(true);
                Panel_3.SetActive(false);
                HeroPanel_1.SetActive(false);
                HeroPanel_2.SetActive(false);
                HeroPanel_3.SetActive(false);
            }
            else
            {
                Panel_1.SetActive(false);
                Panel_2.SetActive(false);
                Panel_3.SetActive(true);
                HeroPanel_2.SetActive(false);
                HeroPanel_3.SetActive(false);
            }
        }
        else
        {
            if (heroes[a].answers.Length == 4)
            {
                Panel_1.SetActive(false);
                Panel_2.SetActive(false);
                HeroPanel_1.SetActive(true);
                HeroPanel_2.SetActive(false);
                HeroPanel_3.SetActive(false);
            }

            else if (heroes[a].answers.Length == 5)
            {
                Panel_1.SetActive(false);
                Panel_2.SetActive(false);
                Panel_3.SetActive(false);
                HeroPanel_1.SetActive(false);
                HeroPanel_2.SetActive(true);
                HeroPanel_3.SetActive(false);
            }

            else
            {
                Panel_1.SetActive(false);
                Panel_2.SetActive(false);
                Panel_3.SetActive(false);
                HeroPanel_1.SetActive(false);
                HeroPanel_2.SetActive(false);
                HeroPanel_3.SetActive(true);
            }
        }
    }

    private void MainElement()
    {
        if (ch == 0)
        {
            MainRamka.transform.localScale = new Vector3(23.2f, 16.8f, 1f);
            Main.transform.localScale = new Vector3(94f, 90f, 1f);

            if (a == 0)
            {
                Main.sprite = from2list[b].question;

            }
            else if (a == 1)
            {
                Main.sprite = from3list[b].question;
            }
            else
            {
                Main.sprite = from4list[b].question;
            }
        }
        else
        {
            MainRamka.transform.localScale = new Vector3(28.2f, 16.8f, 1f);
            Main.transform.localScale = new Vector3(40f, 40f, 1f);
            Main.sprite = heroes[a].question;
        }
    }

    private void lowItems()
    {
        bool Compare = true;
        int tempSize;
        if (ch == 0)
        {
            for (int i = 0; i < 14; i++)
            {
                items[i].transform.localScale = new Vector3(1.15f, 1.1f, 0f);
                items[i].sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
            }

            items[13].sprite = Recipe;

            if (a == 0)
                tempSize = from2list[b].answers.Length;
            else if (a == 1)
                tempSize = from3list[b].answers.Length;
            else
                tempSize = from4list[b].answers.Length;

            int[] temp = new int[tempSize];
            hint = new int[tempSize];

            while (Compare)
            {
                Compare = false;

                for (int i = 0; i < tempSize; i++)
                {
                    temp[i] = (int)UnityEngine.Random.Range(0.0f, 12.9f);
                }

                //Debug.Log("Yes");

                for (int i = 0; i < tempSize - 1; i++)
                {
                    for (int j = i + 1; j < tempSize; j++)
                    {
                        if (temp[i] == temp[j])
                        {
                            Compare = true;
                            //Debug.Log("Yep!");
                        }
                    }

                }
            }
            if (a == 0)
                for (int i = 0, c = 0; i < tempSize && c < tempSize; i++, c++)
                {
                    if (from2list[b].answers[c].name == "Recipe")
                    {
                        c++;
                        if (c < tempSize)
                            items[temp[i]].sprite = from2list[b].answers[c];

                        hint[i] = 13;
                    }
                    else
                    {
                        items[temp[i]].sprite = from2list[b].answers[c];
                        hint[i] = temp[i];
                    }
                    Debug.Log(hint[i]);
                }
            else if (a == 1)
                for (int i = 0, c = 0; i < tempSize && c < tempSize; i++, c++)
                {
                    if (from3list[b].answers[c].name == "Recipe")
                    {
                        c++;
                        if (c < tempSize)
                            items[temp[i]].sprite = from3list[b].answers[c];
                        hint[i] = 13;
                    }
                    else
                    {
                        items[temp[i]].sprite = from3list[b].answers[c];
                        hint[i] = temp[i];
                    }
                    Debug.Log(hint[i]);
                }
            else
                for (int i = 0, c = 0; i < tempSize && c < tempSize; i++, c++)
                {
                    if (from4list[b].answers[c].name == "Recipe")
                    {
                        c++;
                        if (c < tempSize)
                            items[temp[i]].sprite = from4list[b].answers[c];
                        hint[i] = 13;
                    }
                    else
                    {
                        items[temp[i]].sprite = from4list[b].answers[c];
                        hint[i] = temp[i];
                    }
                    Debug.Log(hint[i]);
                }
        }

        else
        {
            tempSize = heroes[a].answers.Length;
            hint = new int[tempSize];

            for (int i = 0; i < 14; i++)
            {
                items[i].transform.localScale = new Vector3(0.76f, 0.56f, 0f);
                items[i].sprite = skills[UnityEngine.Random.Range(0, skills.Length)];
            }

            int[] temp = new int[tempSize];

            while (Compare)
            {
                Compare = false;

                for (int i = 0; i < tempSize; i++)
                {
                    temp[i] = (int)UnityEngine.Random.Range(0.0f, 13.9f);
                }

                //Debug.Log("Yes");

                for (int i = 0; i < tempSize - 1; i++)
                {
                    for (int j = i + 1; j < tempSize; j++)
                    {
                        if (temp[i] == temp[j])
                        {
                            Compare = true;
                            //Debug.Log("Yep!");
                        }
                    }

                }
            }

            for (int i = 0, c = 0; i < tempSize && c < tempSize; i++, c++)
            {
                items[temp[i]].sprite = heroes[a].answers[c];
                hint[i] = temp[i];

                Debug.Log(hint[i]);
            }

        }
    }

    public void OnClick()
    {
        if (!gameOver)
        {
            if (!flag)
            {
                button = GameObject.Find("Gues/" + gameObject.name);
                if (hintFlag)
                    HintParticles[Convert.ToInt32(button.name) - 1].SetActive(false);


                if (FirstClick)
                {
                    FirstClick = false;
                    if (Panel_1.activeSelf)
                        temp = 2;
                    else if (Panel_2.activeSelf)
                        temp = 3;
                    else if (Panel_3.activeSelf || HeroPanel_1.activeSelf)
                        temp = 4;
                    else if (HeroPanel_2.activeSelf)
                        temp = 5;
                    else
                        temp = 6;

                    chooseItems = new GameObject[temp];
                    target = new GameObject[temp];
                    pos = new Vector3[temp];
                    for (var i = 1; i < temp + 1; i++)
                    {
                        target[i - 1] = GameObject.Find("Items/" + i.ToString());
                    }
                }

                if (clicked <= temp)
                {
                    for (var i = 0; i < temp; i++)
                    {
                        /* 
                         * если позиция кнопки совпадает с позицией целевой ячейки, то
                         * переносим кнопку обратно и записываем нули в его бывшее расположение,
                         * чтобы в дальнейшем было ясно, что это место является свободным
                        */
                        if (button.transform.position == target[i].transform.position)
                        {
                            StartCoroutine(MoveYoAss(pos[i]));
                            pos[i] = new Vector3(0, 0);
                            clicked--;
                            chooseItemsCount--;
                            break;
                        }

                        // если позиция кнопки не совпадает со всеми целевыми ячейками
                        if ((i + 1) == temp && clicked < temp)
                        {
                            for (var n = 0; n < temp; n++)
                            {
                                /* 
                                 * проверяем свободные ячейки, если таковая есть, то
                                 * записываем в неё текущую позицию кнопки и переносим в ячейку
                                 */
                                if (pos[n] == new Vector3(0, 0))
                                {
                                    pos[n] = button.transform.position;
                                    //button.transform.position = target[n].transform.position;
                                    StartCoroutine(MoveYoAss(target[n].transform.position));
                                    clicked++;
                                    chooseItems[n] = button;
                                    chooseItemsCount++;

                                    break;
                                }
                            }

                        }
                    }

                }

            }
            if (chooseItemsCount == temp)
            {
                CheckAnswer();
            }
        }
    }
    private IEnumerator MoveYoAss(Vector3 target)
    {
        flag = true;
        while (button.transform.position != target)
        {
            button.transform.position = Vector3.Lerp(button.transform.position, target, 15f * Time.deltaTime);
            if (Mathf.Round(button.transform.position.x) == Mathf.Round(target.x) && Mathf.Round(button.transform.position.y) == Mathf.Round(target.y))
                break;
            yield return new WaitForSeconds(.01f);
        }
        button.transform.position = target;

        flag = false;

        if (correct)
        {
            for (int i = 0; i < temp; i++)
            {
                chooseItems[i].transform.position = pos[i];
            }

            ScoreIncrease();
            if (hintFlag)
                for (int i = 0; i < hint.Length; i++)
                {
                    HintParticles[hint[i]].SetActive(false);
                }
            hintFlag = false;

            animatorScoreUp.Play("ScoreUp");

            yield return new WaitForSeconds(.5f);

            #region items/heroes Counter

            if (ch == 0)
            {
                if (a == 0)
                {
                    if (!PlayerPrefs.HasKey(from2list[b].question.name))
                    {
                        PlayerPrefs.SetInt(from2list[b].question.name, 1);
                        itemsCount++;
                        PlayerPrefs.SetInt("itemsCount", itemsCount);
                    }
                }

                else if (a == 1)
                {
                    if (!PlayerPrefs.HasKey(from3list[b].question.name))
                    {
                        PlayerPrefs.SetInt(from3list[b].question.name, 1);
                        itemsCount++;
                        PlayerPrefs.SetInt("itemsCount", itemsCount);
                    }
                }

                else
                {
                    if (!PlayerPrefs.HasKey(from4list[b].question.name))
                    {
                        PlayerPrefs.SetInt(from4list[b].question.name, 1);
                        itemsCount++;
                        PlayerPrefs.SetInt("itemsCount", itemsCount);
                    }
                }

            }

            else
            {
                if (!PlayerPrefs.HasKey(heroes[a].question.name))
                {
                    PlayerPrefs.SetInt(heroes[a].question.name, 1);
                    heroesCount++;
                    PlayerPrefs.SetInt("heroesCount", heroesCount);
                }
            }

            #endregion
            

            #region ACHIEVEMENTS
            if (Social.localUser.authenticated)
            {
                if (PlayerPrefs.GetInt(achName[0]) != 1)
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQBQ", 100.0f, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(achName[0], 1);
                            AchievementAnimation(12);

                            PlayerPrefs.SetInt("LastAchImg", 12);
                            PlayerPrefs.SetString("LastAchString", achName[0]);
                        }
                    });
                }

                if (PlayerPrefs.GetInt(achName[1]) != 10)
                {
                    if (PlayerPrefs.GetInt(achName[1]) < 9)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQHg", gameCounter * 10, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[1], gameCounter);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQHg", 100f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[1], 10);
                                AchievementAnimation(23);

                                PlayerPrefs.SetInt("LastAchImg", 23);
                                PlayerPrefs.SetString("LastAchString", achName[1]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[2]) != 50)
                {
                    if (PlayerPrefs.GetInt(achName[2]) < 49)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQHw", gameCounter * 2f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[2], gameCounter);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQHw", 100f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[2], 50);
                                AchievementAnimation(28);

                                PlayerPrefs.SetInt("LastAchImg", 28);
                                PlayerPrefs.SetString("LastAchString", achName[2]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[3]) != 100)
                {
                    if (PlayerPrefs.GetInt(achName[3]) < 99)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQIA", gameCounter * 1f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[3], gameCounter);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQIA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[3], 100);
                                AchievementAnimation(31);

                                PlayerPrefs.SetInt("LastAchImg", 31);
                                PlayerPrefs.SetString("LastAchString", achName[3]);
                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[4]) != 1000)
                {
                    if (PlayerPrefs.GetInt(achName[4]) < 999)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQIQ", gameCounter * 0.1f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[4], gameCounter);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQIQ", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[4], 1000);
                                AchievementAnimation(20);

                                PlayerPrefs.SetInt("LastAchImg", 20);
                                PlayerPrefs.SetString("LastAchString", achName[4]);
                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[5]) != 10000)
                {
                    if (PlayerPrefs.GetInt(achName[5]) < 9999)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQIg", gameCounter * 0.01f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[5], gameCounter);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQIg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[5], 10000);
                                AchievementAnimation(1);

                                PlayerPrefs.SetInt("LastAchImg", 1);
                                PlayerPrefs.SetString("LastAchString", achName[5]);
                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[6]) != 1)
                {
                    if (score - 5000 >= 0)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQIw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[6], 1);
                                AchievementAnimation(27);

                                PlayerPrefs.SetInt("LastAchImg", 27);
                                PlayerPrefs.SetString("LastAchString", achName[6]);
                            }
                        });
                    }

                }

                else if (PlayerPrefs.GetInt(achName[7]) != 1)
                {
                    if (score - 10000 >= 0)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQJA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[7], 1);
                                AchievementAnimation(2);

                                PlayerPrefs.SetInt("LastAchImg", 2);
                                PlayerPrefs.SetString("LastAchString", achName[7]);
                            }
                        });
                    }

                }

                else if (PlayerPrefs.GetInt(achName[8]) != 1)
                {
                    if (score - 30000 >= 0)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQJQ", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[8], 1);
                                AchievementAnimation(45);

                                PlayerPrefs.SetInt("LastAchImg", 45);
                                PlayerPrefs.SetString("LastAchString", achName[8]);
                            }
                        });
                    }

                }

                else if (PlayerPrefs.GetInt(achName[9]) != 1)
                {
                    if (score - 100000 >= 0)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQJg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[9], 1);
                                AchievementAnimation(24);

                                PlayerPrefs.SetInt("LastAchImg", 24);
                                PlayerPrefs.SetString("LastAchString", achName[9]);
                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[13]) != 5000000)
                {
                    if (scoreCounter + score < 5000000)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKg", (scoreCounter + score) * 0.00002f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[13], scoreCounter + score);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[13], 5000000);
                                AchievementAnimation(3);

                                PlayerPrefs.SetInt("LastAchImg", 3);
                                PlayerPrefs.SetString("LastAchString", achName[13]);
                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[10]) != 100000)
                {
                    if (scoreCounter + score >= 100000)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQJw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[10], 100000);
                                AchievementAnimation(9);

                                PlayerPrefs.SetInt("LastAchImg", 9);
                                PlayerPrefs.SetString("LastAchString", achName[10]);
                            }
                        });
                    }
                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQJw", (scoreCounter + score) * 0.001f, (bool success) =>
                        {
                            if (success)
                            {

                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[11]) != 500000)
                {
                    if (scoreCounter + score >= 500000)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[11], 500000);
                                AchievementAnimation(44);

                                PlayerPrefs.SetInt("LastAchImg", 44);
                                PlayerPrefs.SetString("LastAchString", achName[11]);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKA", (scoreCounter + score) * 0.0002f, (bool success) =>
                        {
                            if (success)
                            {

                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[12]) != 1000000)
                {
                    if (scoreCounter + score >= 1000000)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKQ", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[12], 1000000);
                                AchievementAnimation(36);

                                PlayerPrefs.SetInt("LastAchImg", 36);
                                PlayerPrefs.SetString("LastAchString", achName[12]);
                            }
                        });
                    }

                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKQ", (scoreCounter + score) * 0.0001f, (bool success) =>
                        {
                            if (success)
                            {

                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[14]) != 1)
                {
                    if (row == 9)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQBg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[14], 1);
                                AchievementAnimation(18);

                                PlayerPrefs.SetInt("LastAchImg", 18);
                                PlayerPrefs.SetString("LastAchString", achName[14]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[15]) != 1)
                {
                    if (row == 20)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQBw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[15], 1);
                                AchievementAnimation(37);

                                PlayerPrefs.SetInt("LastAchImg", 37);
                                PlayerPrefs.SetString("LastAchString", achName[15]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[16]) != 1)
                {
                    if (row == 69)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQCA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[16], 1);
                                AchievementAnimation(19);

                                PlayerPrefs.SetInt("LastAchImg", 19);
                                PlayerPrefs.SetString("LastAchString", achName[16]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[17]) != 1)
                {
                    if (row == 100)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQCQ", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[17], 1);
                                AchievementAnimation(15);

                                PlayerPrefs.SetInt("LastAchImg", 15);
                                PlayerPrefs.SetString("LastAchString", achName[17]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[18]) != 1)
                {
                    if (row == 195)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQCg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[18], 1);
                                AchievementAnimation(16);

                                PlayerPrefs.SetInt("LastAchImg", 16);
                                PlayerPrefs.SetString("LastAchString", achName[18]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[19]) != 1)
                {
                    if (itemsCount == 83)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[19], 1);
                                AchievementAnimation(25);

                                PlayerPrefs.SetInt("LastAchImg", 25);
                                PlayerPrefs.SetString("LastAchString", achName[19]);
                            }
                        });
                    }
                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQKw", itemsCount * 1.2f, (bool success) =>
                        {
                            if (success)
                            {
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[20]) != 1)
                {
                    if (heroesCount == 113)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQLA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[20], 1);
                                AchievementAnimation(11);

                                PlayerPrefs.SetInt("LastAchImg", 11);
                                PlayerPrefs.SetString("LastAchString", achName[20]);
                            }
                        });
                    }
                    else
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQLA", heroesCount * 0.88f, (bool success) =>
                        {
                            if (success)
                            {
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[21]) != 1)
                {
                    if (PlayerPrefs.GetInt(achName[19]) == 1 && PlayerPrefs.GetInt(achName[20]) == 1)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQEQ", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[21], 1);
                                AchievementAnimation(4);

                                PlayerPrefs.SetInt("LastAchImg", 4);
                                PlayerPrefs.SetString("LastAchString", achName[21]);
                            }
                        });
                    }

                }

                if (PlayerPrefs.GetInt(achName[22]) != 1)
                {
                    if (rampageCounter == 1)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQCw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[22], 1);
                                AchievementAnimation(34);

                                PlayerPrefs.SetInt("LastAchImg", 34);
                                PlayerPrefs.SetString("LastAchString", achName[22]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[23]) != 1)
                {
                    if (rampageCounter == 5)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQDA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[23], 1);
                                AchievementAnimation(42);

                                PlayerPrefs.SetInt("LastAchImg", 42);
                                PlayerPrefs.SetString("LastAchString", achName[23]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[24]) != 1)
                {
                    if (rampageCounter == 10)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQDQ", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[24], 1);
                                AchievementAnimation(47);

                                PlayerPrefs.SetInt("LastAchImg", 47);
                                PlayerPrefs.SetString("LastAchString", achName[24]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[25]) != 1)
                {
                    if (rampageCounter == 50)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQDg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[25], 1);
                                AchievementAnimation(43);

                                PlayerPrefs.SetInt("LastAchImg", 43);
                                PlayerPrefs.SetString("LastAchString", achName[25]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[26]) != 1)
                {
                    if (rampageCounter == 100)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQDw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[26], 1);
                                AchievementAnimation(6);

                                PlayerPrefs.SetInt("LastAchImg", 6);
                                PlayerPrefs.SetString("LastAchString", achName[26]);
                            }
                        });
                    }
                }

                else if (PlayerPrefs.GetInt(achName[27]) != 1)
                {
                    if (rampageCounter == 191)
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQEA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[27], 1);
                                AchievementAnimation(17);

                                PlayerPrefs.SetInt("LastAchImg", 17);
                                PlayerPrefs.SetString("LastAchString", achName[27]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[40]) != 1)
                {
                    if (PlayerPrefs.HasKey("greater_crit_lg"))
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQFg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[40], 1);
                                AchievementAnimation(38);

                                PlayerPrefs.SetInt("LastAchImg", 38);
                                PlayerPrefs.SetString("LastAchString", achName[40]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[41]) != 1)
                {
                    if (PlayerPrefs.HasKey("ultimate_scepter_lg"))
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQFw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[41], 1);
                                AchievementAnimation(39);

                                PlayerPrefs.SetInt("LastAchImg", 39);
                                PlayerPrefs.SetString("LastAchString", achName[41]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[42]) != 1)
                {
                    if (PlayerPrefs.HasKey("guardian_greaves_lg"))
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQGA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[42], 1);
                                AchievementAnimation(40);

                                PlayerPrefs.SetInt("LastAchImg", 40);
                                PlayerPrefs.SetString("LastAchString", achName[42]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[43]) != 1)
                {
                    if (PlayerPrefs.HasKey("Phantom_Assassin_Large"))
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQGQ", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[43], 1);
                                AchievementAnimation(21);

                                PlayerPrefs.SetInt("LastAchImg", 21);
                                PlayerPrefs.SetString("LastAchString", achName[43]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[44]) != 1)
                {
                    if (PlayerPrefs.HasKey("Sniper_Large"))
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQGg", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[44], 1);
                                AchievementAnimation(35);

                                PlayerPrefs.SetInt("LastAchImg", 35);
                                PlayerPrefs.SetString("LastAchString", achName[44]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[45]) != 1)
                {
                    if (PlayerPrefs.HasKey("Invoker_Large"))
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQGw", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[45], 1);
                                AchievementAnimation(30);

                                PlayerPrefs.SetInt("LastAchImg", 30);
                                PlayerPrefs.SetString("LastAchString", achName[45]);
                            }
                        });
                    }
                }

                if (PlayerPrefs.GetInt(achName[46]) != 1)
                {
                    if (PlayerPrefs.HasKey("Anti-Mage_Large"))
                    {
                        Social.ReportProgress("CgkIu8LriKAEEAIQHA", 100.0f, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(achName[46], 1);
                                AchievementAnimation(0);

                                PlayerPrefs.SetInt("LastAchImg", 0);
                                PlayerPrefs.SetString("LastAchString", achName[46]);
                            }
                        });
                    }
                }

                double timeCentuary = PlayerPrefs.GetFloat(achName[38]);

                if (PlayerPrefs.GetInt(achName[35]) < 3600)
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQLQ", timeCentuary * 0.027d, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }
                else
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQLQ", 100f, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }

                if (PlayerPrefs.GetInt(achName[36]) < 18000)
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQLg", timeCentuary * 0.0055d, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }

                else
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQLg", 100f, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }

                if (PlayerPrefs.GetInt(achName[37]) < 36000)
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQLw", timeCentuary * 0.0027d, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }

                else
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQLw", 100f, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }

                if (PlayerPrefs.GetInt(achName[38]) < 180000)
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQMA", timeCentuary * 0.00055d, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }
                else
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQMA", 100f, (bool success) =>
                    {
                        if (success)
                        {
                        }
                    });
                }
            }

            else
                PlayerPrefs.SetInt(achName[38], LastTime);

            #endregion

            PlayerPrefs.Save();

            isRight = true;
            correct = false;

            Debug.Log("Combo = " + combo);
        }
    }

    private void CheckAnswer()
    {
        if (chooseItems[temp - 1] != null)
        {
            if (ch == 0)
            {
                if (a == 0)
                {
                    for (int i = 0; i < temp; i++)
                    {
                        for (int j = 0; j < temp; j++)
                        {
                            if (chooseItems[j].GetComponentInChildren<SpriteRenderer>().sprite.name == from2list[b].answers[i].name)
                            {
                                correct = true;
                                //   Debug.Log("Yep");
                                break;
                            }
                            else
                            {
                                correct = false;
                                //  Debug.Log("NO");
                            }
                        }

                        if (!correct)
                        {
                            combo = -1;
                            row = 0;
                            //Embers.Stop();
                            firstBlood = false;
                            Row.text = "IN ROW: " + row.ToString();
                            break;
                        }
                    }
                }

                else if (a == 1)
                {
                    for (int i = 0; i < temp; i++)
                    {
                        for (int j = 0; j < temp; j++)
                        {
                            if (chooseItems[j].GetComponentInChildren<SpriteRenderer>().sprite.name == from3list[b].answers[i].name)
                            {
                                correct = true;
                                // Debug.Log("Yep");
                                break;
                            }
                            else
                            {
                                correct = false;
                                //  Debug.Log("NO");
                            }
                        }

                        if (!correct)
                        {
                            combo = -1;
                            row = 0;
                            //Embers.Stop();
                            firstBlood = false;
                            Row.text = "IN ROW: " + row.ToString();
                            break;
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < temp; i++)
                    {
                        for (int j = 0; j < temp; j++)
                        {
                            if (chooseItems[j].GetComponentInChildren<SpriteRenderer>().sprite.name == from4list[b].answers[i].name)
                            {
                                correct = true;
                                // Debug.Log("Yep");
                                break;
                            }
                            else
                            {
                                correct = false;
                                // Debug.Log("NO");
                            }
                        }

                        if (!correct)
                        {
                            combo = -1;
                            row = 0;
                            //Embers.Stop();
                            firstBlood = false;
                            Row.text = "IN ROW: " + row.ToString();
                            break;
                        }

                    }
                }
            }

            else
            {
                for (int i = 0; i < temp; i++)
                {
                    for (int j = 0; j < temp; j++)
                    {
                        if (chooseItems[j].GetComponentInChildren<SpriteRenderer>().sprite.name == heroes[a].answers[i].name)
                        {
                            correct = true;
                            //   Debug.Log("Yep");
                            break;
                        }
                        else
                        {
                            correct = false;
                            //  Debug.Log("NO");
                        }
                    }

                    if (!correct)
                    {
                        combo = -1;
                        row = 0;
                        //Embers.Stop();
                        firstBlood = false;
                        Row.text = "IN ROW: " + row.ToString();
                        break;
                    }
                }
            }
        }
        if (!correct)
        {
            if (ch == 0)
            {
                if (a == 0)
                    wrong_2.Play("Fail_Particle_2");
                else if (a == 1)
                    wrong_3.Play("Fail_Particle_3");
                else
                    wrong_4.Play("Fail_Particle_4");
            }
            else
            {
                if (HeroPanel_1.activeSelf)
                    wrong_4.Play("Fail_Particle_4");
                else if (HeroPanel_2.activeSelf)
                    wrong_5.Play("Fail_Particle_5");
                else
                    wrong_6.Play("Fail_Particle_6");


            }
            //StartCoroutine(StopSound());
            if (soundSettings != 0)
                audiosource.PlayOneShot(wrongItem);
            if (vibrationSettings != 0)
                Handheld.Vibrate();
            ShakeCam.ShakeCamera();
            StartCoroutine(ChangeColor());
            timer.value -= 1;
        }
    }

    private IEnumerator ChangeColor()
    {
        if (Panel_1.activeSelf)
        {
            Panel_1.GetComponent<Image>().color = new Vector4(1, 0, 0, 1f);
            yield return new WaitForSeconds(1f);
            Panel_1.GetComponent<Image>().color = new Vector4(1, 1, 1, 1f);
        }
        else if (Panel_2.activeSelf)
        {
            Panel_2.GetComponent<Image>().color = new Vector4(1, 0, 0, 1f);
            yield return new WaitForSeconds(1f);
            Panel_2.GetComponent<Image>().color = new Vector4(1, 1, 1, 1f);
        }
        else
        {
            Panel_3.GetComponent<Image>().color = new Vector4(1, 0, 0, 1f);
            yield return new WaitForSeconds(1f);
            Panel_3.GetComponent<Image>().color = new Vector4(1, 1, 1, 1f);
        }

    }

    private void ScoreIncrease()
    {
        if (soundSettings != 0)
            audiosource.PlayOneShot(correctSound);

        number = 100 + row * 10;
        int rand;

        rand = UnityEngine.Random.Range(0, 10);

        if (time_combo > 0) //(DoubleKill, Rampage) и тд
        {
            if (firstBlood)
            {
                if (soundSettings != 0)
                {
                    if (rand % 10 == 0)
                        audiosource.PlayOneShot(firstf);
                    else
                        audiosource.PlayOneShot(firstm);
                }
                KillStreak.GetComponent<Text>().text = "FirstBlood";
                KillStreak.GetComponent<Text>().font = Nosifer;
                KillStreak.GetComponent<Text>().color = new Vector4(0.58f, 0.04f, 0.04f, 255);
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("FirstBlood!");
                firstBlood = false;

                number = 200;
            }
            else
            {
                if (row >= 1 && combo == 0)
                {
                    if (KillStreak.GetComponent<Text>().font != StandartFont)
                    {
                        KillStreak.GetComponent<Text>().font = StandartFont;
                        KillStreak.GetComponent<Text>().color = new Vector4(0.2f, 0.2f, 0.2f, 255);
                    }

                    if (soundSettings != 0)
                        audiosource.PlayOneShot(doublem);

                    KillStreak.GetComponent<Text>().text = "Double Kill";
                    aminatorKillStreak.Play("KillStreak");
                    Debug.Log("Combox2");
                    number = (int)Mathf.Round(100 + row * 10 * 1.5f);
                    timer.value += 1;

                    DoubleKill++;
                }
                else if (time_combo > 0 && row >= 2 && combo == 1)
                {
                    if (KillStreak.GetComponent<Text>().font != StandartFont)
                    {
                        KillStreak.GetComponent<Text>().font = StandartFont;
                        KillStreak.GetComponent<Text>().color = new Vector4(0.2f, 0.2f, 0.2f, 255);
                    }

                    if (soundSettings != 0)
                        audiosource.PlayOneShot(triplem);

                    KillStreak.GetComponent<Text>().text = "Triple Kill";
                    aminatorKillStreak.Play("KillStreak");
                    Debug.Log("Combox3");
                    number = (int)Mathf.Round(100 + row * 10 * 1.75f);
                    timer.value += 1.5f;

                    TripleKill++;
                }
                else if (time_combo > 0 && row >= 3 && combo == 2)
                {
                    if (KillStreak.GetComponent<Text>().font != StandartFont)
                    {
                        KillStreak.GetComponent<Text>().font = StandartFont;
                        KillStreak.GetComponent<Text>().color = new Vector4(0.2f, 0.2f, 0.2f, 255);
                    }
                    if (soundSettings != 0)
                    {
                        if (rand % 10 == 0)
                            audiosource.PlayOneShot(ultraf);
                        else
                            audiosource.PlayOneShot(ultram);
                    }
                    KillStreak.GetComponent<Text>().text = "UltraKill";
                    aminatorKillStreak.Play("KillStreak");
                    Debug.Log("Combox4");
                    number = (int)Mathf.Round(100 + row * 10 * 2f);
                    timer.value += 2;

                    UltraKill++;
                }
                else if (time_combo > 0 && row > 3 && combo >= 3)
                {
                    if (soundSettings != 0)
                    {
                        if (rand % 10 == 0)
                            audiosource.PlayOneShot(rampagef);
                        else
                            audiosource.PlayOneShot(rampagem);
                    }
                    if (KillStreak.GetComponent<Text>().font == StandartFont)
                    {
                        KillStreak.GetComponent<Text>().font = Nosifer;
                        KillStreak.GetComponent<Text>().color = new Vector4(0.58f, 0.04f, 0.04f, 255);
                    }

                    KillStreak.GetComponent<Text>().text = "RAMPAGE!!!";
                    aminatorKillStreak.Play("KillStreak");
                    number = (int)Mathf.Round(100 + row * 10 * 3f);
                    Debug.Log("Combox5");
                    timer.value += 3;

                    Rampage++;

                    rampageCounter++;
                }
                combo++;
            }
        }
        else                     // (Mega KIll, Monster KIll) и тд.
        {
            rampageCounter = 0;

            if (KillStreak.GetComponent<Text>().font != StandartFont)
            {
                KillStreak.GetComponent<Text>().font = StandartFont;
                KillStreak.GetComponent<Text>().color = new Vector4(0.2f, 0.2f, 0.2f, 255);
            }
            combo = 0;
            firstBlood = false;

            if (row == 2)
            {
                if (soundSettings != 0)
                {
                    if (rand % 10 == 0)
                        audiosource.PlayOneShot(spreef);
                    else
                        audiosource.PlayOneShot(spreem);
                }
                KillStreak.GetComponent<Text>().text = "Killing Spree";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("KillingSpree!");
            }
            else if (row == 3)
            {
                if (soundSettings != 0)
                {
                    if (rand % 10 == 0)
                        audiosource.PlayOneShot(domf);
                    else
                        audiosource.PlayOneShot(domm);
                }
                KillStreak.GetComponent<Text>().text = "Dominating";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("Dominating!");
            }
            else if (row == 4)
            {
                if (soundSettings != 0)
                    audiosource.PlayOneShot(megam);

                KillStreak.GetComponent<Text>().text = "Mega Kill";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("Mega Kill!");
            }
            else if (row == 5)
            {
                if (soundSettings != 0)
                {
                    if (rand % 10 == 0)
                        audiosource.PlayOneShot(unstopf);
                    else
                        audiosource.PlayOneShot(unstopm);
                }
                KillStreak.GetComponent<Text>().text = "Unstoppable";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("Unstoppable!");
            }
            else if (row == 6)
            {
                if (soundSettings != 0)
                {
                    if (rand % 10 == 0)
                        audiosource.PlayOneShot(wickedf);
                    else
                        audiosource.PlayOneShot(wickedm);
                }
                KillStreak.GetComponent<Text>().text = "Wicked Sick";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("Wicked Sick!");
            }
            else if (row == 7)
            {
                if (soundSettings != 0)
                {
                    if (rand % 10 == 0)
                        audiosource.PlayOneShot(monsterf);
                    else
                        audiosource.PlayOneShot(monsterm);
                }
                KillStreak.GetComponent<Text>().text = "Monster Kill";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("Monster Kill!");
            }
            else if (row == 8)
            {
                if (soundSettings != 0)
                {
                    if (rand % 10 == 0)
                    {

                        audiosource.PlayOneShot(godf);
                        if (Social.localUser.authenticated)
                        {
                            if (PlayerPrefs.GetInt(achName[39]) != 1)
                            {
                                Social.ReportProgress("CgkIu8LriKAEEAIQFQ", 100.0f, (bool success) =>
                                {
                                    if (success)
                                    {
                                        PlayerPrefs.SetInt(achName[39], 1);
                                        AchievementAnimation(26);

                                        PlayerPrefs.SetInt("LastAchImg", 26);
                                        PlayerPrefs.SetString("LastAchString", achName[39]);
                                    }
                                });
                            }
                        }
                    }
                    else
                        audiosource.PlayOneShot(godm);
                }

                KillStreak.GetComponent<Text>().text = "GodLike";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("GodLike!");
            }
            else if (row > 9)
            {
                KillStreak.GetComponent<Text>().text = "beyond GODLIKE";
                aminatorKillStreak.Play("KillStreak");
                Debug.Log("beyond GODLIKE");
            }
        }

        if (!buttonScoreDivide)
            score += number;
        else
            score += number / 2;

        row++;
        timer.value += 3;

        if (row > RowMax)
            RowMax = row;

        Row.text = "IN ROW: " + row.ToString();
        if (!buttonScoreDivide)
            scoreUp.GetComponent<Text>().text = number.ToString();
        else
            scoreUp.GetComponent<Text>().text = (number / 2).ToString();
    }

    private void GameOver()
    {
        timer.value -= Time.deltaTime;

        if ((timer.value <= 0 && !gameOver) || (itemsHerousCount == GameMode && !gameOver))
        {
            if (Social.localUser.authenticated)
                userName.text = Social.localUser.userName;
            else
                userName.text = "Please sign in";
            ContVideoBtn.GetComponent<Image>().sprite = DefaultContVidBtn;
            ContPenaltyBtn.GetComponent<Image>().sprite = DefaultInActiveBtn;

            Debug.Log("Game Over");

            for (int i = 0; i < 5; i++)
                timerParticles.GetComponentsInChildren<ParticleSystem>()[i].Stop();

            RowText.text = "max row: " + RowMax.ToString();
            DoubleKillText.text = "double kill: " + DoubleKill.ToString();
            TripleKillText.text = "triple kill: " + TripleKill.ToString();
            UltraKillText.text = "ultra kill: " + UltraKill.ToString();
            RampageText.text = "rampage: " + Rampage.ToString();

            if (PlayerPrefs.HasKey("LastAchImg"))
            {
                GameOverAchImg.sprite = achieventSprites[PlayerPrefs.GetInt("LastAchImg")];
                GameOverAchString.text = PlayerPrefs.GetString("LastAchString");
            }

            LvlExpSave();
            
            if (PlayerPrefs.GetInt("GameMode") == 2)
                MMR();

            PlayerPrefs.Save();

            LvlText.text = "LVL " + lastLvl.ToString();
            lvlSprite.sprite = lvlSprites[lastLvl - 1];

            HIntButton.interactable = false;

            StartCoroutine(GameOverAnimation());
            gameOver = true;

        }
    }
    public IEnumerator LoadUserAvatar()
    {
        float trying = 10f;
        float counter = 0.2f;
        Texture2D image = null;

        while (trying > 0)      // попытки загрузить аватарку юзера
        {
            image = Social.localUser.image;
            if (image != null)
            {
                avatarka.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
                break;
            }
            trying -= counter;
            yield return new WaitForSeconds(counter);
        }
    }
    private IEnumerator GameOverAnimation()
    {
        audiosource.Stop();

        if (soundSettings != 0)
        {
            audiosource.PlayOneShot(questcomplite);
        }

        yield return new WaitForSeconds(1.2f);

        animatorScore.Play("Score");

        yield return new WaitForSeconds(1f);

        GameOverMenu.SetActive(true);
        StartCoroutine(Moving());
        animatorGameOverPanel.Play("GameOverPanel");
        if (musicSettings != 0)
            audiosource.PlayOneShot(endGame, .5f);

        yield return new WaitForSeconds(.7f);


        float upLvl = experience.value + (score - lastScore) / 10;

        if (lastLvl < 148)
        {
            LevelLightParticle_1.maxParticles = 5;

            while (experience.value < upLvl)
            {
                if ((experience.value + 30) < upLvl - 1000)
                    experience.value += 30;
                else if ((experience.value + 10) < upLvl - 500)
                    experience.value += 10;
                else if ((experience.value + 5) < upLvl - 25)
                    experience.value += 5;
                else
                    experience.value++;

                if (experience.value == 1000)
                {
                    experience.value = 0;
                    upLvl -= 1000;
                    lastLvl++;
                    LvlText.text = "LVL " + lastLvl.ToString();
                    lvlSprite.sprite = lvlSprites[lastLvl - 1];
                }

                yield return new WaitForSeconds(0.01f);
            }

            LevelLightParticle_1.loop = false;
        }

    }

    public void HintButtonF()
    {
        hintFlag = true;

        if (!gameOver)
        {
            combo = -1;
            firstBlood = false;
            audiosource.PlayOneShot(hintParticlesSound);

            for (int i = 0; i < hint.Length; i++)
            {
                HintParticles[hint[i]].SetActive(true);
            }

            HIntButton.interactable = false;
        }
    }

    private void HintActive()
    {
        if (!HIntButton.interactable)
        {
            if (soundSettings != 0)
                audiosource.PlayOneShot(hintSound);

            HintButtonParticle.Play();
            HIntButton.interactable = true;
        }
    }

    public void ContinuePenalty()
    {
        if (interstitial.IsLoaded() && !PenaltyVideoShowed)
        {
            interstitial.Show();
            PenaltyVideoShowed = true;
            ContPenaltyBtn.GetComponent<Image>().sprite = DefaultActiveBtn;
        }
        else if(PenaltyVideoShowed)
        {
            ContPenaltyBtn.GetComponent<Button>().interactable = false;
            buttonScoreDivide = true;
            ContinueRules();

            if (Social.localUser.authenticated)
            {
                if (PlayerPrefs.GetInt(achName[33]) != 1)
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQEw", 100.0f, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(achName[33], 1);
                            AchievementAnimation(14);

                            PlayerPrefs.SetInt("LastAchImg", 14);
                            PlayerPrefs.SetString("LastAchString", achName[33]);

                            PlayerPrefs.Save();
                        }
                    });
                }
            }
        }
    }
    public void ContinueVideo()
    {
        if (!VideoIsShowed)
        {
            rewardBasedVideo.Show();
        }            
        else
        {
            ContVideoBtn.GetComponent<Button>().interactable = false;
            ContinueRules();

            if (Social.localUser.authenticated)
            {
                if (PlayerPrefs.GetInt(achName[32]) != 1)
                {
                    Social.ReportProgress("CgkIu8LriKAEEAIQEg", 100.0f, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(achName[32], 1);
                            AchievementAnimation(46);

                            PlayerPrefs.SetInt("LastAchImg", 46);
                            PlayerPrefs.SetString("LastAchString", achName[32]);

                            PlayerPrefs.Save();
                        }
                    });
                }
            }
        }        
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("Restart", 1);

        if (gameCounter % 5 == 0 && interstitialRetry.IsLoaded())
            interstitialRetry.Show();

        SceneManager.LoadScene(3);
    }
    private void ContinueRules()
    {
        gameOver = false;
        GameOverMenu.SetActive(false);
        timer.value = 15f;

        if (musicSettings != 0)
        {
            audiosource.Stop();
            audiosource.PlayOneShot(music);
        }

        lastScore = score;

        bool mmrAvarageFlag = true;

        if (PlayerPrefs.HasKey("avarageDone"))
            PlayerPrefs.SetInt("avarageDone", lastMMR);
        else
        {
            for (int i = 8; i >= 0 && mmrAvarageFlag; i--)
            {
                if (PlayerPrefs.HasKey(mmrAvarage[i]))
                {
                    PlayerPrefs.DeleteKey(mmrAvarage[i]);
                    mmrAvarageFlag = false;
                }
            }
        }


        for (int i = 0; i < 5; i++)
            timerParticles.GetComponentsInChildren<ParticleSystem>()[i].Play();

        HIntButton.interactable = true;
    }

    public void notExit()
    {
        if (!GameOverMenu.activeSelf)
        {
            if (!exitPanel.activeSelf)
            {
                exitPanel.SetActive(true);
                animatorExitPanel.Play("ExitMenu");
            }

            else
                StartCoroutine(Moving());
        }
        else
            exit();
    }

    private IEnumerator Moving()
    {
        animatorExitPanel.Play("ExitMenu2");
        yield return new WaitForSeconds(0.333F);
        exitPanel.SetActive(false);
    }

    public void exit()
    {
        if (gameCounter % 5 == 0 && interstitialRetry.IsLoaded())
            interstitialRetry.Show();

        SceneManager.LoadScene(2);
    }

    private void AchievementAnimation(int spriteNumber)
    {
        //achievementSprite.sprite = achieventSprites[spriteNumber];
        //achievementAnimator.Play("Achievement");
    }

    private void LvlExpSave()
    {
        int expa;
        int fakeLvl;

        if (PlayerPrefs.HasKey("experience"))
            expa = PlayerPrefs.GetInt("experience");
        else
            expa = 0;

        lastLvl = expa / 1000 + 1;

        fakeLvl = ((expa + ((score - lastScore) / 10)) / 1000) + 1;

        if (fakeLvl != 148)
            experience.value = expa % 1000;
        else
            experience.value = 1000;

        if (expa / 1000 <= 148)
        {
            int tmp = expa + ((score - lastScore) / 10);

            Social.ReportScore(tmp, ExperienceBoard, succes =>
            {
                if (succes)
                    Debug.Log("experience Succes");
                else
                    Debug.Log("experience Error");
            });

            PlayerPrefs.SetInt("experience", expa + ((score - lastScore) / 10));
        }
    }

    private void MMR()
    {
        if (!PlayerPrefs.HasKey("avarageDone"))
        {
            bool avarageAvailable = true;

            for (int i = 0; i < 9 && avarageAvailable; i++)
            {
                if (!PlayerPrefs.HasKey(mmrAvarage[i]))
                {
                    PlayerPrefs.SetInt(mmrAvarage[i], score);
                    if (i != 9)
                        avarageAvailable = false;
                    mmrText.text = "Calibration: " + (9 - i).ToString() + " games remaining";
                }
            }

            if (avarageAvailable)
                PlayerPrefs.SetInt("avarageDone", -1);
        }

        else
        {
            int mmrScore;
            mmrScore = PlayerPrefs.GetInt("avarageDone");

            if (mmrScore == -1)
            {
                int avarage = 0;

                for (int i = 0; i < 9; i++)
                {
                    avarage += PlayerPrefs.GetInt(mmrAvarage[i]);
                }

                mmrScore = avarage / 9 / 10;

                mmrText.text = "MMR: " + mmrScore.ToString();
            }

            else
            {
                int scoreDivision = score / 10;
                int pts;

                if (scoreDivision == 0)
                    scoreDivision = 1;

                if (mmrScore == 0)
                    mmrScore = 1;

                if (mmrScore < scoreDivision)
                {
                    int mmrPlus = (scoreDivision - mmrScore) / mmrScore * 50;

                    if (mmrPlus > 30)
                        pts = 30;
                    else if (mmrPlus < 22)
                        pts = 22;
                    else
                        pts = mmrPlus;

                    mmrScore += pts;

                    mmrText.color = new Vector4(0.04f, 0.605f, 0.058f, 1f);
                    mmrText.text = "MMR: " + mmrScore.ToString() + " (+" + pts.ToString() + ")";
                }

                else
                {
                    int mmrMinus = (mmrScore - scoreDivision) / scoreDivision * 50;

                    if (mmrMinus > 30)
                        pts = 30;
                    else if (mmrMinus < 22)
                        pts = 22;
                    else
                        pts = mmrMinus;

                    mmrScore -= pts;

                    if (mmrScore < 0)
                        mmrScore = 0;

                    mmrText.color = new Vector4(0.78f, 0f, 0f, 1f);
                    mmrText.text = "MMR: " + mmrScore.ToString() + " (-" + pts.ToString() + ")";
                }
            }

            PlayerPrefs.SetInt("avarageDone", mmrScore);

            if (Social.localUser.authenticated)
            {
                // заносим эмэмэрский в таблицу лидеров
                string url = "http://sapereaude7e1.webutu.com/loadscores.php?state=";
                WWW www = new WWW(url + 100 + "&userid=" + Social.localUser.id + "&mmr=" + mmrScore);
            }
        }

        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, LeaderBoard, success =>
            {
                if (success) Debug.Log("Good, yo score: " + score);
                else Debug.Log("failed report yo score progress!");
            });
        }
    }

    private void OnApplicationQuit()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

}

[Serializable]
public class From2List
{
    [HideInInspector]
    public bool available = true;
    public Sprite question;
    public Sprite[] answers;
}
[Serializable]
public class From3List
{
    [HideInInspector]
    public bool available = true;
    public Sprite question;
    public Sprite[] answers;
}
[Serializable]
public class From4List
{
    [HideInInspector]
    public bool available = true;
    public Sprite question;
    public Sprite[] answers;
}
[Serializable]
public class Heroes
{
    [HideInInspector]
    public bool available;
    public Sprite question;
    public Sprite[] answers;
}
