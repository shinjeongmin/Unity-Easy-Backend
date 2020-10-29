using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button LoginButton;
    public Button CreateUserButton;
    public GameObject RegisterUserUI;

    private void Start()
    {
        LoginButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.Web.Login(UsernameInput.text, PasswordInput.text));
        });
        CreateUserButton.onClick.AddListener(() =>
        {
            RegisterUserUI.SetActive(true);
        });
    }
}
