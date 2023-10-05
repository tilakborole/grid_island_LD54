using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StructureInfoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI pointsText;
    public Image structureCategoryIconImage;
    public Image scoringIndicatorImage;

    public Color scoringIndictorStartColor;

    void Start(){
        
    }

    public void Load(STRUCTURE_CATEGORY structureCategory, int points, bool isScoring){
        //DONE - Get icon from DB
        pointsText.text = "+" + points + " points";

        if(structureCategory == STRUCTURE_CATEGORY.RESIDENTIAL){
            structureCategoryIconImage.sprite = UIManager.instance.residentialIcon;
        }else if(structureCategory == STRUCTURE_CATEGORY.NATURE){
            structureCategoryIconImage.sprite = UIManager.instance.natureIcon;
        }else if(structureCategory == STRUCTURE_CATEGORY.ENTERTAINMENT){
            structureCategoryIconImage.sprite = UIManager.instance.entertainmentIcon;
        }else if(structureCategory == STRUCTURE_CATEGORY.INDUSTRY){
            structureCategoryIconImage.sprite = UIManager.instance.industryIcon;
        }

        if(isScoring){
            scoringIndicatorImage.color = Color.green;
        }else{
            scoringIndicatorImage.color = scoringIndictorStartColor;
        }
    }

    public void Activate(bool activate){
        gameObject.SetActive(activate);
    }

    public void OnPointerEnter(PointerEventData eventData){
        Debug.Log("Test - Pointer enter");  //TEST - Works!!!
    }

    public void OnPointerExit(PointerEventData eventData){
        Debug.Log("Test - Pointer exit");   //TEST - Works!!!
    }
}
