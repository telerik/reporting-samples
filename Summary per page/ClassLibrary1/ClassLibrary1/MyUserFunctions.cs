using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Reporting.Expressions;

namespace Telerik.ReportServer.SampleObjectDataSource
{
    //public static class MyUserFunctions
    //{
    //    public static string HelloWorldFunction()
    //    {
    //        return "Hello, World!";
    //    }
    //}


    //public static class PageContextUserFunctions
    //{
    //    double sum

    //    public static string PageFooterSumUntilNow(double value)
    //    {
    //        return "Hello, World!";
    //    }
    //}

    [AggregateFunction(Description = "Special sum aggregate. Output: (value1, value2, ...)", Name = "PageFooterSumUntilNow")]
    class PageFooterSumUntilNow : IAggregateFunction
    {
        [ThreadStatic]
        static decimal result;

        [ThreadStatic]
        static string currentGroupByValue;

        public void Accumulate(object[] values)
        {
            // The aggregate function expects one parameter
            object value = values[0];

            // null values are not aggregated
            if (null == value)
            {
                return;
            }


            var groupByValue = (string)values[1];

            if (groupByValue != currentGroupByValue)
            {
                //Debug.WriteLine($"RESETTING GROUP {currentGroupByValue} TO {groupByValue}");
                currentGroupByValue = groupByValue;
                result = 0M;
            }

            //Debug.WriteLine($"ACCUMULATING {(decimal)value} INTO {result}");



            result += (decimal)value;
        }

        public object GetValue()
        {
            return result;
        }

        public void Init()
        {
            // Add aggregate function initialization code here if needed
            //result = 0M;
        }

        public void Merge(IAggregateFunction aggregateFunction)
        {
            //PageFooterSumUntilNow aggregate = (PageFooterSumUntilNow)aggregateFunction;

            //this.result += aggregate.result;
        }
    }

    [AggregateFunction(Description = "Special sum aggregate. Output: (value1, value2, ...)", Name = "PageHeaderSumFromPrevPage")]
    class PageHeaderSumFromPrevPage : IAggregateFunction
    {
        [ThreadStatic]
        static decimal result;

        [ThreadStatic]
        static string currentGroupByValue;

        [ThreadStatic]
        static int currentPage = -1;

        [ThreadStatic]
        static List<object[]> currentPageValues;


        public void Accumulate(object[] values)
        {
            var page = (int)values[2];
            Debug.WriteLine($"NEW PAGE {page} CURRENT PAGE {currentPage}");
            if (page != currentPage)
            {
                currentPage = page;

                for (int index = 0; index < currentPageValues.Count; index++)
                {
                    this.AccumulateCore(currentPageValues[index]);
                }
                currentPageValues.Clear();
            }

            currentPageValues.Add(values);
        }

        void AccumulateCore(object[] values)
        {
            // The aggregate function expects one parameter
            object value = values[0];

            // null values are not aggregated
            if (null == value)
            {
                return;
            }


            var groupByValue = (string)values[1];

            if (groupByValue != currentGroupByValue)
            {
                //Debug.WriteLine($"RESETTING GROUP {currentGroupByValue} TO {groupByValue}");
                currentGroupByValue = groupByValue;
                result = 0M;
            }

            //Debug.WriteLine($"ACCUMULATING {(decimal)value} INTO {result}");

            result += (decimal)value;
        }

        public object GetValue()
        {
            if (currentPage == 1)
            {
                return 0;
            }

            return result;
        }

        public void Init()
        {
            if (currentPageValues == null)
            {
                currentPageValues = new List<object[]>();
            }

            // Add aggregate function initialization code here if needed
            //result = 0M;
        }

        public void Merge(IAggregateFunction aggregateFunction)
        {
            //PageFooterSumUntilNow aggregate = (PageFooterSumUntilNow)aggregateFunction;

            //this.result += aggregate.result;
        }
    }
}

