using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotesManager : MonoBehaviour
{
    public static NotesManager instance;

    //scene variables
    public TextMeshProUGUI noteText;

    //variables
    [Multiline]
    public List<string> noteTexts = new List<string>();
    int currentNoteIndex = 0;
    bool firstSwitch = false;

    private void Start()
    {
        instance = this;
        noteText.text = " ";

        if(noteTexts.Count > 0)
        {
            noteText.text = noteTexts[0];
        }
    }

    public void AddNote(string note)
    {
        noteTexts.Add(note);
    }

    public void AddNote(TextMeshProUGUI textObj)
    {
        noteTexts.Add(textObj.text);
    }

    public void SwitchNote()
    {
        Debug.Log("enter SwitchNote");
        if (!firstSwitch && noteTexts.Count > 0)
        {
            Debug.Log("enter first switch");
            currentNoteIndex = 0;
            firstSwitch = true;
        }
        else if(noteTexts.Count > 0)
        {
            Debug.Log("enter switch");
            currentNoteIndex++;
            if(currentNoteIndex >= noteTexts.Count)
            {
                currentNoteIndex = 0;

            }
        }

        UpdateNote();
    }

    public void UpdateNote()
    {
        if(noteTexts.Count > 0)
        {
            noteText.text = noteTexts[currentNoteIndex];
        }
        else
        {
            noteText.text = string.Empty;
        }
    }
}
