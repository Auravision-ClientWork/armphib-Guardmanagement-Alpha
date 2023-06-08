using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AndroidGoodiesExamples;
using DeadMosquito.AndroidGoodies;

public class DeploymentsItem : MonoBehaviour
{
    public TMP_Text containerTxt;
    public TMP_Text emp1NumberTxt;
    public TMP_Text emp2NumberTxt;
    public Button goToLocationBtn;
    public Button goToDialerBtn;

    public void SetUp(GuardDeployment guardData)
    {
        containerTxt.text = guardData.CONTAINER;
        emp1NumberTxt.text = guardData.EMPLOYEE1;
        emp2NumberTxt.text = guardData.EMPLOYEE2;

        goToDialerBtn.onClick.AddListener(() => DialContainer(guardData.CONTAINERPHONENO));
        goToLocationBtn.onClick.AddListener(() => GoToContainerLocation(guardData.LATITUDE,
            guardData.LONGTUDE, guardData.CONTAINER));
    }

    private void DialContainer(string _number)
    {
        var newNumber = "0" + _number;
        var number = int.Parse(newNumber);
        AGDialer.OpenDialer(newNumber);
    }
    private void GoToContainerLocation(string _lat,string _lng,string _label)
    {
        var latitude = float.Parse(_lat);
        var longitude = float.Parse(_lng);
        AGMaps.OpenMapLocationWithLabel(latitude, longitude, _label);

    }
}
