using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextsSetter : MonoBehaviour
{

    public Text[] likes;
    public string[] likesText;
    public Text[] freeTime;
    public string[] freeTimeText;
    public Text[] whenGrown;
    public string[] whenGrownText;
    public Animator genderForm;

    void Start()
    {
        for (int i = 0; i < likes.Length; i++)
        {
            likes[i].text = likesText[i];
        }
        for (int i = 0; i < freeTime.Length; i++)
        {
            freeTime[i].text = freeTimeText[i];
        }
        StartCoroutine("CancelAnimation");
    }

    IEnumerator CancelAnimation()
    {
        yield return new WaitForSeconds(1f);
        genderForm.SetBool("Come", false);
    }

    public void SetGenderTexts(int value)
    {
        switch (value)
        {
            case 1:
                FormController.currentForm.gender = Gender.Male;
                break;
            case 2:
                FormController.currentForm.gender = Gender.Female;
                break;
            case 3:
                FormController.currentForm.gender = Gender.Other;
                break;
        }

        for (int i = 0; i < whenGrown.Length; i++)
        {
            whenGrown[i].text = (FormController.currentForm.gender != Gender.Female) ? whenGrownText[i] : whenGrownText[i].Insert(whenGrownText[i].LastIndexOf("o"), "a").Remove(whenGrownText[i].LastIndexOf("o") + 1, 1);
        }
    }

}
