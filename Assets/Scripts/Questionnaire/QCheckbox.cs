public class QCheckbox : IQuestion
{
    public bool answer = false;
    public Checkbox checkbox;
    
    public override void OnCheckboxClick(Checkbox checkbox)
    {
        answer = !answer;
        checkbox.Check();
    }
}