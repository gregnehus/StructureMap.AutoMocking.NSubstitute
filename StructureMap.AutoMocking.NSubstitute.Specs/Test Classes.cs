namespace StructureMap.AutoMocking.NSubstitute.Specs
{

    public class TestObjectWithNoDependencies
    {
        public string GetHelloWorld()
        {
            return "hello world";
        }        
    }
    public class TestObjectWithDependencies
    {
        readonly IProvideTrueValue _trueValue;
        readonly IProvideTheNumber5 _number5;

        public TestObjectWithDependencies(IProvideTrueValue trueValue, IProvideTheNumber5 number5)
        {
            _trueValue = trueValue;
            _number5 = number5;
        }

        public bool GetTrue()
        {
            return _trueValue.GetTrue();
        }

        public int Get5()
        {
            return _number5.Get5();
        }
    }

    public class TestObjectWithConcreteDependency
    {
        readonly string _someString;

        public TestObjectWithConcreteDependency(string someString)
        {
            _someString = someString;
        }

        public string GetString()
        {
            return _someString;
        }
    }

    public interface IProvideTrueValue
    {
        bool GetTrue();
    }

    public interface IProvideTheNumber5
    {
        int Get5();
    }
}
