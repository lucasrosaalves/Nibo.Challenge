using Nibo.Domain.Commands;
using System.Collections.Generic;
using Xunit;

namespace Nibo.UnitTests.Domain.Commands
{
    public class ImportExtractFilesCommandTests
    {
        [Fact]
        public void ShouldReturnCommandInvalidIfFilesNull()
        {
            var command = new ImportExtractFilesCommand(null);


            Assert.False(command.IsValid());
        }

        [Fact]
        public void ShouldReturnCommandInvalidIfFilesEmpty()
        {
            var command = new ImportExtractFilesCommand(new List<List<string>>());


            Assert.False(command.IsValid());
        }

        [Fact]
        public void ShouldReturnCommandValidIfFilesHasValue()
        {
            var file1 = new List<string> { "1" };
            var file2 = new List<string> { "2" };


            var command = new ImportExtractFilesCommand(new List<List<string>> { file1 , file2 });


            Assert.True(command.IsValid());
        }
    }
}
