using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureScript : MonoBehaviour
{
    public Structure structureCard;
    public float rotation;

    public bool isStructurePlacedOnTop;
    public GameObject structureOnTop;
    public GameObject onTopPoint;

    public GameObject structureAtBottom;    //IMP - TO get track of bottm structure
    
    void Start(){
        PlacementAnimation(gameObject);
        isStructurePlacedOnTop = false;
        structureOnTop = null;
    }

    public void BulgeStructure(){
        PlacementAnimation(gameObject);
    }

    public void PlacementAnimation(GameObject placedObject){
        LeanTween.cancel(placedObject);
        LeanTween.scale(placedObject, placedObject.transform.localScale * 1.085f, 0.75f).setEasePunch();
    }

    public void AddStructureOnTop(GameObject structureOnTop){
        if(!structureCard.canPlaceOnTop){
            return;
        }

        isStructurePlacedOnTop = true;
        this.structureOnTop = structureOnTop;
    }

    public void RemoveStructureOnTop(){
        //IMP - Delete the top structure in structure manager script
        isStructurePlacedOnTop = false;
        structureOnTop = null;
    }
}
