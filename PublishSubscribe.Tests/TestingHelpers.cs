using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.Tests
{
    public static class TestingHelpers
    {
        public static IEnumerable<object[]> CreateObjectArrayOfObjects(string name, Type type)
        {
            return (type.GetProperty(name).GetValue(Activator.CreateInstance(type), null) as IEnumerable<object>)
                .Select(x =>
                {
                    return new object[] { x };
                });
        }
    }
}
