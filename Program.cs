using System;

namespace calculadora
{

    class program
    {
        static string text;
        static char textChar
        {
            get => index < text.Length ? text[index] : (char)0;
            set => textChar = value;
        }
        static int index;

        static void Main()
        {
            //32+80-12+5*2
            while (true)
            {
                Console.Write("expression: ");
                text = Console.ReadLine();
                if (text.Equals("exit")) return;
                //text = "sin(cos(sin(cos(1))))/10+log((e^ln(10))^2)";//"(5*2)!/10+2^(2^(7-3))";///10+2^(2^(7-3))";
                index = 0;

                double result = parseSum();
                Console.WriteLine("expression: {0}\n result: {1}\n index: {2}\n\n", text, result, index);
            }
        }
        static double fact(double num)
        {
            if (num == 2)
                return 2;
            return num * fact(num - 1);
        }
        static double ParseOp(double num)
        {
            double result = num;
            char charOperator = textChar;

            switch (charOperator)
            {
                case '!': index++; result = fact(num); break;
                case '^': index++; result = Math.Pow(num, parseSum()); break;
            }
            return result;
        }
        static double parseNum()
        {
            double sum = 0;

            while (textChar >= '0' && textChar <= '9')
            {
                sum *= 10;
                sum += (double)(textChar - '0');
                index++;
            }
            return sum;
        }
        static double parseConst()
        {
            double num = 0;
            switch (textChar)
            {
                case 'e': num = 2.71828182846; break;
                case 'p':
                    index++;
                    if (textChar == 'i')
                        num = 3.141592653589793238462643383279;
                    break;
            }
            index++;
            return num;
        }
        static double parseGroup()
        {
            double result = 0;
            if (textChar >= '0' && textChar <= '9') result = parseNum();
            if ("epi".Contains(textChar)) result = parseConst();
            else if (textChar == '(')
            {
                index++;
                result = parseSum();
                index++;
            }



            return ParseOp(result);
        }
        static double parseFunc()
        {
            string[] funcs = { "sin", "cos", "log", "log2", "ln" };
            double result = 0;
            if ("scl".Contains(textChar))
            {
                int currIndex = index;
                int currFunc = -1;
                bool find = false;
                while (currFunc + 1 < funcs.Length && !find)
                {
                    int tempIndex = 0;
                    currFunc++;
                    while (tempIndex < funcs[currFunc].Length)
                    {
                        if (funcs[currFunc][tempIndex] == textChar)
                        {
                            tempIndex++;
                            index++;
                        }
                        else
                        {
                            index = currIndex;
                            break;
                        }

                        if (tempIndex == funcs[currFunc].Length) find = true;
                    }
                }
                result = parseGroup();
                if (find)
                {
                    switch (currFunc)
                    {
                        case 0: result = Math.Sin(result); break;
                        case 1: result = Math.Cos(result); break;
                        case 2: result = Math.Log10(result); break;
                        case 3: result = Math.Log2(result); break;
                        case 4: result = Math.Log(result); break;
                    }
                }
            }
            else result = parseGroup();

            return result;
        }
        static double parseMul()
        {
            double result = parseFunc();
            while (textChar == '*' || textChar == '/')
            {
                char charOperator = textChar;
                index++;
                double temp = parseFunc();
                switch (charOperator)
                {
                    case '*':
                        result *= temp;
                        break;
                    case '/':
                        result /= temp;
                        break;
                }
            }
            return result;
        }

        static double parseSum()
        {
            double result = parseMul();
            while (textChar == '+' || textChar == '-')
            {
                char charOperator = textChar;
                index++;
                double temp = parseMul();
                switch (charOperator)
                {
                    case '+':
                        result += temp;
                        break;
                    case '-':
                        result -= temp;
                        break;
                }
            }
            return result;
        }
    }
}
