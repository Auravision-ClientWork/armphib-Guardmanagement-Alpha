using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentsViewPageBackend : MonoBehaviour
{
    public GameObject loadDeploymentsButton;
    public GameObject deploymentViewItem;
    public RectTransform deploymentViewItemHolder;

    private void Start()
    {
        loadDeploymentsButton.GetComponentInChildren<TMP_Text>().text = "Retrieving";
        loadDeploymentsButton.GetComponent<Button>().enabled = false;
    }
    public void LoadDeployments()
    {
        var deployments = FindAnyObjectByType<FirebaseManager>().downloadedData;
        GameObject itemObj;
        foreach (GuardDeployment deployment in deployments)
        {
            itemObj = Instantiate(deploymentViewItem);
            itemObj.transform.SetParent(deploymentViewItemHolder, false);
            itemObj.GetComponent<DeploymentsItem>().SetUp(deployment);
        }
        loadDeploymentsButton.SetActive(false);

    }

    internal void ShowLoadButton()
    {
        loadDeploymentsButton.GetComponentInChildren<TMP_Text>().text = "Load";
        loadDeploymentsButton.GetComponentInChildren<TMP_Text>().color = Color.green;
        loadDeploymentsButton.GetComponent<Button>().enabled = true;

    }
}
