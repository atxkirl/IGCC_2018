using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
public class Login : MonoBehaviour
{
    public InputField LoginIDText;
    public InputField PasswordText;
    public Text LoginErrorText;
    public void CreateAccountButtonClick()
    {
        //ensure all items were entered
        if (LoginIDText.text.Length > 0 && PasswordText.text.Length > 0)
        {

           new  RegistrationRequest()
           .SetDisplayName(LoginIDText.text)
           .SetUserName(LoginIDText.text)
           .SetPassword(PasswordText.text)
           .Send((response) => {
               if (!response.HasErrors)
               {
                   Debug.Log("All gucci");
                   LoginErrorText.text = "All gucci";
               }
               else
               {
                   Debug.Log("All information must be entered");
                   LoginErrorText.text = "All information must be entered";
               }

           });
        }
    }
    public void LoginButtonClick()
    {
        //verify credentials
        if (LoginIDText.text.Length > 0 && PasswordText.text.Length > 0)
        {
            new AuthenticationRequest()
           .SetUserName(LoginIDText.text)
           .SetPassword(PasswordText.text)
           .Send((response) => {

               if (!response.HasErrors)
               {
                   LoginErrorText.text = "All gucci2";
               }
               else
               {
                   LoginErrorText.text = "Account do not exist";
               }

           });
        }
    }
    public void ExitButtonClick()
    {
        Application.Quit();
    }
}
