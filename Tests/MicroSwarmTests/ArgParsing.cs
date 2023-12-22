using FluentAssertions;
using MicroSwarm;

namespace MicroSwarmTests
{
    [TestClass]
    public class TryParseArgs
    {
        private readonly SwarmArgParser _sut = new();

        [TestMethod]
        public void Parse_Short_Help()
        {
            string[] args = ["-h"];
            var input = _sut.Parse(args);
            input.HelpRequested.Should().BeTrue();
            input.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void Parse_Long_Help()
        {
            string[] args = ["--help"];
            var input = _sut.Parse(args);
            input.HelpRequested.Should().BeTrue();
            input.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void Parse_Short_Version()
        {
            string[] args = ["-v"];
            var input = _sut.Parse(args);
            input.VersionRequested.Should().BeTrue();
            input.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void Parse_Long_Version()
        {
            string[] args = ["--version"];
            var input = _sut.Parse(args);
            input.VersionRequested.Should().BeTrue();
            input.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void Parse_Short_Output()
        {
            string dir = "dir";
            string[] args = ["-o", dir];
            var input = _sut.Parse(args);
            input.OutputDir.Should().Be(dir);
            input.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void Parse_Long_Output()
        {
            string dir = "dir";
            string[] args = ["--output", dir];
            var input = _sut.Parse(args);
            input.OutputDir.Should().Be(dir);
            input.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void No_Output_Argument_Should_Cause_Error()
        {
            string[] args = ["-o"];
            var input = _sut.Parse(args);
            input.OutputDir.Should().BeNull();
            input.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void Multiple_Outputs_Should_Cause_Error()
        {
            string dir = "dir";
            string dir2 = "dir2";
            string[] args = ["--output", dir, "-o", dir2];
            var input = _sut.Parse(args);
            input.OutputDir.Should().Be(dir);
            input.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void Unknown_Option_Should_Cause_Error()
        {
            string[] args = ["-z"];
            var input = _sut.Parse(args);
            input.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void NonOption_Should_Be_Files()
        {
            string[] args = ["foo"];
            var input = _sut.Parse(args);
            input.Errors.Should().BeEmpty();
            input.Files.Count.Should().Be(args.Length);
            input.Files[0].Should().Be(args[0]);
        }

        [TestMethod]
        public void Multiple_NonOption_Should_Be_Files()
        {
            string[] args = ["foo", "bar"];
            var input = _sut.Parse(args);
            input.Errors.Should().BeEmpty();
            input.Files.Count.Should().Be(args.Length);
            input.Files[0].Should().Be(args[0]);
            input.Files[1].Should().Be(args[1]);
        }

        [TestMethod]
        public void Should_Parse_All_Options()
        {
            string dir = "dir";
            string in1 = "foo";
            string in2 = "bar";
            string[] args = [in1, "-o", dir, "-h", "-v", in2];
            var input = _sut.Parse(args);
            input.Errors.Should().BeEmpty();
            input.HelpRequested.Should().BeTrue();
            input.VersionRequested.Should().BeTrue();
            input.OutputDir.Should().Be(dir);
            input.Files[0].Should().Be(in1);
            input.Files[1].Should().Be(in2);
        }
    }
}