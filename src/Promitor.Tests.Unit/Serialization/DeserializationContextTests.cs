using System.Collections.Generic;
using System.Reflection;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.FieldValidators;
using Xunit;

namespace Promitor.Tests.Unit.Serialization
{
    public class DeserializationContextTests
    {
        [Fact]
        public void GetSuggestions_NoFieldsConfigured_Empty()
        {
            // Arrange
            var context = new DeserializationContext<Person>(
                new HashSet<string>(), new List<FieldDeserializationInfo>());

            // Act
            var suggestions = context.GetSuggestions("job");

            // Assert
            Assert.Empty(suggestions);
        }

        [Fact]
        public void GetSuggestions_CloseEnoughSuggestion_ReturnsSuggestion()
        {
            // Arrange
            var fields = new List<FieldDeserializationInfo>
            {
                CreateField(typeof(Person).GetProperty(nameof(Person.Name))),
                CreateField(typeof(Person).GetProperty(nameof(Person.Age))),
                CreateField(typeof(Person).GetProperty(nameof(Person.Country))),
                CreateField(typeof(Person).GetProperty(nameof(Person.County)))
            };
            var context = new DeserializationContext<Person>(new HashSet<string>(), fields);

            // Act
            var suggestions = context.GetSuggestions("nmae");

            // Assert
            Assert.Collection(suggestions, suggestion => Assert.Equal("name", suggestion));
        }

        [Fact]
        public void GetSuggestions_CloseEnoughSuggestion_ReturnsAllSuggestions()
        {
            // Arrange
            var fields = new List<FieldDeserializationInfo>
            {
                CreateField(typeof(Person).GetProperty(nameof(Person.Name))),
                CreateField(typeof(Person).GetProperty(nameof(Person.Age))),
                CreateField(typeof(Person).GetProperty(nameof(Person.Country))),
                CreateField(typeof(Person).GetProperty(nameof(Person.County)))
            };
            var context = new DeserializationContext<Person>(new HashSet<string>(), fields);

            // Act
            var suggestions = context.GetSuggestions("count");

            // Assert
            Assert.Collection(suggestions,
                suggestion => Assert.Equal("country", suggestion),
                suggestion => Assert.Equal("county", suggestion));
        }

        [Fact]
        public void GetSuggestions_IgnoredFields_IncludesIgnoredFieldsInSuggestions()
        {
            // Arrange
            var fields = new List<FieldDeserializationInfo>
            {
                CreateField(typeof(Person).GetProperty(nameof(Person.Name))),
                CreateField(typeof(Person).GetProperty(nameof(Person.Age))),
                CreateField(typeof(Person).GetProperty(nameof(Person.Country))),
                CreateField(typeof(Person).GetProperty(nameof(Person.County)))
            };
            var ignoredFields = new HashSet<string> { "named" };
            var context = new DeserializationContext<Person>(ignoredFields, fields);

            // Act
            var suggestions = context.GetSuggestions("nam");

            // Assert
            Assert.Collection(suggestions,
                suggestion => Assert.Equal("name", suggestion),
                suggestion => Assert.Equal("named", suggestion));
        }

        private FieldDeserializationInfo CreateField(PropertyInfo propertyInfo)
        {
            return new FieldDeserializationInfo(propertyInfo, false, null, null, null, new List<IFieldValidator>());
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Country { get; set; }
            public string County { get; set; }
        }
    }
}