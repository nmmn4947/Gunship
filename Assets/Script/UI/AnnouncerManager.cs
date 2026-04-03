using System;
using Napadol.Tools;
using UnityEngine;
using Napadol.Tools.ActionPattern;
using TMPro;

public class AnnouncerManager : ActionListManager
{
    private static AnnouncerManager _instance;

    public static AnnouncerManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AnnouncerManager>();
                if (_instance == null)
                {
                    Debug.LogError("AnnouncerManager not found");
                }
            }
            return _instance;
        }
    }
    
    [SerializeField] private GameObject announcerObj;
    [SerializeField] private GameObject line1;
    [SerializeField] private GameObject line2;
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform bot;
    
    private Vector3 _originalLinePos;
    private Vector3 _scaleLine1;
    private Vector3 _scaleLine2;
    private TextMeshProUGUI announcerText;
    
    private void Start()
    {
        announcerText = announcerObj.GetComponentInChildren<TextMeshProUGUI>();
        _originalLinePos = line1.GetComponent<RectTransform>().anchoredPosition;
        _scaleLine1 = line1.GetComponent<RectTransform>().localScale;
        _scaleLine2 = line2.GetComponent<RectTransform>().localScale;

        Announce("Announced");
        Announce("Twice");
    }

    public void Announce(string txt)
    {
        actionList.AddAction(new CallBackAction(() => ChangeText(txt), nameof(ChangeText))); 
        
        actionList.AddAction(new ScaleAction(line1, Vector3.one, 0.25f).Easer(Easing.EaseOutQuad));
        actionList.AddAction(new ScaleAction(line2, Vector3.one, 0.25f).Easer(Easing.EaseOutQuad).Block());
        
        actionList.AddAction(new MoveRectTransformAction(line1, top.anchoredPosition, 0.25f).Easer(Easing.EaseLinear));
        actionList.AddAction(new MoveRectTransformAction(line2, bot.anchoredPosition, 0.25f).Easer(Easing.EaseLinear));
        actionList.AddAction(new ScaleAction(announcerObj, Vector3.one, 0.25f).Easer(Easing.EaseLinear).Block());
        
        actionList.AddAction(new WaitAction(1.5f));
        
        actionList.AddAction(new MoveRectTransformAction(line1, _originalLinePos, 0.25f).Easer(Easing.EaseInOutBack));
        actionList.AddAction(new MoveRectTransformAction(line2, _originalLinePos, 0.25f).Easer(Easing.EaseInOutBack));
        actionList.AddAction(new ScaleAction(announcerObj, new Vector3(1f, 0f, 1f), 0.25f).Easer(Easing.EaseInOutBack).Block());
        
        actionList.AddAction(new ScaleAction(line1, _scaleLine1, 0.25f).Easer(Easing.EaseOutQuad));
        actionList.AddAction(new ScaleAction(line2, _scaleLine2, 0.25f).Easer(Easing.EaseOutQuad).Block());
    }

    private void ChangeText(string txt)
    {
        announcerText.text = txt;
    }
}
