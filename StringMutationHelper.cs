using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MutableString
{
    public static class StringMutationHelper
    {
        private static readonly Action<string, int> _setLength;

        static StringMutationHelper()
        {
            // find the length field of the string class
            FieldInfo? stringLengthField = typeof(string).GetField("m_stringLength", BindingFlags.Instance | BindingFlags.NonPublic);

            // some versions of the library don't have the m prefix so try without 
            if (stringLengthField == null)
                stringLengthField = typeof(string).GetField("_stringLength", BindingFlags.Instance | BindingFlags.NonPublic);


            ParameterExpression mutableString = Expression.Parameter(typeof(string), "mutableString");
            ParameterExpression length = Expression.Parameter(typeof(int), "length");

            // create a lambda expression so we can set the length without generating garbage
            Expression<Action<string, int>> setLengthLambda = Expression.Lambda<Action<string, int>>(Expression.Assign(Expression.Field(mutableString, stringLengthField), length),
                mutableString,
                length
            );

            _setLength = setLengthLambda.Compile();
        }

        // set the string object to have a new length
        public static void SetLength(this string text, int length)
        {
            _setLength(text, length);
        }
    }
}