﻿using System;
using FluentAssertions;
using Serilog.Sinks.OpenSearch.Tests.Stubs;
using Xunit;

namespace Serilog.Sinks.OpenSearch.Tests.Templating
{
    public class DoNotRegisterIfTemplateExistsTests : OpenSearchSinkTestsBase
    {
        private void DoRegister()
        {
            _templateExistsReturnCode = 200;

            _options.AutoRegisterTemplate = true;
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.OpenSearch(_options);

            var logger = loggerConfig.CreateLogger();
            using (logger as IDisposable)
            {
                logger.Error("Test exception. Should not contain an embedded exception object.");
            }
        }

        [Fact]
        public void WhenTemplateExists_ShouldNotSendAPutTemplate()
        {
            DoRegister();
            _seenHttpPosts.Should().NotBeNullOrEmpty().And.HaveCount(1);
            _seenHttpHeads.Should().NotBeNullOrEmpty().And.HaveCount(1);
            _seenHttpPuts.Should().BeNullOrEmpty();
        }
    }
}