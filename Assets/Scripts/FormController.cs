using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormController : MonoBehaviour
{
    public static Form currentForm = new Form();
    public Animator prevForm;
    public Animator nextForm;
    public Toggle[] toggles;
    public Button next;
    public InputField other;
    public Toggle otherToggle;
    public string otherText;
    public Text finalFormText;
    public Image genderImage;
    public Sprite[] images;


    public void Check()
    {
        next.interactable = CanContinue();
    }

    bool CanContinue()
    {
        if (otherToggle.isOn && otherText.Equals("")) return false;
        else if (otherToggle.isOn && !otherText.Equals("")) return true;

        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn) return true;
        }
        return false;
    }

    public void OtherOption(bool newValue)
    {
        other.interactable = newValue;
        Check();
    }

    public void TextChange(string newValue)
    {
        otherText = newValue;
        Check();
    }

    public void Next()
    {
        StartCoroutine("ComeandGo", true);
    }
    public void Previous()
    {
        StartCoroutine("ComeandGo", false);
    }

    IEnumerator ComeandGo(object value)
    {
        if ((bool)value)
        {
            gameObject.GetComponent<Animator>().SetBool("Go", true);
            yield return new WaitForSeconds(0.5f);
            nextForm.SetBool("Come", true);
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<Animator>().SetBool("Go", false);
            nextForm.SetBool("Come", false);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("ComeReverse", true);
            yield return new WaitForSeconds(0.5f);
            prevForm.SetBool("GoReverse", true);
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<Animator>().SetBool("ComeReverse", false);
            prevForm.SetBool("GoReverse", false);
        }
    }

    public void FirstNext()
    {
        string likes = "";
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn) likes += toggle.gameObject.transform.GetChild(1).GetComponent<Text>().text + ", ";
        }

        if (otherToggle.isOn) likes += otherText;
        else likes = likes.Remove(likes.LastIndexOf(", "), 2);

        currentForm.likes = likes;
    }

    public void SecondNext()
    {
        string freeTimes = "";
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn) freeTimes += toggle.gameObject.transform.GetChild(1).GetComponent<Text>().text + ", ";
        }

        if (otherToggle.isOn) freeTimes += otherText;
        else freeTimes = freeTimes.Remove(freeTimes.LastIndexOf(", "), 2);

        currentForm.freeTime = freeTimes;
    }

    public void ThirdNext()
    {
        string whenGrown = "";
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn) whenGrown += toggle.gameObject.transform.GetChild(1).GetComponent<Text>().text + ", ";
        }

        if (otherToggle.isOn) whenGrown += otherText;
        else whenGrown = whenGrown.Remove(whenGrown.LastIndexOf(", "), 2);

        currentForm.whenGrown = whenGrown;
    }

    public void FourthNext()
    {
        currentForm.name = otherText;
        string iam;
        iam = "<color=#000000>Me llamo\n</color><color=#141414>" + currentForm.name + "\n</color>";
        iam += "<color=#000000>Me gusta\n</color><color=#141414>" + currentForm.likes + "\n</color>";
        iam += "<color=#000000>En mi tiempo libre\n</color><color=#141414>" + currentForm.freeTime + "\n</color>";
        iam += "<color=#000000>Cuando sea grande quiero ser\n</color><color=#141414>" + currentForm.whenGrown + "\n</color>";

        switch (currentForm.gender)
        {
            case Gender.Male:
                genderImage.sprite = images[0];
                break;
            case Gender.Female:
                genderImage.sprite = images[1];
                break;
            case Gender.Other:
                genderImage.sprite = images[2];
                break;
        }

        finalFormText.text = iam;
    }


}

public class Form
{
    public string name = "";
    public string likes;
    public string freeTime = "";
    public string whenGrown = "";
    public Gender gender;
}

[System.Serializable]
public enum Gender { Male, Female, Other }

