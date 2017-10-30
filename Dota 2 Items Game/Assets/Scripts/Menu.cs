using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    public GameObject ItemMenuPanel;    // панель с контентом
    public GameObject BodyPanel;
    public GameObject MainScreenPanel;

    public GameObject SettingsPanel;    // вкладка с настройками
    public GameObject SettingsButton;   // кнопка лайка
    public GameObject HeroPanel;        // вкладка с героями
    public GameObject ItemsInfoPannel;  // вкладка с айтемами
    public GameObject AchievementPanel; // вкладка с ачивками
    public GameObject[] heroButtons;    // кнопки с героями на силу, ловк, инт
    private Color defButtColor;         // дефолтный цвет кнопок

    public SpriteRenderer[] SettingsButtons;
    public Sprite[] SettingsSprite;
    public Sprite LikeSprite;

    #region Heroes

    public GameObject[] midBtt;         // кнопки которые пропадают
    public GameObject[] bottomButtons;  // нижние 4 кнопки

    public RectTransform resizePanel;   // панель с кнопками героев 

    public Text[] heroButt;             // разделы с героями (str, agi, int)
    public Button[] ChooseHeroBtn;      // кнопки выбора героя

    public HeroInformation[] HeroInfo;  // массив героев

    public ScrollRect HeroScroll;       // автоскрол инфы вверх

    public GameObject HeroInfoHeader;
    public Text HeroName;
    public Image heroPortrait;
    public Text[] HeroHpMp;
    public Text[] HeroAttributes;
    public Text HeroRole;
    public Text[] HeroTalents;

    public HeroSkillObj[] HeroSkillObjects;

    #endregion

    #region Items

    // кнопки и панели с предметами
    public GameObject BasicItemButton;
    public GameObject UpgradeItemButton;
    public GameObject BasicItemPanel;
    public GameObject UpgradeItemPanel;


    public BasicItems[] itemsInfo;      // айтемы
    public ScrollRect scrollRect;       // автоскрол вверх

    public Button[] buttons;            // кнопки с предметами
    private string oldButton = "";      // имя предыдуще нажатой кнопки

    public GameObject[] footerBtnsWhere;// кнопки которые используют спрайт в сборке
    public GameObject footerButtPanel;  // панель с этими копками
    public GameObject ArrowWherePannel; // панель стрелок
    public Sprite[] ArrowsWhere;
    public GameObject[] footerBtnsFrom; // кнопки из которых собирается спрайт
    public GameObject footButtFromPan;  // панель с этими копками
    public GameObject ArrowFromePannel; // панель стрелок
    public Sprite[] ArrowsFrom;
    public GameObject CurrImg;          // текущий рисунок (вкл) если он учавствует в сборках

    public Image iImg;                  // Рисунок выделенного предмета
    public Text iName;                  // имя выделенного предмета
    public Text iPrice;                 // стоимость выделенного предмета
    public GameObject header;           // шапка 
    public GameObject footer;           // низ
    public GameObject[] footerElem;     // елементы кулдауна и манакоста
    public GameObject iAddInfoTxt;
    private Text iAddInfo;              // описание выделенного предмета
    public Text iCooldown;
    public Text iManacost;

    private Animator animatorItemMenuPanel;

    #endregion

    #region Achievements

    public string[] achievementName;
    public string[] achievementInfo;
    public Image[] achievementImage;
    public Slider[] achievementSlider;
    public Text[] achievementCounter;
    public Sprite[] achievementSprite;
    public Text[] achievementHeadText;
    public Text[] achievementBottomText;

    #endregion

    #region InternetCheck

    private const bool allowCarrierDataNetwork = false;
    private const string pingAddress = "8.8.8.8"; // Google Public DNS server
    private const float waitingTime = 5f;
    private bool internetConnectBool;
    private Ping ping;
    private float pingStartTime;
    bool CheckDone = true;
    bool HardwareCheck;

    #endregion

    #region Chat

    public Text inputMessage;
    public Text resivedText;
    string db_url = "https://insuppressible-lieu.000webhostapp.com/backend.php";
    string chat_msg = "";
    string userName;
    string last_timestamp = "0";
    WWW comet_www;

    #endregion

    public GameObject PlayPanel;
    public GameObject PlayEnablBtn;
    public GameObject PlayBtnDefault;
    public Animator PlayPanelAnim;

    public Text OwnRaiting;         // ммр юзера на панели профиля
    public GameObject UserName;
    public GameObject LogIn;
    public Image UsrAvatar;
    public Image AccountAvatar;
    public Text AccountNickName;
    private Texture2D image;    // аватарка юзера которую будем загружать

    // Таблицы лидеров
    public Text TopPlayerTab;
    public Text TopPlayerScoreTab;
    public Text HighPlayerTab;
    public Text HighPlayerMmrTab;
    public Text UserPosInPlayerTab;     // позиция игрока в таблице лидеров
    public Text UserMmrPosition;
    int userPos;

    public ToggleButton[] ToggleButPan;     // выдвижные панели на кнопках и спрайты для них
    public Sprite ActiveToggleBtn, InactiveTogBtn;
    // порядковый номер нажатых кнопок 
    private int oldBtnIterator;         // он же индекс режима игры

    private AudioSource audioSrc;
    public AudioClip ClickButton, HideButton;
    public AudioClip OpenPlayPan, ClosePlayPan, playBtnHit, childItemHit;
    public AudioClip MenuMusic;
    float buttonAlpha = 0;

    int musicSettings;
    int soundSettings;
    // int vibrationSettings;
    public Slider ExperienceSlider;
    public Text ExperienceLvl;
    public SpriteRenderer ExperienceSprite;
    public Sprite[] ExperienceSprites;

    public Button RankedGame;
    public Button HeartButton;

    /* ссылка-запрос таблицы лидеров
    * - СОСТОЯНИЯ -
	*	100 - юзер отправляет данные
    *	101 - юзер загружает данные
    *	102 - юзер получает своё место в таблице лидеров
    *	103 - узнаем существует ли юзер(на случай удаления игры)
    */
    string url = "http://sapereaude7e1.webutu.com/loadscores.php?state=";
    ILeaderboard lb;

    private void Start()
    {
        image = null;

        UserPosInPlayerTab.text = "";

        defButtColor = new Vector4(0.4627f, 0.5569f, 0.5529f, 1f);
        iAddInfo = iAddInfoTxt.GetComponent<Text>();
        animatorItemMenuPanel = ItemMenuPanel.GetComponentInChildren<Animator>();
        audioSrc = GetComponent<AudioSource>();

        // загружаем настройки
        LoadSettings();

        // загружаем рисунки в кнопки
        LoadBasicItemImages();
        LoadUpgradeItemImages();

        userAuthNot();
        // загружаем ачивки
        LoadAchievements();

        #region InternetCheck

        ping = new Ping(pingAddress);
        pingStartTime = Time.time;
        CheckDone = false;
        HardwareCheck = false;

        #endregion
    }

    #region Load achievements from GPG
    public void LoadAchivementsFromGPG()
    {
        if (!PlayerPrefs.HasKey("First One's Free"))
        {
            Social.LoadAchievements(achievements =>
            {
                if (achievements.Length > 0)
                {
                    foreach (IAchievement achievement in achievements)
                    {
                        switch(achievement.id)
                        {
                            case "CgkIu8LriKAEEAIQBQ":
                                PlayerPrefs.SetInt(achievementName[0], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQBg":
                                PlayerPrefs.SetInt(achievementName[1], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQBw":
                                PlayerPrefs.SetInt(achievementName[2], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQCA":
                                PlayerPrefs.SetInt(achievementName[3], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQCQ":
                                PlayerPrefs.SetInt(achievementName[4], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQCg":
                                PlayerPrefs.SetInt(achievementName[5], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQCw":
                                PlayerPrefs.SetInt(achievementName[6], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQDA":
                                PlayerPrefs.SetInt(achievementName[7], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQDQ":
                                PlayerPrefs.SetInt(achievementName[8], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQDg":
                                PlayerPrefs.SetInt(achievementName[9], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQDw":
                                PlayerPrefs.SetInt(achievementName[10], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQEA":
                                PlayerPrefs.SetInt(achievementName[11], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQEQ":  // clean sweep
                                PlayerPrefs.SetInt(achievementName[39], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQEg":
                                PlayerPrefs.SetInt(achievementName[12], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQEw":
                                PlayerPrefs.SetInt(achievementName[13], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQFA":
                                PlayerPrefs.SetInt(achievementName[14], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQFQ":
                                PlayerPrefs.SetInt(achievementName[15], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQFg":
                                PlayerPrefs.SetInt(achievementName[16], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQFw":
                                PlayerPrefs.SetInt(achievementName[17], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQGA":
                                PlayerPrefs.SetInt(achievementName[18], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQGQ":
                                PlayerPrefs.SetInt(achievementName[19], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQGg":
                                PlayerPrefs.SetInt(achievementName[20], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQGw":
                                PlayerPrefs.SetInt(achievementName[21], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQHA":
                                PlayerPrefs.SetInt(achievementName[22], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQHQ":
                                PlayerPrefs.SetInt(achievementName[23], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQHg":
                                PlayerPrefs.SetInt(achievementName[24], (int)((achievement.percentCompleted * 10d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQHw":
                                PlayerPrefs.SetInt(achievementName[25], (int)((achievement.percentCompleted * 50d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQIA":
                                PlayerPrefs.SetInt(achievementName[26], (int)((achievement.percentCompleted * 100d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQIQ":
                                PlayerPrefs.SetInt(achievementName[27], (int)((achievement.percentCompleted * 1000d)/ 100d));
                                break;
                            case "CgkIu8LriKAEEAIQIg":
                                PlayerPrefs.SetInt(achievementName[28], (int)((achievement.percentCompleted * 10000d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQIw":
                                PlayerPrefs.SetInt(achievementName[29], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQJA":
                                PlayerPrefs.SetInt(achievementName[30], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQJQ":
                                PlayerPrefs.SetInt(achievementName[31], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQJg":
                                PlayerPrefs.SetInt(achievementName[32], (int)(achievement.percentCompleted / 100));
                                break;
                            case "CgkIu8LriKAEEAIQJw":
                                PlayerPrefs.SetInt(achievementName[33], (int)((achievement.percentCompleted * 100000d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQKA":
                                PlayerPrefs.SetInt(achievementName[34], (int)((achievement.percentCompleted * 500000d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQKQ":
                                PlayerPrefs.SetInt(achievementName[35], (int)((achievement.percentCompleted * 1000000d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQKg":
                                PlayerPrefs.SetInt(achievementName[36], (int)((achievement.percentCompleted * 5000000d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQKw":
                                PlayerPrefs.SetInt("itemsCount", (int)((achievement.percentCompleted * 83d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQLA":  // finish him
                                PlayerPrefs.SetInt("heroesCount", (int)((achievement.percentCompleted * 113d) / 100d));
                                break;
                            case "CgkIu8LriKAEEAIQLQ":
                                PlayerPrefs.SetFloat(achievementName[40], (float)(achievement.percentCompleted * 3600) / 100);
                                break;
                            case "CgkIu8LriKAEEAIQLg":
                                PlayerPrefs.SetFloat(achievementName[41], (float)(achievement.percentCompleted * 18000) / 100);
                                break;
                            case "CgkIu8LriKAEEAIQLw":
                                PlayerPrefs.SetFloat(achievementName[42], (float)(achievement.percentCompleted * 36000) / 100);
                                break;
                            case "CgkIu8LriKAEEAIQMA":
                                PlayerPrefs.SetFloat(achievementName[43], (float)(achievement.percentCompleted * 180000) / 100);
                                break;
                            default:
                                break;
                        }
                    }
                    LoadAchievements();
                }
                else
                    Debug.Log("No achievements returned");
            });
            
        }
    }
    #endregion

    private void Update()
    {
        if (!CheckDone)
            CheckInternetConnection();

        if (!audioSrc.isPlaying && musicSettings == 1)
            audioSrc.PlayOneShot(MenuMusic);
        // SignIn();

        //if (ItemsInfoPannel.activeSelf && buttonAlpha <= 1)
        //    buttonsApears();
        //if (HeroPanel.activeSelf && buttonAlpha <= 1)
        //    HeroesButtonsApear();
    }

    #region GUI

    public void PlayGame()
    {
        if (oldBtnIterator == 0)
        {
            PlayerPrefs.SetInt("GameMode", 0);
            SceneManager.LoadScene(3);
            // play items
        }
        if (oldBtnIterator == 1)
        {
            PlayerPrefs.SetInt("GameMode", 1);
            SceneManager.LoadScene(3);
            // play heroes
        }
        if (oldBtnIterator == 2)        // play mmr
        {
            PlayerPrefs.SetInt("GameMode", 2);
            SceneManager.LoadScene(3);
        }
    }

    public void StartGame() // выдвинуть панель с квопой
    {
        oldBtnIterator = 5;
        ItemMenuPanel.GetComponent<Canvas>().sortingOrder = 3;
        MainScreenPanel.GetComponent<Canvas>().sortingOrder = 3;
        StartCoroutine(PlayEnablBtnOn());

        for (var i = 0; i < 3; i++)
        {
            ToggleButPan[i].ToggleBtnImg.sprite = InactiveTogBtn;
            ToggleButPan[i].ToggleObj.SetActive(false);
        }

        PlayPanel.SetActive(true);
        PlayPanelAnim.SetLayerWeight(1, 0);
        PlayPanelAnim.Play("Play");

        audioSrc.PlayOneShot(OpenPlayPan);
    }

    private IEnumerator PlayHidePanel()     // спрятать панель с квопой
    {
        PlayPanelAnim.SetLayerWeight(1, 0);

        audioSrc.PlayOneShot(ClosePlayPan);

        if (oldBtnIterator < 5)
        {
            ToggleButPan[oldBtnIterator].ToggleBtnImg.sprite = InactiveTogBtn;
        }
        for (var i = 0; i < 3; i++)
        {
            ToggleButPan[i].ToggleBtnImg.sprite = InactiveTogBtn;
            ToggleButPan[i].ToggleObj.SetActive(false);
        }
        // возвращаем кнопке плей дефолтное состояние
        PlayBtnDefault.SetActive(true);
        PlayEnablBtn.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0f);
        PlayEnablBtn.SetActive(false);

        PlayPanelAnim.Play("PlayBack");
        yield return new WaitForSeconds(.3f);
        ItemMenuPanel.GetComponent<Canvas>().sortingOrder = 5;
        MainScreenPanel.GetComponent<Canvas>().sortingOrder = 5;
        PlayPanel.SetActive(false);
    }

    IEnumerator PlayEnablBtnOn()    // делаем плей активной и перекрашиваем
    {
        PlayEnablBtn.SetActive(true);
        while (PlayEnablBtn.GetComponent<Image>().color.a != 1f)
        {
            yield return new WaitForSeconds(0.02f);
            PlayEnablBtn.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, PlayEnablBtn.GetComponent<Image>().color.a + 0.05f);
        }
        PlayBtnDefault.SetActive(false);
    }

    public void PlayItems()
    {
        PlayPanelAnim.Play("ItemsTrening");
        Debug.Log("Playing");
    }

    public void ShowPannel(string buttonName)
    {
        if (PlayPanel.activeSelf)
            StartCoroutine(PlayHidePanel());

        if (oldButton.Length > 0 && oldButton != "Settings")    // выкл подсветку предыдущей кнопки
            GameObject.Find(oldButton).GetComponent<Image>().enabled = false;

        GameObject.Find(buttonName).GetComponent<Image>().enabled = true;   // подсвечиваем текущую кнопку
        if (soundSettings != 0)
            audioSrc.PlayOneShot(ClickButton);
        ItemMenuPanel.SetActive(true);
        BodyPanel.SetActive(true);
        MainScreenPanel.SetActive(false);
        animatorItemMenuPanel.Play("Panel");
        oldButton = buttonName;             // запоминаем последне нажатую кнопку

        bool ifSettings = false;
        bool ifHeroes = false;
        bool ifItems = false;
        bool ifAchievement = false;

        if (buttonName == "Items")
        {
            ifItems = true;
            ifAchievement = false;
            ifHeroes = false;

            UpgradeItemButton.GetComponentInChildren<Text>().color = defButtColor;
            BasicItemButton.GetComponentInChildren<Text>().color = Color.white;

            OnItemClick(0); // показать первый в списке предмет
        }
        else if (buttonName == "Achivmt")
        {
            ifItems = false;
            ifAchievement = true;
            ifHeroes = false;
        }
        else if (buttonName == "Heroes")
        {
            ifItems = false;
            ifAchievement = false;
            ifHeroes = true;
            ShowHeroes(0, 37);

            heroButt[0].color = Color.white;
            for (var i = 1; i < 3; i++)
                heroButt[i].color = defButtColor;

            GameObject btn = new GameObject();
            btn.name = "0";
            OnHeroButtonClick(btn);    // показать первого героя
            Destroy(btn);
        }
        else if (buttonName == "Settings")
        {
            ifItems = false;
            ifAchievement = false;
            ifHeroes = false;
            ifSettings = true;
        }

        UpgradeItemPanel.SetActive(false);
        BasicItemPanel.SetActive(ifItems);
        ItemsInfoPannel.SetActive(ifItems);
        SettingsPanel.SetActive(ifSettings);
        AchievementPanel.SetActive(ifAchievement);

        SettingsButton.SetActive(ifSettings);
        BasicItemButton.SetActive(ifItems);
        UpgradeItemButton.SetActive(ifItems);

        HeroPanel.SetActive(ifHeroes);
        for (var i = 0; i < 3; i++)
            heroButtons[i].SetActive(ifHeroes);

        buttonAlpha = 0;
    }

    public void HidePannel()        // прячет все панели (аля выход на рабочий стол)
    {
        StartCoroutine(PlayHidePanel());

        if (oldButton.Length > 0 && oldButton != "Settings")
            GameObject.Find(oldButton).GetComponent<Image>().enabled = false;

        if (ItemMenuPanel.activeSelf)
        {
            if (soundSettings != 0)
                audioSrc.PlayOneShot(HideButton);

            animatorItemMenuPanel.Play("hidePannel");
            StartCoroutine(hideWaiter());
            BodyPanel.SetActive(false);
            MainScreenPanel.SetActive(true);
        }
    }
    IEnumerator hideWaiter()
    {
        yield return new WaitForSeconds(.3f);
        ItemMenuPanel.SetActive(false);
    }

    public void ToggleTabs(int iterator)        // кнопки с режимом выбора игры
    {
        PlayPanelAnim.SetLayerWeight(1, 1);
        if (oldBtnIterator != iterator)
            StartCoroutine(Toggle(iterator));

        for (var i = 0; i < 3; i++)
            ToggleButPan[i].ToggleBtnImg.sprite = (i == iterator) ? ActiveToggleBtn : InactiveTogBtn;
    }
    private IEnumerator Toggle(int i)
    {
        audioSrc.PlayOneShot(playBtnHit);
        ToggleButPan[i].ToggleObj.SetActive(true);
        Debug.Log(oldBtnIterator + " nomerKnopki: " + i);
        PlayPanelAnim.Play("toggle_bottom" + i);
        if (oldBtnIterator < 5) PlayPanelAnim.Play("toggle_back" + oldBtnIterator);
        yield return new WaitForSeconds(.3f);
        oldBtnIterator = i;
    }

    #endregion

    #region Settings

    public void Settings(string name)
    {
        int num;

        if (name == "Sound")
            num = 0;
        else if (name == "Music")
            num = 1;
        else
            num = 2;

        if (PlayerPrefs.GetInt(name) == 0)
        {
            PlayerPrefs.SetInt(name, 1);
            SettingsButtons[num].sprite = SettingsSprite[1];
            if (name == "Sound")
            {
                soundSettings = 1;
                audioSrc.PlayOneShot(ClickButton);
            }
            if (name == "Vibration")
                Handheld.Vibrate();

            if (name == "Music")
            {
                audioSrc.PlayOneShot(MenuMusic);
                musicSettings = 1;
            }
        }
        else
        {
            PlayerPrefs.SetInt(name, 0);
            SettingsButtons[num].sprite = SettingsSprite[0];
            if (name == "Sound")
                soundSettings = 0;
            if (name == "Music")
            {
                audioSrc.Stop();
                musicSettings = 0;
            }
        }

        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 1);
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("Vibration"))
            PlayerPrefs.SetInt("Vibration", 1);

        for (int i = 0; i < 3; i++)
            if (PlayerPrefs.GetInt(SettingsButtons[i].name) == 0)
                SettingsButtons[i].sprite = SettingsSprite[0];
            else
                SettingsButtons[i].sprite = SettingsSprite[1];

        soundSettings = PlayerPrefs.GetInt("Sound");
        musicSettings = PlayerPrefs.GetInt("Music");

        if (PlayerPrefs.GetInt("Music") != 0)
            audioSrc.PlayOneShot(MenuMusic);
    }

    #endregion

    #region Items

    public void ShowBasicItems()
    {
        audioSrc.PlayOneShot(childItemHit);
        UpgradeItemButton.GetComponentInChildren<Text>().color = defButtColor;
        BasicItemButton.GetComponentInChildren<Text>().color = Color.white;
        UpgradeItemPanel.SetActive(false);
        BasicItemPanel.SetActive(true);
    }

    public void ShowUpgradeItems()
    {
        audioSrc.PlayOneShot(childItemHit);
        BasicItemPanel.SetActive(false);
        UpgradeItemPanel.SetActive(true);
        BasicItemButton.GetComponentInChildren<Text>().color = defButtColor;
        UpgradeItemButton.GetComponentInChildren<Text>().color = Color.white;
    }

    public void OnItemClick(int btnId)
    {
        header.SetActive(false);
        iImg.sprite = itemsInfo[btnId].image;
        iName.text = itemsInfo[btnId].Name;
        iPrice.text = itemsInfo[btnId].Price.ToString();

        iAddInfo.text = "";
        if (itemsInfo[btnId].AddInfo.Length > 0)
            iAddInfo.text += itemsInfo[btnId].AddInfo + "\n\n";
        if (itemsInfo[btnId].Notes.Length > 0)
            iAddInfo.text += itemsInfo[btnId].Notes + "\n\n";

        // Добавляем прибавки к атрибутам
        if (itemsInfo[btnId].Stats.Length > 0)
            for (var i = 0; i < itemsInfo[btnId].Stats.Length; i++)
            {
                iAddInfo.text += itemsInfo[btnId].Stats[i] + "\n";
            }
        iAddInfo.text += "\n";
        // Добавляем бонусы
        if (itemsInfo[btnId].Bonus.Length > 0)
            for (var i = 0; i < itemsInfo[btnId].Bonus.Length; i++)
            {
                iAddInfo.text += itemsInfo[btnId].Bonus[i] + "\n";
            }

        #region Манакост и Кулдаун
        footer.SetActive((itemsInfo[btnId].Cooldown != 0 || itemsInfo[btnId].Manacost != 0) ? true : false);
        // если есть кулдаун
        if (itemsInfo[btnId].Cooldown != 0)
        {
            footerElem[0].SetActive(true);
            footerElem[1].SetActive(true);
            iCooldown.text = itemsInfo[btnId].Cooldown.ToString();
        }
        else
        {
            footerElem[0].SetActive(false);
            footerElem[1].SetActive(false);
        }
        // если есть манакост
        if (itemsInfo[btnId].Manacost != 0)
        {
            footerElem[2].SetActive(true);
            footerElem[3].SetActive(true);
            iManacost.text = itemsInfo[btnId].Manacost.ToString();
        }
        else
        {
            footerElem[2].SetActive(false);
            footerElem[3].SetActive(false);
        }
        #endregion     

        if (itemsInfo[btnId].buttonsWhere.Length > 0)   // если текущий премет учавствует в сборках
        {
            int itemLen = itemsInfo[btnId].buttonsWhere.Length;

            footerButtPanel.SetActive(true);
            ArrowWherePannel.SetActive(true);
            ArrowWherePannel.GetComponent<Image>().sprite = ArrowsWhere[itemLen - 1];

            for (var i = 0; i < itemLen; i++)
            {
                footerBtnsWhere[i].SetActive(true);
                footerBtnsWhere[i].GetComponent<Button>().onClick = itemsInfo[btnId].buttonsWhere[i].onClick;
                footerBtnsWhere[i].GetComponent<Image>().sprite = itemsInfo[btnId].buttonsWhere[i].GetComponent<Image>().sprite;
            }
            for (var i = itemLen; i < 6; i++)
            {
                footerBtnsWhere[i].SetActive(false);
            }

        }
        else
        {
            footerButtPanel.SetActive(false);
            ArrowWherePannel.SetActive(false);
        }
        /* делаем активным центральный рисунок */
        if (itemsInfo[btnId].buttonsWhere.Length > 0 || itemsInfo[btnId].buttonsFrom.Length > 0)
        {
            CurrImg.SetActive(true);
            CurrImg.GetComponent<Image>().sprite = iImg.sprite;
        }
        else
            CurrImg.SetActive(false);

        if (itemsInfo[btnId].buttonsFrom.Length > 0)    // если текущий премет собирается из чего-либо
        {
            ArrowFromePannel.SetActive(true);
            ArrowFromePannel.GetComponent<Image>().sprite = ArrowsFrom[itemsInfo[btnId].buttonsFrom.Length - 1];
            footButtFromPan.SetActive(true);

            Debug.Log(itemsInfo[btnId].buttonsFrom.Length);
            for (var i = 0; i < itemsInfo[btnId].buttonsFrom.Length; i++)
            {
                footerBtnsFrom[i].SetActive(true);
                footerBtnsFrom[i].GetComponent<Image>().sprite = itemsInfo[btnId].buttonsFrom[i].GetComponent<Image>().sprite;
                footerBtnsFrom[i].GetComponent<Button>().onClick = itemsInfo[btnId].buttonsFrom[i].onClick;
            }
            for (var i = itemsInfo[btnId].buttonsFrom.Length; i < 6; i++)
            {
                footerBtnsFrom[i].SetActive(false);
            }
        }
        else
        {
            ArrowFromePannel.SetActive(false);
            footButtFromPan.SetActive(false);
        }

        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(.001f);
        header.SetActive(true);
        //yield return new WaitForSeconds(.001f);
        //scrollRect.verticalNormalizedPosition = 1f;
    }

    private void LoadBasicItemImages()
    {
        for (var i = 0; i < 65; i++)
        {
            buttons[i].GetComponent<Image>().sprite = itemsInfo[i].image;
        }
    }
    private void LoadUpgradeItemImages()
    {
        for (var i = 65; i < 148; i++)
        {
            buttons[i].GetComponent<Image>().sprite = itemsInfo[i].image;
        }
    }

    private void buttonsApears()
    {
        ColorBlock buttonColor;
        for (int i = 0; i < 5; i++)
        {
            buttonAlpha += 1.5f / 255f;
            buttonColor = buttons[i].colors;
            buttonColor.normalColor = new Vector4(1, 1, 1, buttonAlpha);
            buttons[i].colors = buttonColor;
        }

        for (int i = 14; i < 19; i++)
        {
            buttonAlpha += 1.5f / 255f;
            buttonColor = buttons[i].colors;
            buttonColor.normalColor = new Vector4(1, 1, 1, buttonAlpha);
            buttons[i].colors = buttonColor;
        }

        for (int i = 25; i < 30; i++)
        {
            buttonAlpha += 1.5f / 255f;
            buttonColor = buttons[i].colors;
            buttonColor.normalColor = new Vector4(1, 1, 1, buttonAlpha);
            buttons[i].colors = buttonColor;
        }

        for (int i = 39; i < 44; i++)
        {
            buttonAlpha += 1.5f / 255f;
            buttonColor = buttons[i].colors;
            buttonColor.normalColor = new Vector4(1, 1, 1, buttonAlpha);
            buttons[i].colors = buttonColor;
        }

        for (int i = 53; i < 58; i++)
        {
            buttonAlpha += 1.5f / 255f;
            buttonColor = buttons[i].colors;
            buttonColor.normalColor = new Vector4(1, 1, 1, buttonAlpha);
            buttons[i].colors = buttonColor;
        }
    }

    #endregion

    #region Achievements

    private void LoadAchievements()
    {
        for (int i = 0; i < 23; i++)
        {
            if (PlayerPrefs.GetInt(achievementName[i]) == 1)
            {
                achievementHeadText[i].text = achievementName[i];
                achievementBottomText[i].text = achievementInfo[i];
                achievementImage[i].sprite = achievementSprite[i];
            }

        }

        if (PlayerPrefs.GetInt(achievementName[23]) == 1)
        {
            achievementHeadText[23].text = achievementName[23];
            achievementBottomText[23].text = achievementInfo[23];
            achievementImage[23].sprite = achievementSprite[23];

            HeartButton.interactable = false;
            HeartButton.GetComponent<Image>().sprite = LikeSprite;
        }

        if (PlayerPrefs.GetInt(achievementName[24]) == 10)
            achievementImage[24].sprite = achievementSprite[24];
        achievementSlider[0].value = PlayerPrefs.GetInt(achievementName[24]);
        achievementCounter[0].text = achievementSlider[0].value.ToString() + "/10";

        if (PlayerPrefs.GetInt(achievementName[25]) == 50)
            achievementImage[25].sprite = achievementSprite[25];
        achievementSlider[1].value = PlayerPrefs.GetInt(achievementName[25]);
        achievementCounter[1].text = achievementSlider[1].value.ToString() + "/50";

        if (PlayerPrefs.GetInt(achievementName[26]) == 100)
            achievementImage[26].sprite = achievementSprite[26];
        achievementSlider[2].value = PlayerPrefs.GetInt(achievementName[26]);
        achievementCounter[2].text = achievementSlider[2].value.ToString() + "/100";

        if (PlayerPrefs.GetInt(achievementName[27]) == 1000)
            achievementImage[27].sprite = achievementSprite[27];
        achievementSlider[3].value = PlayerPrefs.GetInt(achievementName[27]);
        achievementCounter[3].text = achievementSlider[3].value.ToString() + "/1000";

        if (PlayerPrefs.GetInt(achievementName[28]) == 10000)
            achievementImage[28].sprite = achievementSprite[28];
        achievementSlider[4].value = PlayerPrefs.GetInt(achievementName[28]);
        achievementCounter[4].text = achievementSlider[4].value.ToString() + "/10000";


        for (int i = 29; i < 33; i++)
        {
            if (PlayerPrefs.GetInt(achievementName[i]) == 1)
                achievementImage[i].sprite = achievementSprite[i];
        }

        if (PlayerPrefs.GetInt(achievementName[33]) == 100000)
        {
            achievementImage[33].sprite = achievementSprite[33];
            achievementSlider[5].value = 100000;
            achievementCounter[5].text = "100000/100000";
        }
        else
        {
            achievementSlider[5].value = PlayerPrefs.GetInt(achievementName[36]);
            achievementCounter[5].text = achievementSlider[5].value.ToString() + "/100000";
        }
        if (PlayerPrefs.GetInt(achievementName[34]) == 500000)
        {
            achievementImage[34].sprite = achievementSprite[34];
            achievementSlider[6].value = 500000;
            achievementCounter[6].text = "500000/500000";
        }
        else
        {
            achievementSlider[6].value = PlayerPrefs.GetInt(achievementName[36]);
            achievementCounter[6].text = achievementSlider[6].value.ToString() + "/500000";
        }
        if (PlayerPrefs.GetInt(achievementName[35]) == 1000000)
        {
            achievementImage[35].sprite = achievementSprite[35];
            achievementSlider[7].value = 1000000;
            achievementCounter[7].text = "1000000/1000000";
        }
        else
        {
            achievementSlider[7].value = PlayerPrefs.GetInt(achievementName[36]);
            achievementCounter[7].text = achievementSlider[7].value.ToString() + "/1000000";
        }
        if (PlayerPrefs.GetInt(achievementName[36]) == 5000000)
            achievementImage[36].sprite = achievementSprite[36];
        achievementSlider[8].value = PlayerPrefs.GetInt(achievementName[36]);
        achievementCounter[8].text = achievementSlider[8].value.ToString() + "/5000000";
        if (PlayerPrefs.GetInt(achievementName[37]) == 1)
            achievementImage[37].sprite = achievementSprite[37];
        if (PlayerPrefs.HasKey("itemsCount"))
        {
            achievementSlider[9].value = PlayerPrefs.GetInt("itemsCount");
            achievementCounter[9].text = achievementSlider[9].value.ToString() + "/83";

        }
        if (PlayerPrefs.GetInt(achievementName[38]) == 1)
            achievementImage[38].sprite = achievementSprite[38];
        if (PlayerPrefs.HasKey("heroesCount"))
        {
            achievementSlider[10].value = PlayerPrefs.GetInt("heroesCount");
            achievementCounter[10].text = achievementSlider[10].value.ToString() + "/113";
        }
        if (PlayerPrefs.GetInt(achievementName[39]) == 1)
        {
            achievementImage[39].sprite = achievementSprite[39];
            achievementHeadText[39].text = achievementName[39];
            achievementBottomText[39].text = achievementInfo[39];
        }

        if (PlayerPrefs.GetFloat(achievementName[40]) == 3600)
        {
            achievementImage[40].sprite = achievementSprite[40];
            achievementSlider[11].value = 60;
            achievementCounter[11].text = "60/60";
        }
        else
        {
            if (PlayerPrefs.HasKey(achievementName[43]))
            {
                achievementSlider[11].value = (int)(PlayerPrefs.GetFloat(achievementName[43]) / 60);
                achievementCounter[11].text = achievementSlider[11].value.ToString() + "/60";
            }
        }
        if (PlayerPrefs.GetFloat(achievementName[41]) == 18000)
        {
            achievementImage[41].sprite = achievementSprite[41];
            achievementSlider[12].value = 300;
            achievementCounter[12].text = "300/300";
        }
        else
        {
            if (PlayerPrefs.HasKey(achievementName[43]))
            {
                achievementSlider[12].value = (int)(PlayerPrefs.GetFloat(achievementName[43]) / 60);
                achievementCounter[12].text = achievementSlider[12].value.ToString() + "/300";
            }
        }
        if (PlayerPrefs.GetFloat(achievementName[42]) == 36000)
        {
            achievementImage[42].sprite = achievementSprite[42];
            achievementSlider[13].value = 600;
            achievementCounter[13].text = "600/600";
        }
        else
        {
            if (PlayerPrefs.HasKey(achievementName[43]))
            {
                achievementSlider[13].value = (int)(PlayerPrefs.GetFloat(achievementName[43])/60);
                achievementCounter[13].text = achievementSlider[13].value.ToString() + "/600";
            }
        }
        if (PlayerPrefs.GetFloat(achievementName[43]) == 180000)
        {
            achievementImage[43].sprite = achievementSprite[43];
            achievementSlider[14].value = 3000;
            achievementCounter[14].text = "3000/3000";
        }
        else
        {
            if (PlayerPrefs.HasKey(achievementName[43]))
            {
                achievementSlider[14].value = (int)(PlayerPrefs.GetFloat(achievementName[43])/60);
                achievementCounter[14].text = achievementSlider[14].value.ToString() + "/3000";
            }
        }


    }

    public void Heart()
    {
        if (Social.localUser.authenticated)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.sapereaude.d2quiz");

            Social.ReportProgress("CgkIu8LriKAEEAIQHQ", 100.0f, (bool success) =>
            {
                if (success)
                {
                    PlayerPrefs.SetInt(achievementName[23], 1);
                    PlayerPrefs.SetString("LastAchString", achievementName[23]);

                    achievementHeadText[23].text = achievementName[23];
                    achievementBottomText[23].text = achievementInfo[23];
                    achievementImage[23].sprite = achievementSprite[23];

                    HeartButton.interactable = false;
                    HeartButton.GetComponent<Image>().sprite = LikeSprite;

                    PlayerPrefs.Save();
                }
            });
        }
    }

    #endregion

    #region Heroes

    public void ButtonHeroes(int k)
    {
        audioSrc.PlayOneShot(childItemHit);
        // размер массива героев выбранного параметра
        int size = (k == 0) ? 37 : (k == 1) ? 73 : 113;
        //первый грой в массиве
        int j = (k == 0) ? 0 : (k == 1) ? 37 : 73;

        for (var i = 0; i < 3; i++)
        {
            if (i == k)
            {
                heroButt[k].color = Color.white;
                ShowHeroes(j, size);
            }
            else
                heroButt[i].color = defButtColor;
        }
    }

    private void ShowHeroes(int j, int size)
    {
        for (int i = j, n = 0; i < size; i++, n++)
        {
            if (i == 21 || i == 56)
            {
                n += (i == 21) ? 3 : 5;
                int arr = (i == 21) ? 3 : 5;
                for (var m = 0; m < arr; m++)
                {
                    midBtt[m].SetActive(false);
                }
                for (var m = arr; m < midBtt.Length; m++)
                {
                    midBtt[m].SetActive(true);
                }
                if (i == 56)
                {
                    bottomButtons[0].SetActive(true);
                    for (var m = 1; m < 4; m++)
                        bottomButtons[m].SetActive(false);

                    resizePanel.offsetMax = new Vector2(0, 0);
                    resizePanel.offsetMin = new Vector2(0, -713f);
                }
                else
                {
                    for (var m = 0; m < 4; m++)
                        bottomButtons[m].SetActive(false);

                    resizePanel.offsetMax = new Vector2(0, 0);
                    resizePanel.offsetMin = new Vector2(0, -592f);
                }

            }
            else if (i == 93)
            {
                resizePanel.offsetMax = new Vector2(0, 0);
                resizePanel.offsetMin = new Vector2(0, -713f);
                for (var m = 0; m < 4; m++)
                    bottomButtons[m].SetActive(true);

                for (var m = 0; m < 5; m++)
                {
                    if (m != 3)
                        midBtt[m].SetActive(false);
                    else
                        midBtt[m].SetActive(true);
                }
                n += 4;
            }

            ChooseHeroBtn[n].GetComponent<Image>().sprite = HeroInfo[i].btnImage;
            ChooseHeroBtn[n].name = i.ToString();
        }
    }

    public void OnHeroButtonClick(GameObject button)
    {
        int i = Convert.ToInt32(button.name);
        HeroName.text = HeroInfo[i].heroName;
        heroPortrait.sprite = HeroInfo[i].faceImage;
        HeroRole.text = HeroInfo[i].role;

        for (int n = 0; n < 8; n++)
        {
            if (n < 2)
                HeroHpMp[n].text = HeroInfo[i].HpMp[n].ToString();

            if (n < 6)
                HeroAttributes[n].text = HeroInfo[i].attributes[n].ToString();

            HeroTalents[n].text = HeroInfo[i].talents[n].ToString();
        }

        int arrLen = HeroInfo[i].skills.Length;

        for (var zero = 0; zero < 14; zero++)       // откл ненужные позиции для скиллов
        {
            HeroSkillObjects[zero].skillObj.SetActive(false);
        }

        for (var k = 0; k < arrLen; k++)        // заполняем позиции для скиллов
        {
            HeroSkillObjects[k].skillParams.skillObjName.text = HeroInfo[i].skills[k].sName;
            HeroSkillObjects[k].skillParams.skillObjImg.sprite = HeroInfo[i].skills[k].sImage;

            HeroSkillObjects[k].skillParams.skillObjAttr.GetComponent<Text>().text = "";
            foreach (var att in HeroInfo[i].skills[k].sAttributes)
            {
                HeroSkillObjects[k].skillParams.skillObjAttr.GetComponent<Text>().text += att + "\n";
            }
            if (HeroInfo[i].skills[k].Description.Length > 0)
            {
                string addParagraph = "";
                if (HeroInfo[i].skills[k].sAttributes.Length < 4)
                    addParagraph = "\n";
                HeroSkillObjects[k].skillParams.skillObjDesc.GetComponent<Text>().text = addParagraph + HeroInfo[i].skills[k].Description + "\n";
                HeroSkillObjects[k].skillParams.skillObjDesc.SetActive(true);
            }
            else
                HeroSkillObjects[k].skillParams.skillObjDesc.SetActive(false);

            if (HeroInfo[i].skills[k].ifAghanim.Length > 0)
            {
                HeroSkillObjects[k].skillParams.skillObjAghanim.SetActive(true);
                HeroSkillObjects[k].skillParams.skillObjAghanim.GetComponent<Text>().text = HeroInfo[i].skills[k].ifAghanim + "\n";
            }
            else
                HeroSkillObjects[k].skillParams.skillObjAghanim.SetActive(false);

            if (HeroInfo[i].skills[k].Bonus.Length > 0)
            {
                HeroSkillObjects[k].skillParams.skillObjBonus.SetActive(true);
                HeroSkillObjects[k].skillParams.skillObjBonus.GetComponent<Text>().text = HeroInfo[i].skills[k].Bonus + "\n";
            }
            else
                HeroSkillObjects[k].skillParams.skillObjBonus.SetActive(false);

            if (HeroInfo[i].skills[k].sInfo.Length > 0)
            {
                HeroSkillObjects[k].skillParams.skillObjInfo.SetActive(true);
                HeroSkillObjects[k].skillParams.skillObjInfo.GetComponent<Text>().text = "";
                foreach (var inf in HeroInfo[i].skills[k].sInfo)
                {
                    HeroSkillObjects[k].skillParams.skillObjInfo.GetComponent<Text>().text += inf + "\n";
                }
            }
            HeroSkillObjects[k].skillParams.skillObjInfo.SetActive(false);

            if (HeroInfo[i].skills[k].Cooldown.Length > 0 || HeroInfo[i].skills[k].Manacost.Length > 0)
                HeroSkillObjects[k].skillParams.skillCdMpPanel.SetActive(true);
            else
                HeroSkillObjects[k].skillParams.skillCdMpPanel.SetActive(false);

            HeroSkillObjects[k].skillParams.skillObjCd.GetComponent<Text>().text = HeroInfo[i].skills[k].Cooldown;
            HeroSkillObjects[k].skillParams.skillObjMp.GetComponent<Text>().text = HeroInfo[i].skills[k].Manacost;

            StartCoroutine(HeroWaiter());
            HeroSkillObjects[k].skillObj.SetActive(true);
        }
        // задержка на появление контента ! (без неё контент налазит друг на друга)
        StartCoroutine(HeroWaiter());
    }

    IEnumerator HeroWaiter()
    {
        yield return new WaitForSeconds(0.001f);
        HeroInfoHeader.SetActive(false);
        yield return new WaitForSeconds(0.02f);
        HeroInfoHeader.SetActive(true);
        //yield return new WaitForSeconds(0.02f);
        //HeroScroll.verticalNormalizedPosition = 1f;
    }

    private void HeroesButtonsApear()
    {
        ColorBlock buttonColor;

        for (int i = 0; i < 20; i++)
        {
            buttonAlpha += 2f / 255;
            buttonColor = ChooseHeroBtn[i].colors;
            buttonColor.normalColor = new Vector4(1, 1, 1, buttonAlpha);
            ChooseHeroBtn[i].colors = buttonColor;
        }
    }

    #endregion

    #region InternetCheck

    private void CheckInternetConnection()
    {
        if (!HardwareCheck)
        {
            bool internetPossiblyAvailable;

            switch (Application.internetReachability)
            {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    internetPossiblyAvailable = true;
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    internetPossiblyAvailable = allowCarrierDataNetwork;
                    internetPossiblyAvailable = true;
                    break;
                default:
                    internetPossiblyAvailable = false;
                    break;
            }

            HardwareCheck = true;

            if (!internetPossiblyAvailable)
            {
                InternetIsNotAvailable();
                return;
            }
        }

        bool stopCheck;

        if (ping != null)
        {
            Debug.Log("processing...");

            stopCheck = true;
            if (ping.isDone)
                InternetAvailable();
            else if (Time.time - pingStartTime < waitingTime)
                stopCheck = false;
            else
                InternetIsNotAvailable();
            if (stopCheck)
                ping = null;
        }
    }

    private void InternetIsNotAvailable()
    {
        //Debug.Log("No Internet");

        internetConnectBool = false;
        CheckDone = true;
        Debug.Log("Not Connected");
    }

    private void InternetAvailable()
    {
        //Debug.Log("Internet is available;)");

        internetConnectBool = true;
        CheckDone = true;
        SignIn();
        Debug.Log("Connected");
    }

    #endregion

    #region Authentication

    public void CheckInternetConnectionButton()
    {
        ping = new Ping(pingAddress);
        pingStartTime = Time.time;
        HardwareCheck = false;
        CheckDone = false;
    }

    public void SignIn()
    {
        // Create client configuration
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success)
            {
                userAuth();
            }
        });
    }

    public void userAuth()
    {
        LogIn.SetActive(false);
        UserName.SetActive(true);
        UsrAvatar.enabled = true;
        AccountAvatar.enabled = true;
        UserName.GetComponent<Text>().text = Social.localUser.userName;
        AccountNickName.text = UserName.GetComponent<Text>().text;
        StartCoroutine(LoadUserAvatar());
        RankedGame.interactable = true;

        #region Chat
        userName = UserName.GetComponent<Text>().text;
        comet_www = new WWW(db_url + "?timestamp=" + last_timestamp);
        StartCoroutine(WaitingForResponse(comet_www, ParseResponse));
        #endregion
    }

    private void userAuthNot()
    {
        LogIn.SetActive(true);
        UsrAvatar.enabled = false;
        AccountAvatar.enabled = false;
        UserName.SetActive(false);
    }

    public IEnumerator LoadUserAvatar()
    {
        float trying = 10f;
        float counter = 0.2f;

        while (trying > 0)      // попытки загрузить аватарку юзера
        {
            image = Social.localUser.image;
            if (image != null)
            {
                UsrAvatar.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
                AccountAvatar.sprite = UsrAvatar.sprite;
                break;
            }
            trying -= counter;
            yield return new WaitForSeconds(counter);
        }

        lb = Social.CreateLeaderboard();
        lb.id = "CgkIu8LriKAEEAIQAg";

        HighestScorePlayers();
        LoadMmrLeaders();
        ExperienceLoad();

        UserPosInScoreTab();
        StartCoroutine(UserMmrPosInScoreTab());
        LoadAchivementsFromGPG();
    }
    #endregion

    #region Chat

    public delegate void RequestCallback(string str);

    public void Send()
    {
        if (!LogIn.activeSelf)
        {
            char[] correctMessage = inputMessage.text.ToCharArray();

            for (int i = 0; i < correctMessage.Length; i++)
                if (correctMessage[i] == ' ' || correctMessage[i] == '\x81')
                    correctMessage[i] = '';

            chat_msg = new string(correctMessage);

            if (chat_msg != "")
            {
                StartCoroutine(WaitingForResponse(new WWW(db_url + "?msg=" + chat_msg + "&userName=" + userName), null));
                Debug.Log("Send");
            }
        }
    }

    void OnApplicationQuit()
    {
        if (comet_www != null)
        {
            comet_www.Dispose();
            comet_www = null;
        }

        PlayGamesPlatform.Instance.SignOut();
    }

    public IEnumerator WaitingForResponse(WWW www, RequestCallback callback)
    {

        Debug.Log("" + Time.time);
        yield return www; // ожидаем пока получим с сервера данные
        Debug.Log("" + Time.time);

        if (www.error == null)
        {
            Debug.Log("Successful.");
        }
        else
        {
            Debug.Log("Failed.");
        }

        if (callback != null)
        {
            callback(www.text);
            callback = null;
        }

        //Очищаем данные
        www.Dispose();
        www = null;
    }

    void ParseResponse(string stras)
    {
        if (stras.Contains("{"))
        {
            Chat content = JsonUtility.FromJson<Chat>(stras);
            Debug.Log(content.stroke + " / " + content.timestamp);

            if (content.timestamp != last_timestamp)
            {
                char[] fixMessage = content.stroke.ToCharArray();

                for (int i = 0; i < fixMessage.Length; i++)
                    if (fixMessage[i] == '')
                        fixMessage[i] = ' ';

                last_timestamp = content.timestamp;
                resivedText.text = new string(fixMessage);
            }

            comet_www = new WWW(db_url + "?timestamp=" + last_timestamp);
            StartCoroutine(WaitingForResponse(comet_www, ParseResponse));
        }
        else
        {
            Debug.Log("strange response");
        }
    }

    #endregion

    #region Scores
    public void UserPosInScoreTab()     // Текущая позиция игрока в таблице лидеров
    {
        PlayGamesPlatform.Instance.LoadScores(lb.id, LeaderboardStart.PlayerCentered, 1,
             LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (LeaderboardScoreData data) =>
             {
                 UserPosInPlayerTab.text = "You are on " + data.PlayerScore.rank + " of " + data.ApproximateCount + "!";

             });
    }

    private void ExperienceLoad()
    {
        if (!PlayerPrefs.HasKey("experience"))
        {
            PlayerPrefs.SetInt("experience", 0);

            PlayGamesPlatform.Instance.LoadScores("CgkIu8LriKAEEAIQMQ", LeaderboardStart.PlayerCentered, 1, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (LeaderboardScoreData data) =>
            {
                PlayerPrefs.SetInt("experience", Convert.ToInt32(data.PlayerScore.value));
            });
        }

        else
        {
            int tmp = PlayerPrefs.GetInt("experience");

            ExperienceSlider.value = tmp % 1000;
            ExperienceLvl.text ="LVL " + (tmp / 1000 + 1).ToString();
            ExperienceSprite.sprite = ExperienceSprites[tmp / 1000];
        }
    }
    // Проверяем есть ли такой игрок в базе 
    private IEnumerator UserMmrPosInScoreTab()  // и узнаем его текущую позицию в таблице ммр
    {
        WWW ifUserExist = new WWW(url + 103 + "&userid=" + Social.localUser.id);

        yield return ifUserExist;
        if (ifUserExist.text != "null")
        {
            PlayerPrefs.SetInt("avarageDone", Convert.ToInt32(ifUserExist.text));
            OwnRaiting.text = "MMR: " + PlayerPrefs.GetInt("avarageDone");
        }
        else
        {
            for (int i = 8; i >= 0; i--)
            {
                if (PlayerPrefs.HasKey("mmrAvarage_" + i))
                {
                    OwnRaiting.text = "Calibration: " + (9 - i) + " games remaining";
                    break;
                }
            }

            if (OwnRaiting.text == "")
                OwnRaiting.text = "Calibration: 10 games remaining";
        }

        int mmr = PlayerPrefs.GetInt("avarageDone");
        WWW pos = new WWW(url + 102 + "&comp=" + mmr);

        yield return pos;

        string[] p = pos.text.Split(new Char[] { ' ' });
        UserMmrPosition.text = "You are on " + p[0] + " of " + p[1] + "!";
    }

    public void HighestScorePlayers()     // загрузка топ юзеров
    {
        TopPlayerTab.text = "";
        TopPlayerScoreTab.text = "";

        IScore[] scoresFromLeaderboard = null;

        PlayGamesPlatform.Instance.LoadScores(
                    lb.id,
                    LeaderboardStart.TopScores,
                    10,
                    LeaderboardCollection.Public,
                    LeaderboardTimeSpan.AllTime,
                    (data) =>
                    {
                        scoresFromLeaderboard = data.Scores;

                        List<string> userIds = new List<string>();

                        foreach (IScore score in scoresFromLeaderboard)
                        {
                            TopPlayerScoreTab.text += score.formattedValue + "\n";
                            userIds.Add(score.userID);
                        }
                        Social.LoadUsers(userIds.ToArray(), (users) =>
                        {
                            foreach (var usr in users)
                                TopPlayerTab.text += usr.userName + "\n";
                        });
                    }
        );
    }

    public void LoadMmrLeaders()
    {
        StartCoroutine(LoadLeaderBoard());
    }

    public IEnumerator LoadLeaderBoard()       // загрузка ммр из БД
    {
        HighPlayerTab.text = "";
        HighPlayerMmrTab.text = "";

        WWW www = new WWW(url + 101);
        Debug.Log(www.url);
        yield return www;

        string jsonString = fixJson(www.text);

        Debug.Log(jsonString);

        Player[] player = JsonHelper.FromJson<Player>(jsonString);

        List<string> userIds = new List<string>();

        foreach (var plr in player)
        {
            userIds.Add(plr.userId);
            HighPlayerMmrTab.text += plr.mmr + "\n";
        }
        Social.LoadUsers(userIds.ToArray(), (users) =>
        {
            foreach (var usr in users)
                HighPlayerTab.text += usr.userName + "\n";
        });

        //ExperienceSlider.value = PlayerPrefs.GetInt("experience") % 1000;
        //ExperienceLvl.text = "LVL " + PlayerPrefs.GetInt("experience"); //((PlayerPrefs.GetInt("experience") / 1000 + 1).ToString());
    }

    private string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    #endregion

}

#region Classes

[Serializable]
public class BasicItems
{
    public string Name;
    public Sprite image;
    public int Price;
    public string AddInfo;
    public string Notes;
    public string[] Stats;
    public string[] Bonus;
    public int Cooldown;
    public int Manacost;
    public Button[] buttonsWhere;
    public Button[] buttonsFrom;
}
[Serializable]
public class HeroInformation
{
    public string heroName;
    public Sprite btnImage;
    public Sprite faceImage;
    public string[] attributes;
    public int[] HpMp;
    public string role;
    public string[] talents;
    public Skill[] skills;
}
[Serializable]
public class Skill
{
    public Sprite sImage;
    public string sName;
    public string[] sAttributes;
    public string Description;
    public string ifAghanim;
    public string Bonus;
    public string[] sInfo;
    public string Cooldown;
    public string Manacost;
}
[Serializable]
public class HeroSkillObj
{
    public GameObject skillObj;
    public HeroSkillObjParams skillParams;
}
[Serializable]
public class HeroSkillObjParams
{
    public Text skillObjName;
    public Image skillObjImg;
    public GameObject skillObjAttr;
    public GameObject skillObjDesc;
    public GameObject skillObjAghanim;
    public GameObject skillObjBonus;
    public GameObject skillObjInfo;
    public GameObject skillCdMpPanel;
    public GameObject skillObjCdPanel;
    public GameObject skillObjMdPanel;
    public GameObject skillObjCd;
    public GameObject skillObjMp;
}
[Serializable]
public class Player
{
    public string id;
    public string userId;
    public string mmr;
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
[Serializable]
public class Chat
{
    public string stroke;
    public string timestamp;
}
[Serializable]
public class ToggleButton
{
    public Image ToggleBtnImg;
    public GameObject ToggleObj;
}

#endregion