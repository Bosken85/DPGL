using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Digipolis.Web.Guidelines.UnitTests.ValidationExceptionTests
{
    public class AddMessageTests
    {
        [Fact]
        private void MessageIsAddedWithEmptyKey()
        {
            var ex = new ValidationException(new ModelStateDictionary());
            ex.AddMessage("aMessage");
            Assert.Equal(1, ex.ModelState.Count);
            Assert.Collection(ex.ModelState.Keys, x => Assert.Empty(x));
            Assert.Collection(ex.ModelState.Values, x => Assert.Collection(x, y => Assert.Equal("aMessage", y)));
        }

        [Fact]
        private void TwoMessagesAreAddedWithEmptyKey()
        {
            var ex = new ValidationException(new ModelStateDictionary());
            ex.AddMessage("aMessage1");
            ex.AddMessage("aMessage2");
            Assert.Equal(1, ex.ModelState.Count);
            Assert.Collection(ex.ModelState.Keys, x => Assert.Empty(x));
            Assert.Collection(ex.ModelState.Values, x => Assert.Collection(x, y => Assert.Equal("aMessage1", y), 
                                                                              y => Assert.Equal("aMessage2", y)));
        }

        [Fact]
        private void MessageIsAddedWithKey()
        {
            var ex = new ValidationException(new ModelStateDictionary());
            ex.AddMessage("aKey", "aMessage");
            Assert.Equal(1, ex.ModelState.Count);
            Assert.Collection(ex.ModelState.Keys, x => Assert.Equal("aKey", x));
            Assert.Collection(ex.ModelState.Values, x => Assert.Collection(x, y => Assert.Equal("aMessage", y)));
        }

        [Fact]
        private void TwoMessagesAreAddedWithTwoKeys()
        {
            var ex = new ValidationException(new ModelStateDictionary());
            ex.AddMessage("key1", "aMessage1");
            ex.AddMessage("key2", "aMessage2");
            Assert.Equal(2, ex.ModelState.Count);
            Assert.Collection(ex.ModelState.Keys, x => Assert.Equal("key1", x), x => Assert.Equal("key2", x));
            Assert.Collection(ex.ModelState.Values, x => Assert.Collection(x, y => Assert.Equal("aMessage1", y)),
                                                    x => Assert.Collection(x, y => Assert.Equal("aMessage2", y)));
        }

        [Fact]
        private void NullMessageIsNotAdded()
        {
            var ex = new ValidationException(new ModelStateDictionary());
            ex.AddMessage(null);
            Assert.Empty(ex.ModelState);
        }
    }
}
