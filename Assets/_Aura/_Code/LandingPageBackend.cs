using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPageBackend : MonoBehaviour
{
    private void Start()
    {
        var user = new User("dmikala");

    }
    class User
    {
        public string password;

        public User(string _pass)
        {
            password = _pass;
        }
    }
}
