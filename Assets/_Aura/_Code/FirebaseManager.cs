using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using SimpleJSON;
using System;

public class FirebaseManager : MonoBehaviour
{
    public DeploymentsViewPageBackend deploymentsPage;
    public string json;
    public GuardDeployment dep;
    public List<GuardDeployment> deployments = new List<GuardDeployment>();
    public List<string> containerRegions = new List<string>();
    public List<GuardDeployment> downloadedData = new List<GuardDeployment>();    
    DatabaseReference dbRef;
    private void Awake()
    {
        var node = JSON.Parse(json);

        foreach (var n in node.Values)
        {
            var newGuard = new GuardDeployment();
            newGuard = JsonUtility.FromJson<GuardDeployment>(n.ToString());
            deployments.Add(newGuard);
            containerRegions.Add(newGuard.CONTAINER);
        }
    }

    private void OnEnable()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        //CreateDeploymentsData();
        StartCoroutine(GetDeploymentsData(()=>deploymentsPage.ShowLoadButton()));
    }

    private void CreateDeploymentsData()
    {
        foreach (var n in deployments)
        {
            var guardJson = JsonUtility.ToJson(n);
            dbRef.Child("Deployments").Child(n.CONTAINER).SetRawJsonValueAsync(guardJson);
        }
    }

    private IEnumerator GetDeploymentsData(Action _callback)
    {
        foreach(var n in deployments)
        {
            var taskTocomplete = dbRef.Child("Deployments").Child(n.CONTAINER).GetValueAsync();
            yield return new WaitUntil(predicate: () => taskTocomplete.IsCompleted);

            if (taskTocomplete != null)
            {
                DataSnapshot snapShot = taskTocomplete.Result;

                var newData = snapShot.GetRawJsonValue();
                var guard = JsonUtility.FromJson<GuardDeployment>(newData);
                downloadedData.Add(guard);
            }
        }
        _callback.Invoke();
    }
}
