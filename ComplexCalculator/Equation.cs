using System;
using System.Collections.Generic;



namespace ComplexCalculator
{
    // To add operator, add it's name in line 9, add it's sign in line 104 switch and add it's logic in line 238 switch
    enum operators { plus = 1, minus = 2, multiplication = 3, division = 4, power = 5 }

    // To add function, add it's name in line 13 and add logic in line 238 switch
    // Functions are only lowercase letters
    enum functions { sin = -1, cos = -2, tan = -3, sqrt = -4, degtorad = -5, round = -6 }

    struct node
    {
        public int priority;
        public LinkedListNode<double> where;
        public int whatOperator;

        public node(int priority, LinkedListNode<double> where, int whatOperator)
        {
            this.priority = priority;
            this.where = where;
            this.whatOperator = whatOperator;
        }


    }


    public class Equation
    {
        public string equationText { get; private set; }
        LinkedList<double> equationsList;
        List<node> operatorsList;
        public bool error { get; private set; }

        public Equation(string equation)
        {
            equationsList = new LinkedList<double>();
            operatorsList = new List<node>();
            ChangeEquation(equation);
        }

        public Equation()
        {
            equationsList = new LinkedList<double>();
            operatorsList = new List<node>();
            error = false;
        }

        public void ChangeEquation(string equation)
        {
            equationText = equation;
            error = false;
            TextToList();
        }

        private void TextToList()
        {
            LinkedListNode<double> last = new LinkedListNode<double>(-1);
            int currPriority = 0;
            string numb = "", command = "";
            bool lastnumb = false;
            bool commandinprogress = false;
            bool lastoperator = false;


            for (int i = 0; i < equationText.Length; i++)
            {
                if ((equationText[i] >= '0' && equationText[i] <= '9') || equationText[i] == '.')
                {

                    if (command == "")
                    {
                        numb += equationText[i];
                    }
                    else
                    {
                        command += equationText[i];
                    }
                    lastnumb = true;
                    lastoperator = false;
                }
                else if (char.ToLower(equationText[i]) >= 'a' && char.ToLower(equationText[i]) <= 'z')
                {
                    commandinprogress = true;
                    command += char.ToLower(equationText[i]);
                    lastnumb = false;
                    lastoperator = false;
                }
                else
                {
                    if (numb != "")
                    {
                        double temp = 0;
                        double.TryParse(numb, out temp);
                        last = new LinkedListNode<double>(temp);
                        equationsList.AddLast(last);
                        numb = "";
                    }
                    LinkedListNode<double> templ;
                    switch (equationText[i])
                    {
                        case '(':
                            if (command != "")
                            {
                                functions func;
                                commandinprogress = false;
                                if (Enum.TryParse(command, out func))
                                {
                                    templ = new LinkedListNode<double>(0);
                                    equationsList.AddLast(templ);
                                    operatorsList.Add(new node(currPriority + 9, templ, (int)func));
                                }
                                else
                                {
                                    error = true;
                                    return;
                                }
                                command = "";
                            }
                            if (lastnumb)
                            {
                                Console.WriteLine(lastnumb);
                                templ = new LinkedListNode<double>(0);
                                equationsList.AddLast(templ);
                                operatorsList.Add(new node(currPriority + 2, templ, (int)operators.multiplication));
                            }
                            currPriority += 10;
                            break;
                        case ')':
                            if (currPriority == 0)
                            {
                                error = true;
                                return;
                            }
                            currPriority -= 10;
                            break;
                        case '+':
                            if (lastoperator)
                            {
                                error = true;
                                return;
                            }
                            templ = new LinkedListNode<double>(0);
                            equationsList.AddLast(templ);
                            operatorsList.Add(new node(currPriority + 1, templ, (int)operators.plus));
                            break;
                        case '-':
                            if (lastoperator || i == 0)
                            {
                                numb += equationText[i];
                            }
                            else
                            {
                                templ = new LinkedListNode<double>(0);
                                equationsList.AddLast(templ);
                                operatorsList.Add(new node(currPriority + 1, templ, (int)operators.minus));
                            }
                            break;
                        case '*':
                            if (lastoperator)
                            {
                                error = true;
                                return;
                            }
                            templ = new LinkedListNode<double>(0);
                            equationsList.AddLast(templ);
                            operatorsList.Add(new node(currPriority + 2, templ, (int)operators.multiplication));
                            break;
                        case '/':
                            if (lastoperator)
                            {
                                error = true;
                                return;
                            }
                            templ = new LinkedListNode<double>(0);
                            equationsList.AddLast(templ);
                            operatorsList.Add(new node(currPriority + 2, templ, (int)operators.division));
                            break;
                        case '^':
                            if (lastoperator)
                            {
                                error = true;
                                return;
                            }
                            templ = new LinkedListNode<double>(0);
                            equationsList.AddLast(templ);
                            operatorsList.Add(new node(currPriority + 3, templ, (int)operators.power));
                            break;
                        default:
                            break;
                    }
                    if (commandinprogress)
                    {
                        error = true;
                        return;
                    }
                    lastnumb = false;
                    lastoperator = true;
                }
            }
            if (numb != "")
            {
                double temp = 0;
                double.TryParse(numb, out temp);
                last = new LinkedListNode<double>(temp);
                equationsList.AddLast(last);
                numb = "";
            }

            for (int i = 0; i < operatorsList.Count; i++)
            {
                for (int j = operatorsList.Count - 1; j > i; j--)
                {
                    if (operatorsList[j - 1].priority < operatorsList[j].priority)
                    {
                        node temp = operatorsList[j];
                        operatorsList[j] = operatorsList[j - 1];
                        operatorsList[j - 1] = temp;
                    }
                }
            }

        }

        public bool Solve(out double result)
        {
            if (error)
            {
                result = 0;
                return false;
            }

            foreach (node n in operatorsList)
            {
                switch (n.whatOperator)
                {
                    case (int)operators.plus:
                        if (n.where.Previous == null || n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = n.where.Next.Value + n.where.Previous.Value;
                        equationsList.Remove(n.where.Next);
                        equationsList.Remove(n.where.Previous);
                        break;
                    case (int)operators.minus:
                        if (n.where.Previous == null || n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = n.where.Previous.Value - n.where.Next.Value;
                        equationsList.Remove(n.where.Next);
                        equationsList.Remove(n.where.Previous);
                        break;
                    case (int)operators.multiplication:
                        if (n.where.Previous == null || n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = n.where.Next.Value * n.where.Previous.Value;
                        equationsList.Remove(n.where.Next);
                        equationsList.Remove(n.where.Previous);
                        break;
                    case (int)operators.division:
                        if (n.where.Previous == null || n.where.Next == null || n.where.Next.Value == 0)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = n.where.Previous.Value / n.where.Next.Value;
                        equationsList.Remove(n.where.Next);
                        equationsList.Remove(n.where.Previous);
                        break;
                    case (int)operators.power:
                        if (n.where.Previous == null || n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = Math.Pow(n.where.Previous.Value, n.where.Next.Value);
                        equationsList.Remove(n.where.Next);
                        equationsList.Remove(n.where.Previous);
                        break;
                    case (int)functions.sin:
                        if (n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = Math.Sin(n.where.Next.Value);
                        equationsList.Remove(n.where.Next);
                        break;
                    case (int)functions.cos:
                        if (n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = Math.Cos(n.where.Next.Value);
                        equationsList.Remove(n.where.Next);
                        break;
                    case (int)functions.tan:
                        if (n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = Math.Tan(n.where.Next.Value);
                        equationsList.Remove(n.where.Next);
                        break;
                    case (int)functions.sqrt:
                        if (n.where.Next == null || n.where.Next.Value < 0)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = Math.Sqrt(n.where.Next.Value);
                        equationsList.Remove(n.where.Next);
                        break;
                    case (int)functions.degtorad:
                        if (n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = n.where.Next.Value / 180.0 * Math.PI;
                        equationsList.Remove(n.where.Next);
                        break;
                    case (int)functions.round:
                        if (n.where.Next == null)
                        {
                            result = 0;
                            return false;
                        }
                        n.where.Value = Math.Round(n.where.Next.Value);
                        equationsList.Remove(n.where.Next);
                        break;
                }
            }
            if (equationsList.First == null || equationsList.Count > 1)
            {
                result = 0;
                return false;
            }
            result = equationsList.First.Value;
            return true;
        }


    }
}
