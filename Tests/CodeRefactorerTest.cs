using Xunit;

namespace WheelzyProject.Tests
{
    public class CodeRefactorerTest
    {
        [Fact]
        public void Appends_Async_To_Async_Methods()
        {
            var code = """
        class C {
            public async System.Threading.Tasks.Task Save() { await System.Threading.Tasks.Task.Yield(); }
            public async System.Threading.Tasks.Task SaveAsync() { await System.Threading.Tasks.Task.Yield(); }
        }
        """;

            var outText = CodeRefactorer.ProcessText(code);
            Assert.Contains("SaveAsync()", outText);
            Assert.Contains("SaveAsync()", outText);
        }

        [Theory]
        [InlineData("CustomerVm x;", "CustomerVM x;")]
        [InlineData("FooVms x;", "FooVMs x;")]
        [InlineData("OrderDto x;", "OrderDTO x;")]
        [InlineData("ListDtos x;", "ListDTOs x;")]
        public void Uppercases_Suffixes(string input, string expected)
        {
            var code = $"class C {{ {input} }}";
            var outText = CodeRefactorer.ProcessText(code);
            Assert.Contains(expected, outText);
        }

        [Fact]
        public void Adds_Blank_Line_Between_Methods()
        {
            var code = "class C { void A() {}\n    void B() {} }";
            var outText = CodeRefactorer.ProcessText(code).Replace("\r\n", "\n");
            Assert.Contains("A() {}\n\n    void B()", outText);
        }
    }
}