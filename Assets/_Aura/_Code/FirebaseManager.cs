using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    DatabaseReference dbReference;
    private void Start()
    {
        // Get the root reference location of the database.
        dbReference  = FirebaseDatabase.DefaultInstance.RootReference;
        CreateAdminUser();
    }

    private void CreateAdminUser()
    {
        User admin = new User("dmikala");
        string adminJson = JsonUtility.ToJson(admin);

        dbReference.Child("admin").Child("Admin").SetRawJsonValueAsync(adminJson);
    }

    public void HandleUserPassInput(string _pass)
    {
        //compare the input pass with the pass in the database
        //get the database pass
        //string storedPass= GetPasswordInDatabase();
    }

 
}
