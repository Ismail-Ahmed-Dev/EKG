using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordToggler : MonoBehaviour
{
    [Header("References")]
    public TMP_InputField passwordField;
    public Image toggleButtonImage;

    [Header("Assets")]
    public Sprite eyeOpenSprite;   // ÃíŞæäÉ ÇáÚíä ÇáãİÊæÍÉ (ãÚäÇåÇ "ÇÖÛØ áÊÙåÑ")
    public Sprite eyeClosedSprite; // ÃíŞæäÉ ÇáÚíä ÇáãÔØæÈÉ (ãÚäÇåÇ "ÇÖÛØ áÊÎİí")

    private bool isHidden = true;

    void Start()
    {
        // åĞå ÇáÏÇáÉ åí ÇáÍá: ÊÌÈÑ ÇáÍŞá Ãä íßæä ãÎİíÇğ ÚäÏ ÈÏÁ ÇááÚÈÉ
        UpdateState();
    }

    public void ToggleVisibility()
    {
        // ÚßÓ ÇáÍÇáÉ ÇáÍÇáíÉ
        isHidden = !isHidden;
        UpdateState();
    }

    private void UpdateState()
    {
        if (isHidden)
        {
            // æÖÚ ÇáÅÎİÇÁ (********)
            passwordField.contentType = TMP_InputField.ContentType.Password;

            // äÚÑÖ ÕæÑÉ "ÇáÚíä ÇáãİÊæÍÉ" áíİåã ÇáãÓÊÎÏã Ãäå íãßäå ÇáÖÛØ ááÑÄíÉ
            if (eyeOpenSprite != null) toggleButtonImage.sprite = eyeOpenSprite;
        }
        else
        {
            // æÖÚ ÇáÅÙåÇÑ (Text)
            passwordField.contentType = TMP_InputField.ContentType.Standard;

            // äÚÑÖ ÕæÑÉ "ÇáÚíä ÇáãÔØæÈÉ" áíİåã ÇáãÓÊÎÏã Ãäå íãßäå ÇáÖÛØ ááÅÎİÇÁ
            if (eyeClosedSprite != null) toggleButtonImage.sprite = eyeClosedSprite;
        }

        // ÊÍÏíË Çáİíæ İæÑÇğ
        passwordField.ForceLabelUpdate();
    }
}