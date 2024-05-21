using System.Text;
using TMPro;
using UnityEngine;

public class PasswordShow : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI realInput;
    StringBuilder stringBuilder = new StringBuilder();

    public void Change()
    {
        if (inputField.contentType == TMP_InputField.ContentType.Password)
        {
            inputField.contentType = TMP_InputField.ContentType.Standard;
            realInput.text = inputField.text;
        }
        else
        {
            inputField.contentType = TMP_InputField.ContentType.Password;
            stringBuilder.Clear();
            for(int i = 0; i < inputField.text.Length; i++)
            {
                stringBuilder.Append("*");
            }
            realInput.text = stringBuilder.ToString();
            
        }
    }
}
