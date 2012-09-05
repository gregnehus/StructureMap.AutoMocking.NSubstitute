using System;
using Machine.Specifications;
using NSubstitute;

namespace StructureMap.AutoMocking.NSubstitute.Specs
{
    [Subject(typeof(NSubstituteAutoMockerBuilder))]
    public class When_automocking_object_with_a_concrete_dependency : With<TestObjectWithConcreteDependency>
    {
        static string _result;
        static Exception _exception;
        Establish context = () => For<IProvideTheNumber5>().Get5().Returns(5);

        Because of = () => _exception = Catch.Exception(() => _result = Subject.GetString());

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
        It should_not_return_anything = () => _result.ShouldBeNull();
    }
}