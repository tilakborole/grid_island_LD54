using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isClicked = false;
    public void OnPointerEnter(PointerEventData eventData){
        //LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector3(1.2f, 1.2f, gameObject.GetComponent<RectTransform>().localScale.z), 0.3f).setEase(LeanTweenType.easeInQuart);
        UnityEngine.UI.Outline outline = gameObject.GetComponent<UnityEngine.UI.Outline>();
        if(outline == null){
            outline = gameObject.AddComponent<UnityEngine.UI.Outline>();
            outline.effectColor = Color.blue;
            outline.effectDistance = new Vector2(10f, -10f);
            outline.enabled = true;
        }else{
            outline.enabled = true;
            outline.effectColor = Color.blue;
            outline.effectDistance = new Vector2(10f, -10f);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if(!isClicked){
            //LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), gameObject.GetComponent<RectTransform>().localScale.z).setEase(LeanTweenType.easeOutQuart);
            UnityEngine.UI.Outline outline = gameObject.GetComponent<UnityEngine.UI.Outline>();
            if(outline != null){
                outline.enabled = false;
                outline.effectColor = Color.black;
                outline.effectDistance = new Vector2(5f, -5f);
            }
        }
    }

    public void Bulge(){
        //LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector3(1.2f, 1.2f, gameObject.GetComponent<RectTransform>().localScale.z), 0.3f).setEase(LeanTweenType.easeInQuart);
        UnityEngine.UI.Outline outline = gameObject.GetComponent<UnityEngine.UI.Outline>();
        if(outline == null){
            outline = gameObject.AddComponent<UnityEngine.UI.Outline>();
            outline.effectColor = Color.blue;
            outline.effectDistance = new Vector2(10f, -10f);
            outline.enabled = true;
        }else{
            outline.enabled = true;
            outline.effectColor = Color.blue;
            outline.effectDistance = new Vector2(10f, -10f);
        }
    }

    public void BulgeExit(){
        //LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), gameObject.GetComponent<RectTransform>().localScale.z).setEase(LeanTweenType.easeOutQuart);
        UnityEngine.UI.Outline outline = gameObject.GetComponent<UnityEngine.UI.Outline>();
        if(outline != null){
            outline.enabled = false;
        }
    }
}
