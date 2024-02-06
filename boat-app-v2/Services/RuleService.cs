namespace boat_app_v2.Services;

public class RuleService
{
    public string IncrementBoatCode(string lastInsertedCode)
    {
        var index = 0;
        var newValue = "";
        foreach (var c in lastInsertedCode.Reverse())
        {
            index++;
            if (c == '-')
            {
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

            if (!IsCharIncrementable(c)) continue;
            newValue = IncrementCharacter(c).ToString();
            index--;
            break;
        }
        var result = lastInsertedCode[^index..];
        result = ResetValues(result);
        var finalResult = lastInsertedCode[..^(index+1)];

        finalResult += newValue + result;
        return finalResult;
    }

    private string ResetValues(string values)
    {
        var result = "";
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

    private bool IsCharIncrementable(char c)
    {
        if (c < 122) return true;
        return false;
    }

    private static bool IsIntegerIncrementable(char c)
    {
        var i = char.GetNumericValue(c);
        return i < 9;
    }

    private static char IncrementCharacter(char c)
    {
        if (c != 'Z') return ++c;
        c = 'a';
        return c;
    }

    private static int IncrementInteger(char c)
    {
        var d = char.GetNumericValue(c);
        var i = Convert.ToInt32(d);
        i = i + 1;
        return i;
    }
}