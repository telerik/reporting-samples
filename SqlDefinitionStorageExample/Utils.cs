namespace SqlDefinitionStorageExample
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Xml.XPath;

    class SharedDataSourceDescriptionHelper
    {
        static readonly string descriptionPropertyName = PropertyUtils.ExtractPropertyName(() => new Telerik.WebReportDesigner.Services.Models.SharedDataSourceModel().Description);

        public static string Read(byte[] dsDefinitionBytes)
        {
            using (var ms = new MemoryStream(dsDefinitionBytes))
            {
                return SharedDataSourceDescriptionHelper.ReadDescription(ms);
            }
        }

        public static string ReadDescription(string dsDefinition)
        {
            using (var reader = new StringReader(dsDefinition))
            {
                return SharedDataSourceDescriptionHelper.ReadDescription(new XPathDocument(reader));
            }
        }

        public static string ReadDescription(Stream stream)
        {
            return SharedDataSourceDescriptionHelper.ReadDescription(new XPathDocument(stream));
        }

        static string ReadDescription(XPathDocument document)
        {
            var navigator = document.CreateNavigator();
            navigator.MoveToFirstChild();

            return navigator.GetAttribute(descriptionPropertyName, string.Empty);
        }
    }

    public static class PropertyUtils
    {
        public static string ExtractPropertyName(Expression<Func<object>> exp)
        {
            var propertyName = String.Empty;
            if (exp is System.Linq.Expressions.LambdaExpression lambda)
            {
                var memberExp = lambda.Body as MemberExpression;
                if (null == memberExp)
                {
                    if (lambda.Body is UnaryExpression unary)
                    {
                        memberExp = unary.Operand as MemberExpression;
                    }
                }

                if (null != memberExp)
                {
                    propertyName = memberExp.Member.Name;
                }
            }
            return propertyName;
        }
    }

    public static class TaskExtensions
    {
        public static T ToResult<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }

        public static void ToResult(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        public static Task<TResult> ToTask<TResult>(this TResult t) where TResult : class
        {
            return Task.FromResult(t);
        }
    }
}
