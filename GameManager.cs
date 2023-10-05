using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameInfo currentGameInfo;

    public GAMEPLAY_STATE gameplayState;

    public int highScore;

    public LayerMask structureLayer;

    //SECTION - Floating text related
    public GameObject floatingTextObjectPrefab;
    public GameObject floatingCanvas;

    public AnimationCurve townLevelPointsCurve;

    public bool gameWon = false;    //IMP - For game end

    //SECTION - Scoring indicators
    public List<GameObject> highlightStructureNormalPool = new List<GameObject>();
    public List<GameObject> highlightStructureScoringPool = new List<GameObject>();

    public int highlightStructureNormalPoolIndex;
    public int highlightStructureScoringPoolIndex;

    private bool hasActivatedIndicator; //IMP PERFORMANCE - to control performance, this flag used

    public bool lastPlacedIsStructure;

    public bool soundOn;

    public bool gameInPorgress;

    void Awake(){
        if(instance == null){
            instance = this;
        }else if(instance != null){
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO - Uncomment this
        UIManager.instance.GameplayToMenuUI();
        highScore = 0;
        soundOn = true;
        gameInPorgress = false;

        GameManager.instance.gameplayState = GAMEPLAY_STATE.MenuORUI;

        //StartGame();    //TEST
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(){
        Reset();
        gameWon = false;
        gameInPorgress = true;

        currentGameInfo = new GameInfo("", 0, 0, 25, 1, 1, 1, 1);

        //IMP - Porfile game in progress
        gameplayState = GAMEPLAY_STATE.PLACEMENT;
        
        currentGameInfo.pointsToNextLevel = GameManager.instance.GetLevelPoints(1);

        Init();
        StructureManager.instance.Init();   //IMP - Place this line after modelsDB init
        
        //TODO - Uncomment this
        UIManager.instance.MenuToGameplayUI();
    }
    
    public void Init(){
        highlightStructureNormalPool = new List<GameObject>();
        highlightStructureScoringPool = new List<GameObject>();

        PoolIndicators();
    }

    public void EndGame(){
        gameInPorgress = false;
        if(currentGameInfo.points >= highScore){
            highScore = currentGameInfo.points;
        }
    }

    public void Reset(){
        StructureManager.instance.Clear();
        //IMP - Game manager related reset is done in  start game function

        highlightStructureNormalPoolIndex = 0;
        highlightStructureScoringPoolIndex = 0;
        
        foreach (GameObject highlightObject in highlightStructureNormalPool)
        {
            Destroy(highlightObject);
        }

        foreach (GameObject highlightObject in highlightStructureScoringPool)
        {
            Destroy(highlightObject);
        }

        highlightStructureNormalPool.Clear();
        highlightStructureScoringPool.Clear();
    }

    //SECTION - Scoring related
    public void HandlePointsLevelUpdate(int points){
        //DONE - Handle scoring Update score and check for level
        currentGameInfo.points += points;
        bool isLevelUp = CheckLevel();

        //TODO - Uncomment this and Handle UI
        UIManager.instance.LoadUpdateLevelPointsUI(isLevelUp, false);
    }

    //SECTION - Level and points system
    private bool CheckLevel(){
        bool levelUp = false;
        while(currentGameInfo.points >= currentGameInfo.pointsToNextLevel){
            Debug.Log("Points "+ currentGameInfo.points);

            currentGameInfo.level++;
            currentGameInfo.pointsOfPreviousLevel = currentGameInfo.pointsToNextLevel;
            currentGameInfo.pointsToNextLevel = GetLevelPoints(currentGameInfo.level + 1);
            GiveLevelUpReward();    //IMP - Always call this function after level++ Order is very important

            Debug.Log("Level Now "+ currentGameInfo.level);
            Debug.Log("Points Of Previous Level "+ currentGameInfo.pointsOfPreviousLevel);
            Debug.Log("Points To Next Level "+ currentGameInfo.pointsToNextLevel + "\n");
            Debug.Log("Structures Left " + currentGameInfo.structuresLeft);

            levelUp = true;
        }

        if(!levelUp){
            CheckWinLoseCondition();
        }

        return levelUp;
    }

    public int GetLevelPoints(int levelNo){
        //NOTE - Default
        return (int) townLevelPointsCurve.Evaluate(levelNo);
    }

    private void GiveLevelUpReward(){
        //TODO - Make this more compliated
        int r = Random.Range(0, 100);
        if(r < 25){
            UIManager.instance.ShowNewStructures(2, 1, 1, 0);
        }else if(r < 50){
            UIManager.instance.ShowNewStructures(0, 1, 1, 2);
        }else if(r < 75){
            UIManager.instance.ShowNewStructures(0, 2, 2, 0);
        }else{
            UIManager.instance.ShowNewStructures(3, 1, 0, 0);
        }
    }

    public List<STRUCTURE_CATEGORY> GetNearStructureCategories(Vector3 pos){
        List<STRUCTURE_CATEGORY> nearStructureCategories = new List<STRUCTURE_CATEGORY>();

        Collider[] hitColliders = Physics.OverlapSphere(pos, Constants.SCORE_RADIUS, structureLayer);
        foreach (var hitCollider in hitColliders)
        {
            if(Vector3.Distance(hitCollider.transform.position, pos) < 2f){
                continue;
            }

            nearStructureCategories.Add(hitCollider.transform.parent.gameObject.GetComponent<StructureScript>().structureCard.structureCategory);
        }

        return nearStructureCategories;
    }

    public int CalculateScore(Vector3 pos, int basePoints, NearStructureScore[] nearStructureScores, bool posChanged){
        Debug.Log("In calculate score");
        int score = basePoints;

        //DONE - Add score logic here
        //DONE IMP - Exclude to place structure
        Collider[] hitColliders = Physics.OverlapSphere(pos, Constants.SCORE_RADIUS, structureLayer);
        Debug.Log("HitCollider detected inc self " + hitColliders.Length);
        foreach (var hitCollider in hitColliders)
        {
            bool isScoring = false;
            Debug.Log("Hit : " + hitCollider.name);
            Debug.Log("Hit Pos: " + hitCollider.transform.position);
            Debug.Log("Pos for sphere : " + pos);
            if(Vector3.Distance(hitCollider.transform.position, pos) < 2f){
                Debug.Log("self detected");
                continue;
            }

            foreach (NearStructureScore nearStructureScore in nearStructureScores)
            {
                if(hitCollider.transform.parent.gameObject.GetComponent<StructureScript>().structureCard.structureCategory == nearStructureScore.structureCategory){
                    score += nearStructureScore.points;
                    isScoring = true;
                    break; 
                }else{
                    isScoring = false;
                }
            }

            if(posChanged){
                if(isScoring){
                    ActivateScoringIndicator(hitCollider.transform.parent.gameObject.GetComponent<StructureScript>().structureCard.yHeight,
                                            hitCollider.transform.parent.transform.position,
                                            hitCollider.transform.parent.transform.rotation);
                }else{
                    ActivateNormalIndicator(hitCollider.transform.parent.gameObject.GetComponent<StructureScript>().structureCard.yHeight,
                                            hitCollider.transform.parent.transform.position,
                                            hitCollider.transform.parent.transform.rotation);
                }
            }
        }

        // foreach (CellType nearCellType in nearCellTypes)
        // {
        //     foreach (NearStructureId nearStructure in nearStructures)
        //     {
        //         if(nearCellType == nearStructure.structureId){
        //             score += nearStructure.points;
        //         }
        //     }
        // }

        return score;
    }

    public void CalculateHandleScore(Vector3 pos, int basePoints, NearStructureScore[] nearStructureScores){
        int score = CalculateScore(pos, basePoints, nearStructureScores, false);
        
        HandlePointsLevelUpdate(score);

        ClearAllIndicators();   //IMP
        
    }

    public void CheckWinLoseCondition(){
        currentGameInfo.structuresLeft--;

        //DONE - Work on win condition
        if(currentGameInfo.structuresLeft <= 0){

            //DONE - Game won
            Debug.Log("Game Won");
            gameWon = true;
            UIManager.instance.LoadGameEndPanel();

            return;
        }

        //DONE IMP - Add this - && !LandManager.instance.isLastRingSegment
        if(currentGameInfo.industryStructuresAvailable == 0 && currentGameInfo.entertainmentStructuresAvailable == 0
            && currentGameInfo.natureStructuresAvailable == 0 && currentGameInfo.residentialStructuresAvailable == 0){
            //DONE - Game lost
            Debug.Log("Game Lost");
            gameWon = false;
            UIManager.instance.LoadGameEndPanel();

            return;
        }

        if(currentGameInfo.structuresLeft > 0){
            //DO NOTHING
        }
    }

    //SECTION - Structure highlight
    private void PoolIndicators(){
        hasActivatedIndicator = false;
        highlightStructureNormalPoolIndex = 0;
        highlightStructureScoringPoolIndex = 0;

        for(int i = 0; i < Constants.HIGHLIGHT_POOL_SIZE; i++){
            GameObject highlightStructureNormal = Instantiate(ModelsDB.instance.highlightStructureNormal);
            highlightStructureNormal.SetActive(false);
            highlightStructureNormalPool.Add(highlightStructureNormal);
        }

        for(int i = 0; i < Constants.HIGHLIGHT_POOL_SIZE; i++){
            GameObject highlightStructureScoring = Instantiate(ModelsDB.instance.highlightStructureScoring);
            highlightStructureScoring.SetActive(false);
            highlightStructureScoringPool.Add(highlightStructureScoring);
        }
    }

    private void ActivateNormalIndicator(float yHeight, Vector3 pos, Quaternion rotation){
        highlightStructureNormalPoolIndex = ++highlightStructureNormalPoolIndex % highlightStructureNormalPool.Count;

        highlightStructureNormalPool[highlightStructureNormalPoolIndex].SetActive(true);

        highlightStructureNormalPool[highlightStructureNormalPoolIndex].transform.position = pos;
        highlightStructureNormalPool[highlightStructureNormalPoolIndex].transform.rotation = rotation;
        highlightStructureNormalPool[highlightStructureNormalPoolIndex].transform.localScale = new Vector3(
                                                                                                highlightStructureNormalPool[highlightStructureNormalPoolIndex].transform.localScale.x,
                                                                                                yHeight,
                                                                                                highlightStructureNormalPool[highlightStructureNormalPoolIndex].transform.localScale.z
                                                                                            );
        hasActivatedIndicator = true;

        // for(int i = 0; i < highlightStructureNormalPool.Count; i++){
        //     if(i != highlightStructureNormalPoolIndex){
        //         highlightStructureNormalPool[i].SetActive(false);
        //     }
        // }
    }

    private void ActivateScoringIndicator(float yHeight, Vector3 pos, Quaternion rotation){
        highlightStructureScoringPoolIndex = ++highlightStructureScoringPoolIndex % highlightStructureScoringPool.Count;

        highlightStructureScoringPool[highlightStructureScoringPoolIndex].SetActive(true);

        highlightStructureScoringPool[highlightStructureScoringPoolIndex].transform.position = pos;
        highlightStructureScoringPool[highlightStructureScoringPoolIndex].transform.rotation = rotation;
        highlightStructureScoringPool[highlightStructureScoringPoolIndex].transform.localScale = new Vector3(
                                                                                                highlightStructureScoringPool[highlightStructureScoringPoolIndex].transform.localScale.x,
                                                                                                yHeight,
                                                                                                highlightStructureScoringPool[highlightStructureScoringPoolIndex].transform.localScale.z
                                                                                            );
        hasActivatedIndicator = true;

        // for(int i = 0; i < highlightStructureScoringPool.Count; i++){
        //     if(i != highlightStructureScoringPoolIndex){
        //         highlightStructureScoringPool[i].SetActive(false);
        //     }
        // }
    }

    public void ClearAllIndicators(){
        if(!hasActivatedIndicator){
            return; //IMP - No need to clear if not activated after previous clear
        }
        
        //DONE - Clear indexes as well (set to 0)
        highlightStructureNormalPoolIndex = 0;
        highlightStructureScoringPoolIndex = 0;

        foreach (GameObject highlightObject in highlightStructureNormalPool)
        {
            highlightObject.SetActive(false);
        }

        foreach (GameObject highlightObject in highlightStructureScoringPool)
        {
            highlightObject.SetActive(false);
        }

        hasActivatedIndicator = false;
    }
}
