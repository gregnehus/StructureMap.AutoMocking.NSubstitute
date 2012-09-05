using Machine.Specifications;

namespace StructureMap.AutoMocking.NSubstitute.Specs
{
    [Subject(typeof(NSubstituteAutoMockerBuilder))]
    public class When_automocking_object_with_no_dependencies_and_method_is_called : With<TestObjectWithNoDependencies>
    {
        static string _result;
        Because of = () => _result = Subject.GetHelloWorld();
        It should_return_expected_result = () => _result.ShouldEqual("hello world");
    }
}