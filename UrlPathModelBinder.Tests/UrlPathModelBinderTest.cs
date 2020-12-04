using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using FluentAssertions;
using Xunit;

namespace UrlPathModelBinder.Tests
{
    public class TestPathModelBinder<T> : PathModelBinder<T> where T: new()
    {
        private readonly string _path;

        public TestPathModelBinder(string path) : base(null)
        {
            _path = path;
        }

        public override string GetPath(ModelBindingContext bindingContext) => _path;
    }

    public class UrlPathModelBinderTest
    {
        class DerivedStringProp : StringProp {  }
        class StringProp { public string Name { get; set; } }

        class NumericProp<T> { public T Id { get; set; } }

        [Fact] public async Task When_Model_Has_Property_OfType_String_It_Is_Bound() => await StringPropertyTest<StringProp>();

        [Fact] public async Task When_Model_Has_Derived_Property_OfType_String_It_Is_Bound() => await StringPropertyTest<DerivedStringProp>();

        private static async Task StringPropertyTest<T>() where T: StringProp, new()
        {
            var binder = new TestPathModelBinder<T>("controller/action/Name/Dmitry");
            await binder.BindModelAsync(new Mock<ModelBindingContext>().Object);
            binder.Model.Name.Should().Be("Dmitry");
        }

        [Fact] public async Task When_Model_Has_Property_OfType_Int32_It_Is_Bound() => await NumericPropertyTest<int>();
        [Fact] public async Task When_Model_Has_Property_OfType_UInt32_It_Is_Bound() => await NumericPropertyTest<uint>();
        [Fact] public async Task When_Model_Has_Property_OfType_Int64_It_Is_Bound() => await NumericPropertyTest<long>();
        [Fact] public async Task When_Model_Has_Property_OfType_UInt64_It_Is_Bound() => await NumericPropertyTest<ulong>();
        [Fact] public async Task When_Model_Has_Property_OfType_Int16_It_Is_Bound() => await NumericPropertyTest<short>();
        [Fact] public async Task When_Model_Has_Property_OfType_UInt16_It_Is_Bound() => await NumericPropertyTest<ushort>();

        private static async Task NumericPropertyTest<T>() where T: new()
        {
            var binder = new TestPathModelBinder<NumericProp<T>>("controller/action/Id/88");
            await binder.BindModelAsync(new Mock<ModelBindingContext>().Object);
            dynamic dynamicModel = binder.Model;
            T id = (T) dynamicModel.Id;
            id.Should().Be(88);
        }
    }
}