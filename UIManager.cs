using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public StructureInfoPanel[] structureInfoPanels;

    public TextMeshProUGUI pointsInfoText;

    //SECTION - points level UI related
    public Slider levelSlider;
    public TextMeshProUGUI currentPointsText;
    public TextMeshProUGUI pointsToNextLevelText;
    public TextMeshProUGUI pointOfPrevLevelText;

    //SECTION - Icons
    public Sprite residentialIcon;
    public Sprite natureIcon;
    public Sprite entertainmentIcon;
    public Sprite industryIcon;

    //SECTION - Structure category
    public TextMeshProUGUI residentialText;
    public TextMeshProUGUI natureText;
    public TextMeshProUGUI entertainmentText;
    public TextMeshProUGUI industryText;

    public Button residentialButton;
    public Button natureButton;
    public Button entertainmentButton;
    public Button industryButton;

    //SECTION - Sound setting
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    //SECTION - UI Show/Hide
    public GameObject soundButtonObject;
    public GameObject structureCategories;
    public GameObject scoreUI;
    public GameObject infoPanel;
    public GameObject creditsTextObject;
    public GameObject titleTextObject;
    public GameObject playButtonObject;
    public GameObject highScoreTextObject;
    public GameObject gameEndPanelObject;

    //SECTION - End gamePanel
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameEndScoreText;

    //SECTION - New structures Animation
    public GameObject[] newStructureCategoriesButtonObjects;
    public TextMeshProUGUI[] newStructureCategoriesText;
    public GameObject newStructuresObject;
    public GameObject newStructuresHeadingObject;

    private int residentialStructures;
    private int natureStructures;
    private int entertainmentStructures;
    private int industryStructures;

    //SECTION - Controls info panel
    public GameObject controlsPanel;
    public GameObject controlsButtonObject;

    private bool firstTime = true;

    //SECTION - Tutorial related
    public string[] tutorialSteps;
    public Vector2[] tutorialWindowPositions;
    public GameObject helpButtonObject;
    public GameObject tutorialWindow;
    public int tutorialStepIndex;
    public TextMeshProUGUI tutorialText;

    public bool inTutorial = false;
    public bool tutorialFirstTime = true;

    public bool isShowingMenu = true;
    public bool isShowingGameEnd = false;


    void Awake(){
        if(instance == null){
            instance = this;
        }else if(instance != null){
            Destroy(gameObject);
        }
    }

    void Start(){
        firstTime = true;
        inTutorial = false;
        tutorialFirstTime = true;

        GameplayToMenuUI();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.S)){
            if(inTutorial){
                Debug.Log("Tutorial Next");
                TutorialNextClicked();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!isShowingGameEnd && GameManager.instance.gameInPorgress){
                if(isShowingMenu){
                    MenuToGameplayUI();
                }else{
                    GameplayToMenuUI();
                }
            }
        }
    }

    public void PlayButtonClicked(){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
        }
        GameManager.instance.StartGame();
    }

    public void MenuToGameplayUI(){
        //DONE - Complete
        LoadUpdateLevelPointsUI(false, false);

        infoPanel.SetActive(false);

        LeanTween.scale(soundButtonObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(structureCategories, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(scoreUI, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(infoPanel, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(controlsButtonObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        
        LeanTween.scale(creditsTextObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);     //IMP
        LeanTween.scale(titleTextObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(playButtonObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(highScoreTextObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(gameEndPanelObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(controlsPanel, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);

        GameManager.instance.gameplayState = GAMEPLAY_STATE.PLACEMENT;

        isShowingMenu = false;

        if(tutorialFirstTime){
            StartTutorial();
            tutorialFirstTime = false;
        }else{
            LeanTween.scale(helpButtonObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
            LeanTween.scale(tutorialWindow, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        }
    }

    public void GameplayToMenuUI(){
        //DONE - Complete

        if(firstTime){
            Debug.Log("First time");
            ControlsButtonClicked();
            firstTime = false;
        }else{
            Debug.Log("Not first time");
            ControlsCloseButtonClicked();
        }

        LeanTween.scale(soundButtonObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);

        LeanTween.scale(structureCategories, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(scoreUI, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(infoPanel, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        
        LeanTween.scale(creditsTextObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);     //IMP
        LeanTween.scale(titleTextObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(playButtonObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(highScoreTextObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        
        LeanTween.scale(gameEndPanelObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);

        highScoreTextObject.GetComponent<TextMeshProUGUI>().text = "High Score\n" + GameManager.instance.highScore;

        GameManager.instance.gameplayState = GAMEPLAY_STATE.MenuORUI;

        isShowingMenu = true;

        LeanTween.scale(tutorialWindow, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(helpButtonObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
    }

    public void LoadGameEndPanel(){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("LevelUp");
        }
        LeanTween.scale(structureCategories, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(scoreUI, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(infoPanel, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        
        LeanTween.scale(gameEndPanelObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);

        if(!GameManager.instance.gameWon){
            gameOverText.text = "Game Over Try Again!!!";
        }else{
            gameOverText.text = "Game Won!!!";
        }

        gameEndScoreText.text = "" + GameManager.instance.currentGameInfo.points;

        isShowingGameEnd = true;
    }

    public void GameEndOkClicked(){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
        }

        GameManager.instance.EndGame();
        GameplayToMenuUI();

        isShowingGameEnd = false;
    }

    public void LoadUpdateSelectedStructureIconInfo(Vector3 position, NearStructureScore[] nearStructureScores, bool showScoreIndication){
        infoPanel.SetActive(true);
        LoadStructureInfo(position, nearStructureScores, showScoreIndication);
    }

    public void LoadStructureInfoWithoutIndication(NearStructureScore[] nearStructureScores){
        infoPanel.SetActive(true);
        pointsInfoText.text = "" + StructureManager.instance.currentStructureSelected.points;
        
        //DONE - Complete this
        int i = 0;
        for(i = 0; i < nearStructureScores.Length; i++){
            structureInfoPanels[i].Activate(true);
            structureInfoPanels[i].Load(nearStructureScores[i].structureCategory, nearStructureScores[i].points, false);
        }

        if(i < structureInfoPanels.Length){
            for(int j = i; j < structureInfoPanels.Length; j++){
                structureInfoPanels[j].Activate(false);
            }
        }
    }

    private void LoadStructureInfo(Vector3 position, NearStructureScore[] nearStructureScores, bool showScoreIndication){
        pointsInfoText.text = "" + StructureManager.instance.currentStructureSelected.points + " Points";

        List<STRUCTURE_CATEGORY> nearStructureCategories = new List<STRUCTURE_CATEGORY>(); 
        if(showScoreIndication){
            nearStructureCategories = GameManager.instance.GetNearStructureCategories(position);
        }else{
            nearStructureCategories = new List<STRUCTURE_CATEGORY>();  //IMP - Called from ChooseNextStrcuture
        }
        
        //DONE - Complete this
        int i = 0;
        for(i = 0; i < nearStructureScores.Length; i++){
            structureInfoPanels[i].Activate(true);
            if(nearStructureCategories.Contains(nearStructureScores[i].structureCategory)){
                structureInfoPanels[i].Load(nearStructureScores[i].structureCategory, nearStructureScores[i].points, true);
            }else{
                structureInfoPanels[i].Load(nearStructureScores[i].structureCategory, nearStructureScores[i].points, false);
            }
        }

        if(i < structureInfoPanels.Length){
            for(int j = i; j < structureInfoPanels.Length; j++){
                structureInfoPanels[j].Activate(false);
            }
        }
    }

    public void LoadUpdateLevelPointsUI(bool isLevelUp, bool isUndo){
        currentPointsText.text = "" + GameManager.instance.currentGameInfo.points;
        pointsToNextLevelText.text = "" + GameManager.instance.currentGameInfo.pointsToNextLevel;
        pointOfPrevLevelText.text = "" + GameManager.instance.currentGameInfo.pointsOfPreviousLevel;

        levelSlider.value = ((float) (GameManager.instance.currentGameInfo.points - GameManager.instance.currentGameInfo.pointsOfPreviousLevel))
                            / ((float) (GameManager.instance.currentGameInfo.pointsToNextLevel - GameManager.instance.currentGameInfo.pointsOfPreviousLevel));
    
        LoadStructureCategoryUI();
    }

    public void StructureCategoryClicked(int index){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
        }
        STRUCTURE_CATEGORY structureCategory = STRUCTURE_CATEGORY.RESIDENTIAL;

        if(index == 0){
            structureCategory = STRUCTURE_CATEGORY.RESIDENTIAL;
            residentialButton.gameObject.GetComponent<OptionHover>().isClicked = true;
            
            natureButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            natureButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            entertainmentButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            entertainmentButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            industryButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            industryButton.gameObject.GetComponent<OptionHover>().BulgeExit();
        }else if(index == 1){
            structureCategory = STRUCTURE_CATEGORY.NATURE;
            residentialButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            residentialButton.gameObject.GetComponent<OptionHover>().BulgeExit();

            natureButton.gameObject.GetComponent<OptionHover>().isClicked = true;

            entertainmentButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            entertainmentButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            industryButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            industryButton.gameObject.GetComponent<OptionHover>().BulgeExit();
        }else if(index == 2){
            structureCategory = STRUCTURE_CATEGORY.ENTERTAINMENT;
            residentialButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            residentialButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            natureButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            natureButton.gameObject.GetComponent<OptionHover>().BulgeExit();

            entertainmentButton.gameObject.GetComponent<OptionHover>().isClicked = true;

            industryButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            industryButton.gameObject.GetComponent<OptionHover>().BulgeExit();
        }else if(index == 3){
            structureCategory = STRUCTURE_CATEGORY.INDUSTRY;
            residentialButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            residentialButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            natureButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            natureButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            entertainmentButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            entertainmentButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            
            industryButton.gameObject.GetComponent<OptionHover>().isClicked = true;
        }

        StructureManager.instance.StructureCategorySelected(structureCategory);
    }

    public void ClearAllCategoryOutlines(){
        residentialButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            residentialButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            natureButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            natureButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            entertainmentButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            entertainmentButton.gameObject.GetComponent<OptionHover>().BulgeExit();
            industryButton.gameObject.GetComponent<OptionHover>().isClicked = false;
            industryButton.gameObject.GetComponent<OptionHover>().BulgeExit();
    }

    public void LoadStructureCategoryUI(){
        residentialText.text = "" + GameManager.instance.currentGameInfo.residentialStructuresAvailable;
        natureText.text = "" + GameManager.instance.currentGameInfo.natureStructuresAvailable;
        entertainmentText.text = "" + GameManager.instance.currentGameInfo.entertainmentStructuresAvailable;
        industryText.text = "" + GameManager.instance.currentGameInfo.industryStructuresAvailable;

        if(GameManager.instance.currentGameInfo.residentialStructuresAvailable <= 0){
            residentialButton.interactable = false;
        }else{
            residentialButton.interactable = true;
        }
         
        if(GameManager.instance.currentGameInfo.natureStructuresAvailable <= 0){
            natureButton.interactable = false;
        }else{
            natureButton.interactable = true;
        } 
        
        if(GameManager.instance.currentGameInfo.entertainmentStructuresAvailable <= 0){
            entertainmentButton.interactable = false;
        }else{
            entertainmentButton.interactable = true;
        }
        
        if(GameManager.instance.currentGameInfo.industryStructuresAvailable <= 0){
            industryButton.interactable = false;
        }else{
            industryButton.interactable = true;
        }
    }

    public void SoundOnOffClicked(){
        if(GameManager.instance.soundOn){
            GameManager.instance.soundOn = false;
        }else{
            GameManager.instance.soundOn = true;
        }

        //TODO - Change icon
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().UnmuteMusic();
            soundButtonObject.GetComponent<Image>().sprite = soundOnSprite;
        }else{
            FindObjectOfType<AudioManager>().MuteMusic();
            soundButtonObject.GetComponent<Image>().sprite = soundOffSprite;
        }

        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
            FindObjectOfType<AudioManager>().Play("Theme");
        }else{
            FindObjectOfType<AudioManager>().StopOne("Theme");
        }
    }

    public void HideInofPanel(){
        infoPanel.SetActive(false);
    }

    public void ShowNewStructures(int residential, int nature, int entertainment, int industry){
        //DONE - Sound effect
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("LevelUp");
        }

        Debug.Log("New Structures " + residential + " "+ nature + " " + entertainment + " " + industry);

        if(residential >= 0){
            newStructureCategoriesButtonObjects[0].gameObject.SetActive(true);
            newStructureCategoriesText[0].text = "+" + residential;
        }else{
            newStructureCategoriesButtonObjects[0].gameObject.SetActive(false);
        }

        if(nature >= 0){
            newStructureCategoriesButtonObjects[1].gameObject.SetActive(true);
            newStructureCategoriesText[1].text = "+" + nature;
        }else{
            newStructureCategoriesButtonObjects[1].gameObject.SetActive(false);
        }

        if(entertainment >= 0){
            newStructureCategoriesButtonObjects[2].gameObject.SetActive(true);
            newStructureCategoriesText[2].text = "+" + entertainment;
        }else{
            newStructureCategoriesButtonObjects[2].gameObject.SetActive(false);
        }

        if(industry >= 0){
            newStructureCategoriesButtonObjects[3].gameObject.SetActive(true);
            newStructureCategoriesText[3].text = "+" + industry;
        }else{
            newStructureCategoriesButtonObjects[3].gameObject.SetActive(false);
        }

        this.residentialStructures = residential;
        this.natureStructures = nature;
        this.entertainmentStructures = entertainment;
        this.industryStructures = industry;

        //DONE - Animation
        LeanTween.scale(newStructuresHeadingObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack).setOnComplete(HideNewStructuresUI);
        LeanTween.scale(newStructuresObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
    }

    public void HideNewStructuresUI(){
        LeanTween.scale(newStructuresHeadingObject, new Vector3(0f, 0f, 0f), 0.4f).setDelay(1f).setEase(LeanTweenType.easeInBack).setOnComplete(UpdateNumberOfStructuresAvaliable);
        LeanTween.scale(newStructuresObject, new Vector3(0f, 0f, 0f), 0.4f).setDelay(1f).setEase(LeanTweenType.easeInBack);
    }

    public void UpdateNumberOfStructuresAvaliable(){
        GameManager.instance.currentGameInfo.residentialStructuresAvailable += residentialStructures;
        GameManager.instance.currentGameInfo.natureStructuresAvailable += natureStructures;
        GameManager.instance.currentGameInfo.entertainmentStructuresAvailable += entertainmentStructures;
        GameManager.instance.currentGameInfo.industryStructuresAvailable += industryStructures;

        GameManager.instance.CheckWinLoseCondition();

        LoadStructureCategoryUI();
    }

    public void ControlsButtonClicked(){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
        }
        LeanTween.scale(controlsButtonObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(controlsPanel, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
    }

    public void ControlsCloseButtonClicked(){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
        }
        LeanTween.scale(controlsPanel, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(controlsButtonObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
    }

    public void StartTutorial(){
        //DONE - Scale down help
        tutorialStepIndex = 0;
        inTutorial = true;

        LeanTween.scale(tutorialWindow, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(helpButtonObject, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);

        LoadTutorialStepUI();
    }

    public void TutorialNextClicked(){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
        }

        if(tutorialStepIndex >= tutorialSteps.Length - 1){
            EndTutorial();
            return;
        }

        tutorialStepIndex++;

        if(tutorialStepIndex < tutorialSteps.Length){
            LoadTutorialStepUI();
        }        
    }

    void LoadTutorialStepUI(){
        tutorialText.text = tutorialSteps[tutorialStepIndex];
        LeanTween.move(tutorialWindow.GetComponent<RectTransform>(), new Vector3(tutorialWindowPositions[tutorialStepIndex].x,tutorialWindowPositions[tutorialStepIndex].y,0f), 0.3f);
    }

    public void HelpClicked(){
        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Click");
        }
        StartTutorial();
    }

    public void EndTutorial(){
        inTutorial = false;
        LeanTween.scale(helpButtonObject, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(tutorialWindow, new Vector3(0f, 0f, 0f), 0.6f).setEase(LeanTweenType.easeInBack);
    }
}
