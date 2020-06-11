using Nibo.Util.Extensions;
using System.Collections.Generic;
using Xunit;

namespace Nibo.UnitTests.Util
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void ShouldReturnTrueIfCollectionIsEmpty()
        {
            var list = new List<string>();

            var result = list.IsNullOrEmpty();

            Assert.True(result);
        }

        [Fact]
        public void ShouldReturnTrueIfCollectionIsNull()
        {
            List<string> list = null;

            var result = list.IsNullOrEmpty();

            Assert.True(result);
        }


        [Fact]
        public void ShouldReturnFalseIfCollectionIsNotEmpty()
        {
            var list = new List<int> { 1,2,3 };

            var result = list.IsNullOrEmpty();

            Assert.False(result);
        }
    }
}
