public class QOpen : IQuestion
{
    public string answer = "";

    public void OnValueChanged(string value)
    {
        answer = value;
    }
}