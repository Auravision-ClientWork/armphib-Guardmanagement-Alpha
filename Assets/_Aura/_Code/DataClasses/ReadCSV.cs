using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReadCSV : MonoBehaviour
{
    public List<GuardDeployment> deployments = new List<GuardDeployment>();
    private void Start()
    {
        ReadCSVFile();
    }

    private void ReadCSVFile()
    {
        StreamReader sr = new StreamReader("Assets/_Aura/CSV/containerdata.csv",true);
        bool endOfFile = false;
        while(!endOfFile)
        {
            string data_string = sr.ReadLine();
            if(data_string == null )
            {
                endOfFile = true;
                break;
            }

            var data_values = data_string.Split(',');
            Debug.Log(data_values.Length);
            foreach( var value in data_values )
            {
                Debug.Log(value.ToString());
                //GuardDeployment deployment = new GuardDeployment()
                //{
                //    CONTAINER = value[0].ToString(),
                //    CONTAINERPHONENO = value[1],
                //    LATITUDE = value[8],
                //    LONGTUDE = value[9],
                //    EMPLOYEE1 = value[10].ToString(),
                //    EMPLOYEE2 = value[11].ToString()      
                //};
                //deployments.Add(deployment);

            }
        }
    }
}
