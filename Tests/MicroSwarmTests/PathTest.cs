using FluentAssertions;
using MicroSwarm.FileSystem;

namespace MicroSwarmTests
{
    [TestClass]
    public class TestSwarmDir
    {
        private SwarmDir _currentDir = SwarmDir.GetCurrentDirectory();

        [TestMethod]
        public void Last_Parent_Should_Be_Root()
        {
            SwarmDir? sut = _currentDir;
            while (sut.Parent != null && !sut.IsRoot)
            {
                sut = sut.Parent;
            }

            sut.Parent.Should().BeNull();
            sut.IsRoot.Should().BeTrue();
        }

        [TestMethod]
        public void Absolute_Path_Should_Match()
        {
            string sut = _currentDir.GetAbsolutePath();
            string absolute = Path.GetFullPath("./");
            sut.Should().Be(absolute);
        }

        [TestMethod]
        public void Should_Get_Dir_From_Absolute()
        {
            string absolute = "C:/";
            SwarmDir sut = _currentDir.GetDir(absolute);
            sut.Should().Be(_currentDir.GetRoot());
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void Get_Dir_Should_Fail_With_Non_Existent_Drive()
        {
            string absolute = "X:/";
            SwarmDir sut = _currentDir.GetDir(absolute);
        }
    }
}