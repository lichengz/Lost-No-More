using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.QuestMachine;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    private InputReader _inputReader = default;

    private QuestJournal _questJournal;

    private void Awake()
    {
        _questJournal = GetComponent<QuestJournal>();
    }

    private void OnEnable()
    {
        _inputReader.OpenQuestEvent += OpenQuestJournal;
        _inputReader.CloseQuestEvent += CloseQuestJournal;
    }

    private void OnDisable()
    {
        _inputReader.OpenQuestEvent -= OpenQuestJournal;
        _inputReader.CloseQuestEvent -= CloseQuestJournal;
    }

    private void OpenQuestJournal()
    {
        _inputReader.EnableMenuInput();
        _questJournal.ShowJournalUI();
    }
    
    private void CloseQuestJournal()
    {
        _inputReader.EnableGameplayInput();
        _questJournal.HideJournalUI();
    }
}