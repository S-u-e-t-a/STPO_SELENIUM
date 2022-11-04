using System.Collections.Generic;


namespace STPO_dynamic_test.Model;

public class InitialTestData
{
    public string Min { get; init; }
    public string Max { get; init; }
    public string Step { get; init; }
    public List<string> Coefs { get; init; }

    public string CoefsString => string.Join(' ', Coefs);

    public string IntegrateMethod { get; init; }

    public override string ToString()
    {
        var strCoefs = string.Join(" ", Coefs);

        return $"{Min} {Max} {Step} {IntegrateMethod} {strCoefs}";
    }
}
