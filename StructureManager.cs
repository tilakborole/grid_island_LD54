using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class StructureManager : MonoBehaviour
{
    public static StructureManager instance;

    //SECTION - Input Mouse button up related
    private float timer;

    //SECTION - Placement related
    public List<StructureScript> placedStructures = new List<StructureScript>(); //IMP - Keep a Track
    public LayerMask structureLayer;
    public LayerMask cellLayer;
    public GameObject currentHoverCell;
    public GameObject lastSpawnedStructure;
    public GameObject structureToPlace;

    public Structure currentStructureSelected;

    //SECTION - Rotate Related
    public float rotateByAngle;
    private Quaternion targetRotation;
    private float targetAngle;
    public float rotateSpeed;
    private float rotationAngle;

    //SECTION - Floating text
    public GameObject floatingTextObject;

    //SECTION - Particle Effects
    public GameObject placementParticleEffect;

    //SECTION - Undo Placement
    private GameObject lastPlacedStructure;

    public bool structureSelected;

    public GameObject[] cells;

    private Vector3 lastPos;

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
        rotationAngle = 0;
        timer = 0f;
        lastPos = Vector3.zero;
        targetRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.gameplayState == GAMEPLAY_STATE.PLACEMENT){
            if(GameManager.instance.currentGameInfo.structuresLeft > 0){
                if (!EventSystem.current.IsPointerOverGameObject(-1)){
                    if(Input.GetMouseButtonUp(0)){
                        Debug.Log("Click over");
                        //SECTION - Left Click
                        if(timer < 0.5f){
                            var hitGameObject = RaycastCell();
                            if(hitGameObject != null){
                                //NOTE - Place struture
                                PlaceStructure();
                                
                            }
                            // }else if(RaycastOtherThanStructure()){
                            //     //IMP - If we click any point other than strcuture, then deselect it
                            //     //TEST - Test this!!!
                                
                            // }
                        }

                        timer = 0f;
                    }else if(Input.GetMouseButton(0)){
                        //SECTION - Left hold
                        timer += Time.deltaTime;
                    }else{
                        //SECTION - Hover on structure
                        //TODO - Use this for tooltip/highlight for structure
                        var hitGameObject = RaycastCell();
                        if(hitGameObject != null){
                            HoverStructure(hitGameObject);
                        }else{
                            ClearHoverStructure();
                        }

                        //SECTION - Rotation
                        if(Input.GetKeyDown(KeyCode.Z)){
                            //RotateHoverStructure(false);
                            StartCoroutine(LerpRotateHoverStructure(false));
                        }else if(Input.GetKeyDown(KeyCode.X)){
                            //RotateHoverStructure(true);
                            StartCoroutine(LerpRotateHoverStructure(true));
                        }
                    }
                }

                if(floatingTextObject != null && structureToPlace != null){
                    floatingTextObject.transform.position = Camera.main.WorldToScreenPoint(structureToPlace.transform.position)  + new Vector3(0f, 100f, 0f);
                }
            }
        }
    }

    private GameObject RaycastCell(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayer)){
            if(hit.transform.parent == null){
                Debug.Log("Hit strtcure GameObject has no parent it seems. Name: " + hit.transform.gameObject);
                return null;
            }
            return hit.transform.gameObject; //IMP - Collider is  child to the parent one
        }

        return null;
    }

    IEnumerator LerpRotateHoverStructure(bool clockwise){
        if(structureToPlace == null){
            yield return null;
        }

        //IMP - if statement is important because yield retruns waits for next frame and then continues. It does not skip exceution
        if(structureToPlace != null){
            rotationAngle = structureToPlace.transform.rotation.eulerAngles.y;
            targetRotation = structureToPlace.transform.rotation;

            //SECTION - If structure supports rotateby45 then rotateangle = 45 othersie it is 90
            if(currentStructureSelected.is45DegreeRotation){
                if(clockwise){
                    targetRotation *= Quaternion.AngleAxis(45f, transform.up);
                }else{
                    targetRotation *= Quaternion.AngleAxis(45f, -transform.up);
                }
            }else{
                if(clockwise){
                    targetRotation *= Quaternion.AngleAxis(rotateByAngle, transform.up);
                }else{
                    targetRotation *= Quaternion.AngleAxis(rotateByAngle, -transform.up);
                }
            }

            if(currentStructureSelected.is45DegreeRotation){
                if(clockwise){
                    rotationAngle += 45;
                }else{
                    rotationAngle -= 45;
                }
            }else{
                if(clockwise){
                    rotationAngle += 90;
                }else{
                    rotationAngle -= 90;
                }
            }

            Debug.Log("structure to rotate "+currentStructureSelected.structureName);
            Debug.Log("target rotation "+targetRotation);
            Debug.Log(" strcuture rotation "+ structureToPlace.transform.rotation);

            while(true){
                if(structureToPlace == null){
                    break;
                }

                if(structureToPlace.transform.rotation == targetRotation){
                    break;
                }

                if(!IsQuaternionInvalid(targetRotation)){
                    structureToPlace.transform.rotation = Quaternion.RotateTowards(structureToPlace.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                }

                yield return null;
            }
        }

        yield return null;
    }

    private bool IsQuaternionInvalid(Quaternion q) {
        bool check = q.x == 0f;
        check &= q.y == 0;
        check &= q.z == 0;
        check &= q.w == 0;
    
        return check;
    }

    public void Init(){
        //DONE - Complete this
        ClearFloatingText();

        if(lastPlacedStructure != null){
            Destroy(lastPlacedStructure);
            lastPlacedStructure = null;
        }

        rotationAngle = 0;
        timer = 0f;
        targetRotation = Quaternion.identity;

        structureSelected = false;

        //TestStructure(); //TEST
        lastPos = Vector3.zero;
    }

    public void Clear(){
        //DONE - Complete this
        ClearFloatingText();

        if(structureToPlace != null){
            Destroy(structureToPlace);
            structureToPlace = null;
        }

        if(currentStructureSelected != null){
            currentStructureSelected = null;
        }

        if(currentHoverCell != null){
            Destroy(currentHoverCell);
            currentHoverCell = null;
        }

        foreach(StructureScript structureScript in placedStructures){
            Destroy(structureScript.gameObject);
        }

        placedStructures.Clear();
        lastPlacedStructure = null;

        ClearFloatingText();

        foreach (GameObject cell in cells)
        {
            cell.GetComponent<Collider>().enabled = true;
        }
    }

    //TODO - Complete this
    public void StructureCategorySelected(STRUCTURE_CATEGORY structureCategory){
        structureSelected = true;
        currentStructureSelected = ModelsDB.instance.GetStructureForCategory(structureCategory);

        //DONE - UI Function
        UIManager.instance.LoadStructureInfoWithoutIndication(currentStructureSelected.nearStructureScores);
    }

    void HoverStructure(GameObject marker){
        if(currentStructureSelected == null){
            //Debug.Log("Structure not selected");  //NOTE - Too many logs
            return;
        }

        if(!structureSelected){
            return;
        }

        Vector3 position = marker.transform.position;

        if(lastPos != position){
            GameManager.instance.ClearAllIndicators();
        }

        currentHoverCell = marker;

        if(structureToPlace == null && structureSelected){
            structureToPlace = Instantiate(currentStructureSelected.structurePrefab,
                                 new Vector3(position.x, position.y + 0.15f, position.z), marker.transform.parent.rotation);    //IMP - Island rotation
            
        }else{
            structureToPlace.transform.position = new Vector3(position.x, position.y + 0.15f, position.z);
            //structureToPlace.transform.rotation = marker.transform.parent.rotation; //IMP - Redundant line
        }
        
        if(currentHoverCell != null){
            ShowFloatingText(position, currentStructureSelected.points, currentStructureSelected.nearStructureScores, lastPos != position);
            // OutlineNeigborStructures(position, currentStructureSelected.nearStructures);
            //TODO - UI Function here
            UIManager.instance.LoadUpdateSelectedStructureIconInfo(position, currentStructureSelected.nearStructureScores, true);
        }

        lastPos = position;
    }

    public void ClearHoverStructure(){
        if(structureToPlace == null){
            //No structure to destroy
            return;
        }

        Debug.Log("Clearing hover strcuture");
        Destroy(structureToPlace);
        structureToPlace = null;
        //currentHoverCell.SetActive(true);
        currentHoverCell = null;
        
        ClearFloatingText();

        //TODO - Uncomment this later
        GameManager.instance.ClearAllIndicators();  //IMP PERFORMANCE
    }

    void PlaceStructure(){
        //DONE - Make child of segement
        if(currentStructureSelected == null){
            Debug.Log("No structure selected");
            return;
        }

        if(structureToPlace == null){
            Debug.Log("No hover strcuture found");
            return;
        }

        if(GameManager.instance.soundOn){
            FindObjectOfType<AudioManager>().Play("Place");
        }

        Vector3 position = Vector3.zero;

        //CHANGE
        position = currentHoverCell.transform.position + new Vector3(0f, 0.02f, 0f);

        structureToPlace.transform.position = position;

        structureToPlace.GetComponent<StructureScript>().structureCard = currentStructureSelected;

        structureToPlace.GetComponent<StructureScript>().rotation = structureToPlace.transform.rotation.eulerAngles.y;
        
        //IMP - Made strcture child of segment
        structureToPlace.transform.parent = currentHoverCell.transform;

        placedStructures.Add(structureToPlace.GetComponent<StructureScript>());

        structureToPlace.GetComponent<StructureScript>().BulgeStructure();

        //DONE - Uncomment and complete this
        Instantiate(placementParticleEffect, new Vector3(position.x, position.y + 0.05f, position.z), placementParticleEffect.transform.rotation);

        GameManager.instance.lastPlacedIsStructure = true;

        if(currentStructureSelected.structureCategory == STRUCTURE_CATEGORY.RESIDENTIAL){
            GameManager.instance.currentGameInfo.residentialStructuresAvailable--;
        }else if(currentStructureSelected.structureCategory == STRUCTURE_CATEGORY.NATURE){
            GameManager.instance.currentGameInfo.natureStructuresAvailable--;
        }else if(currentStructureSelected.structureCategory == STRUCTURE_CATEGORY.ENTERTAINMENT){
            GameManager.instance.currentGameInfo.entertainmentStructuresAvailable--;
        }else if(currentStructureSelected.structureCategory == STRUCTURE_CATEGORY.INDUSTRY){
            GameManager.instance.currentGameInfo.industryStructuresAvailable--;
        }

        UIManager.instance.LoadStructureCategoryUI();

        //TODO - Uncomment soon
        GameManager.instance.CalculateHandleScore(position, currentStructureSelected.points, currentStructureSelected.nearStructureScores);
        
        //NOTE - The win/lose condition will also be handled by the above function
        ClearFloatingText();

        
        //IMP - Disable the collider of cell
        currentHoverCell.GetComponent<Collider>().enabled = false;
        //currentHoverCell.GetComponent<QuarterScript>().AddStructureToCircleLand(structureToPlace.GetComponent<StructureScript>());
        
        lastPlacedStructure = structureToPlace;
        //currentStructureSelected = null;
        structureToPlace = null;
        currentHoverCell = null;

        //TestStructure();    //IMP TEST

        structureSelected = false;
        UIManager.instance.HideInofPanel();
        UIManager.instance.ClearAllCategoryOutlines();
    }


    private void ShowFloatingText(Vector3 pos, int basePoints, NearStructureScore[] nearStructureScores, bool posChanged){
        //DONE - Uncomment soon
        int score = GameManager.instance.CalculateScore(pos, basePoints, nearStructureScores, posChanged);
        //Debug.Log("Trying to show floating text");
        if(floatingTextObject == null){
            //DONE - Uncomment soon
            floatingTextObject = Instantiate(GameManager.instance.floatingTextObjectPrefab, GameManager.instance.floatingCanvas.transform);
        }else{
            floatingTextObject.SetActive(true);
        }

        floatingTextObject.GetComponent<TextMeshProUGUI>().text = "+" + score;
    }

    private void ClearFloatingText(){
        if(floatingTextObject == null){
            return;
        }

        Destroy(floatingTextObject);
        floatingTextObject = null;
    }

}
