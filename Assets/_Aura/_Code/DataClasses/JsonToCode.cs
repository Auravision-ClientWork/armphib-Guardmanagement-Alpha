using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class JsonToCode : MonoBehaviour
{
    public string json;
    public GuardDeployment dep;
    public List<GuardDeployment> deployments = new List<GuardDeployment>();
    private void Start()
    {
        var node = JSON.Parse(json);
        Debug.Log(node[0].ToString());
        foreach (var n in node.Values)
        {
            var newGuard = new GuardDeployment();
            newGuard = JsonUtility.FromJson<GuardDeployment>(n.ToString()); 
            deployments.Add(newGuard);
        }
    }
}
