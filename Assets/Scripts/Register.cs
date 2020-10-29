using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField PasswordComfirmInput;
    public Button Submit;

    private void Start()
    {
        Submit.onClick.AddListener(() =>
        {
            if (PasswordInput.text == PasswordComfirmInput.text)
            {
                StartCoroutine(Main.Instance.Web.RegisterUser(UsernameInput.text, PasswordInput.text));   
            }
            else
            {
                Debug.Log("Please Reconfirm your password!");
            }
        });
    }
}
