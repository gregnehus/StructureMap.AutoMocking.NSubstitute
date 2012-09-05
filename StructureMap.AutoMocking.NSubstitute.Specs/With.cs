using Machine.Specifications;

namespace StructureMap.AutoMocking.NSubstitute.Specs
{
    public abstract class With<TSubject>
       where TSubject : class
    {
        Establish context = () =>
        {
            Mocks = new NSubstituteAutoMocker<TSubject>();
        };

        public static TDependency For<TDependency>()
            where TDependency : class
        {
            return Mocks.Get<TDependency>();
        }

        public static TDependency Override<TDependency>(TDependency x)
            where TDependency : class
        {
            Mocks.Inject(x);
            return x;
        }

        public static NSubstituteAutoMocker<TSubject> Mocks { get; private set; }
        public static TSubject Subject { get { return Mocks.ClassUnderTest; } }
    }
}
