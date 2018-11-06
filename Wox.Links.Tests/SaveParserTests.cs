﻿using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Wox.Links.Parsers;
using Wox.Plugin;
using Xunit;

namespace Wox.Links.Tests
{
    public class SaveParserTests
    {
        private IStorage _storage;
        private SaveParser _saveParser;

        public SaveParserTests()
        {
            _storage = Substitute.For<IStorage>();
            _saveParser = new SaveParser(_storage);
        }

        [Theory]
        [InlineData("--save")]
        [InlineData("-s")]
        public void SaveTerms_ReturnTrueAndProposeToSave(string key)
        {
            _saveParser.TryParse(new[] {key, "https://some.com/link", "Shortcut"}, out List<Result> results).Should()
                .BeTrue();
            results.Should().HaveCount(1);

            var result = results[0];
            result.Title.Should().Be("Save the link as 'Shortcut'");
            result.SubTitle.Should().Be("https://some.com/link");

            result.Action(new ActionContext());
            _storage.Received(1).Set("Shortcut", "https://some.com/link");
        }

        [Theory]
        [InlineData("--save1")]
        [InlineData("-save")]
        [InlineData("-s2")]
        public void NotSaveKeyWord_ReturnFalse(string key)
        {
            _saveParser.TryParse(new[] {key, "https://some.com/link", "Shortcut"}, out List<Result> results).Should()
                .BeFalse();
            results.Should().HaveCount(0);
        }
    }
}