using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.ParametersVM;

public class VariableParameterArray : VariableParameter
{
    public int Count { get; set; }

    public new string GetValue()
    {
        if (!IsVariable)
        {
            return Value;
        }

        var coefs = "";

        for (var i = 0; i < Count; i++)
        {
            coefs += RandomDouble.GetRandomDouble(Min, Max).ToString().Replace('.', ',');
            coefs += " ";
        }

        coefs = coefs.Substring(0, coefs.Length - 1);

        return coefs;
    }
}
