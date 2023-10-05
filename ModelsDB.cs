using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsDB : MonoBehaviour
{
    public static ModelsDB instance;

    public Structure[] residentialStructures;
    public Structure[] natureStructures;
    public Structure[] entertainmentStructures;
    public Structure[] industryStructures;

    public GameObject highlightStructureNormal;
    public GameObject highlightStructureScoring;

    void Awake(){
        if(instance == null){
            instance = this;
        }else if(instance != null){
            Destroy(gameObject);
        }
    }

    public Structure GetStructureForCategory(STRUCTURE_CATEGORY structureCategory){
        if(structureCategory == STRUCTURE_CATEGORY.RESIDENTIAL){
            return residentialStructures[Random.Range(0, residentialStructures.Length)];
        }else if(structureCategory == STRUCTURE_CATEGORY.NATURE){
            return natureStructures[Random.Range(0, natureStructures.Length)];
        }else if(structureCategory == STRUCTURE_CATEGORY.ENTERTAINMENT){
            return entertainmentStructures[Random.Range(0, entertainmentStructures.Length)];
        }else if(structureCategory == STRUCTURE_CATEGORY.INDUSTRY){
            return industryStructures[Random.Range(0, industryStructures.Length)];
        }

        Debug.Log("Structure Category not found");
        return null;
    }
}
