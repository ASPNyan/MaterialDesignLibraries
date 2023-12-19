using System.Diagnostics.CodeAnalysis;

namespace MaterialDesignCLI;

public class Function(string name, params ParamCheck[] paramChecks)
{
    public List<object> GetParameters(string input)
    {
        input = input.Trim();
        if (!input.StartsWith(name)) throw new Exception($"Invalid syntax, expected name {name}.");
        int valuesStartIndex = input.IndexOf('(') + 1;
        string valuesString = input[valuesStartIndex..input.LastIndexOf(')')];
        List<string> values = [];
        while (valuesString.Length > 0)
        {
            int valEndPos;
            int leftParen = 0; // occurrences of (
            for (int i = 0;; i++)
            {
                if (i >= valuesString.Length)
                {
                    if (leftParen is not 0) throw new Exception("Invalid syntax.");
                    valEndPos = i;
                    break;
                }
                
                char current = valuesString[i];
                switch (current)
                {
                    case '(':
                        leftParen++;
                        continue;
                    case ')':
                    {
                        leftParen--;
                        if (leftParen < 0) throw new Exception($"Invalid syntax at Col {valuesStartIndex + i}.");
                        continue;
                    }
                    case not ',':
                        continue;
                }
                
                if (leftParen is not 0) continue;

                valEndPos = i;
                break;
            }
            
            values.Add(valuesString[..valEndPos].Trim());
            valuesString = valEndPos < valuesString.Length - 1 ? valuesString[(valEndPos + 1)..] : string.Empty;
        }

        if (values.Count != paramChecks.Length)
            throw new Exception($"Invalid syntax. Expected {paramChecks.Length} parameters, got {values.Count}.");

        List<object> outputs = [];
        for (int i = 0; i < values.Count; i++)
        {
            if (!paramChecks[i](values[i], out object? returnValue))
            {
                throw new Exception($"Invalid syntax at parameter {i + 1}.");
            }
            outputs.Add(returnValue);
        }

        return outputs;
    }
}

public delegate bool ParamCheck(string input, [NotNullWhen(true)] out object? returnValue);