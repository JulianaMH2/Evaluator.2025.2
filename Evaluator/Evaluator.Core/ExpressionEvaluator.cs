namespace Evaluator.Core;

public class ExpressionEvaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return Calculate(postfix);
    }

    private static List<string> Tokenize(string infix)
    {
        var tokens = new List<string>();
        var number = string.Empty;

        foreach (char c in infix)
        {
            if (char.IsDigit(c) || c == '.')
            {
                number += c;
            }
            else
            {
                if (!string.IsNullOrEmpty(number))
                {
                    tokens.Add(number);
                    number = string.Empty;
                }
                if (!char.IsWhiteSpace(c))
                    tokens.Add(c.ToString());
            }
        }

        if (!string.IsNullOrEmpty(number))
            tokens.Add(number);

        return tokens;
    }

    private static List<string> InfixToPostfix(string infix)
    {
        var tokens = Tokenize(infix);
        var stack = new Stack<string>();
        var postfix = new List<string>();

        foreach (var token in tokens)
        {
            if (double.TryParse(token, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out _))
            {
                postfix.Add(token);
            }
            else if (IsOperator(token))
            {
                if (token == ")")
                {
                    while (stack.Peek() != "(")
                        postfix.Add(stack.Pop());
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && stack.Peek() != "(" && PriorityInfix(token) <= PriorityStack(stack.Peek()))
                        postfix.Add(stack.Pop());
                    stack.Push(token);
                }
            }
        }

        while (stack.Count > 0)
            postfix.Add(stack.Pop());

        return postfix;
    }

    private static bool IsOperator(string token) =>
        token is "^" or "/" or "*" or "%" or "+" or "-" or "(" or ")";

    private static int PriorityInfix(string op) => op switch
    {
        "^" => 4,
        "*" or "/" or "%" => 2,
        "+" or "-" => 1,
        "(" => 5,
        _ => 0
    };

    private static int PriorityStack(string op) => op switch
    {
        "^" => 3,
        "*" or "/" or "%" => 2,
        "+" or "-" => 1,
        "(" => 0,
        _ => 0
    };

    private static double Calculate(List<string> postfix)
    {
        var stack = new Stack<double>();

        foreach (var token in postfix)
        {
            if (double.TryParse(token, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double number))
            {
                stack.Push(number);
            }
            else
            {
                var op2 = stack.Pop();
                var op1 = stack.Pop();
                stack.Push(Calculate(op1, token, op2));
            }
        }

        return stack.Peek();
    }

    private static double Calculate(double op1, string op, double op2) => op switch
    {
        "*" => op1 * op2,
        "/" => op1 / op2,
        "^" => Math.Pow(op1, op2),
        "+" => op1 + op2,
        "-" => op1 - op2,
        "%" => op1 % op2,
        _ => throw new Exception("Invalid expression.")
    };
}