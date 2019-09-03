public class Rule  {

    public char input;
    public string output;
    

    public Rule (char _input , string _output)
    {
        input = _input;
        output = _output;
    }
    public bool Check(char _in)
    {
        if (_in == input)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
