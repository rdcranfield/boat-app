using System.Data;

namespace boat_app_v2.Services;

public class RuleService
{
    public RuleService()
    {
    }

    public string IncrementBoatCode(string lastInsertedCode)
    {
        int index = 0;
        string newValue = "";
        foreach (var c in lastInsertedCode.Reverse())
        {
            index++;
            if (c == '-')
            {
                //index--;
                continue;
            }
            
            if (char.IsNumber(c))
            {
                if (IsIntegerIncrementable(c))
                {
                    newValue = IncrementInteger(c).ToString();
                    index--;
                    break;
                }
                continue;
            }
            if (IsCharIncrementable(c))
            {
                newValue = IncrementCharacter(c).ToString();
                index--;
                break;
            }
        }
        string result = lastInsertedCode.Substring(lastInsertedCode.Length-index);
        result = ResetValues(result);
        string finalResult = lastInsertedCode.Substring(0, lastInsertedCode.Length-(index+1));

        finalResult += newValue + result;
        return finalResult;
    }

    private string ResetValues(string values)
    {
        string result = "";
        foreach (var c in values)
        {
            if (c == '-')
            {
                result += c;
                continue;
            }

            if (char.IsNumber(c))
            {
                result += "0";
                continue;
            }
            result += "A";
        }

        return result;
    }

    public bool IsCharIncrementable(char c)
    {
        if (c < 122) return true;
        return false;
    }
    
    public bool IsIntegerIncrementable(char c)
    {
        double i = char.GetNumericValue(c);
        if (i < 9) return true;
        return false;
    }

    public char IncrementCharacter(char c)
    {
        if (c == 'Z')
        {
            c = 'a';
            return c;
        }
        return ++c;
    }
    
    public int IncrementInteger(char c)
    {
        double d = char.GetNumericValue(c);
        var i = Convert.ToInt32(d);
        i = i + 1;
        return i;
    }
}