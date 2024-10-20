using UnityEngine;

public abstract class MailSender
{
    public static void SendEmail(string subject, string emailTo, string body)
    {
        subject = MyEscapeURL(subject);
        body = MyEscapeURL("Email from Unity\r\n" + body);
        Application.OpenURL("mailto:" + emailTo + "?subject=" + subject + "&amp;body=" + body);
    }

    static string MyEscapeURL(string URL)
    {
        return WWW.EscapeURL(URL).Replace("+", "%20");
    }
}