namespace DotMSN
{
    using System;
    using System.Text.RegularExpressions;

    public class RegularExpressionException : Exception
    {
        // Methods
        public RegularExpressionException(string message) : base(message)
        {
        }

        public RegularExpressionException(Regex regularExpression, Exception innerException) : base(string.Concat("Regular expression failed: ", regularExpression.GetType().ToString()), innerException)
        {
        }

        public RegularExpressionException(Regex regularExpression, string target, Exception innerException) : base(string.Concat(textArray1), innerException)
        {
            string[] textArray1 = new string[5];
            textArray1[0] = "Regular expression failed: ";
            textArray1[1] = regularExpression.GetType().ToString();
            textArray1[2] = " working on \'";
            textArray1[3] = target;
            textArray1[4] = "\'";
        }

    }}

