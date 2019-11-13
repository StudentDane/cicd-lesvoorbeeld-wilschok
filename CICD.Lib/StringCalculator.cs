using System;

namespace CICD.Lib
{
    public class StringCalculator
    {
        public int Add(string toAdd)
        {
            if (toAdd == null) throw new ArgumentException("Numbers to add can not be null!");

            if (toAdd != String.Empty)
            {
                string[] numbers = toAdd.Split(',');
                int sum = 0;
                foreach (string number in numbers)
                {
                    try
                    {
                        int numberToAdd = Convert.ToInt32(number);
                        sum += numberToAdd;
                    }
                    catch
                    {
                        throw new ArgumentException("One of the items in not a number!");
                    }
                }
                if (sum > 1000)
                    throw new OverflowException("Number to big. Max=1000.");
                return sum;
            }
            return 0;
        }
    }
}
