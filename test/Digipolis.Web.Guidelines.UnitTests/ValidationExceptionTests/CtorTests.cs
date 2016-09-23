﻿using System;
using System.Collections.Generic;
using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Digipolis.Web.Guidelines.UnitTests.ValidationExceptionTests
{
    public class CtorTests
    {
        [Fact]
        private void ModelStateIsSet()
        {
            var modelstate = new Dictionary<string, IEnumerable<string>> {{"aKey", new[] {"aMessage"}}};
            var ex = new ValidationException(modelstate);

            Assert.Equal(1, ex.ModelState.Count);
            Assert.Collection(ex.ModelState.Keys, x => Assert.Equal("aKey", x));
            Assert.Collection(ex.ModelState.Values, x => Assert.Collection(x, y => Assert.Equal("aMessage", y)));
        }

        [Fact]
        private void ModelStateNullIsAllowed()
        {
            var ex = new ValidationException(null, "aMessage", new Exception());
            Assert.NotNull(ex.ModelState);
        }

        [Fact]
        private void ModelStateIsInitialized()
        {
            var ex = new ValidationException("aMessage");
            Assert.NotNull(ex.ModelState);
        }

        [Fact]
        private void MessageIsSet()
        {
            var modelstate = new Dictionary<string, IEnumerable<string>>();
            var ex = new ValidationException(modelstate, "aMessage");
            Assert.Equal("aMessage", ex.Message);
        }

        [Fact]
        private void MessageIsSet2()
        {
            var ex = new ValidationException("aMessage");
            Assert.Equal("aMessage", ex.Message);
        }

        [Fact]
        private void ExceptionIsSet()
        {
            var modelstate = new Dictionary<string, IEnumerable<string>>();
            var innerEx = new Exception();
            var ex = new ValidationException(modelstate, "aMessage", innerEx);
            Assert.Same(innerEx, ex.InnerException);
        }

        [Fact]
        private void ExceptionIsSet2()
        {
            var innerEx = new Exception();
            var ex = new ValidationException("aMessage", innerEx);
            Assert.Same(innerEx, ex.InnerException);
        }

        [Fact]
        private void MessageIsAddedToModelState()
        {
            var modelstate = new Dictionary<string, IEnumerable<string>>();
            var ex = new ValidationException(modelstate, "aMessage");
            Assert.Equal(1, ex.ModelState.Count);
            Assert.Collection(ex.ModelState.Keys, x => Assert.Empty(x));
            Assert.Collection(ex.ModelState.Values, x => Assert.Collection(x, y => Assert.Equal("aMessage", y)));
        }

        [Fact]
        private void MessageIsAddedToModelState2()
        {
            var ex = new ValidationException("aMessage");
            Assert.Equal(1, ex.ModelState.Count);
            Assert.Collection(ex.ModelState.Keys, x => Assert.Empty(x));
            Assert.Collection(ex.ModelState.Values, x => Assert.Collection(x, y => Assert.Equal("aMessage", y)));
        }

    }
}
