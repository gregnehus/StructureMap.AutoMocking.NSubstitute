using Machine.Specifications;
using NSubstitute;

namespace StructureMap.AutoMocking.NSubstitute.Specs
{
    [Subject(typeof(NSubstituteAutoMockerBuilder))]
    public class When_automocking_object_with_dependencies_and_get_5_is_called : With<TestObjectWithDependencies>
    {
        static int _result;
        Establish context = () => For<IProvideTheNumber5>().Get5().Returns(5);

        Because of = () => _result = Subject.Get5();

        It should_request_value_from_mocked_number_5_provider_object = () => For<IProvideTheNumber5>().Received().Get5();
        It should_return_the_value_returned_by_mock_provider = () => _result.ShouldEqual(5);
    }
}