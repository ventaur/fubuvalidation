using System.Diagnostics;

using FubuMVC.Core;
using FubuMVC.Validation.IntegrationTesting.Ajax;

using FubuTestingSupport;

using FubuValidation;

using NUnit.Framework;


namespace FubuMVC.Validation.IntegrationTesting
{
    [TestFixture]
    public class MissingValueTester : ValidationHarness
    {
        protected override void configure(FubuRegistry registry)
        {
            registry.Actions.IncludeType<MissingValueEndpoint>();
            registry.Import<FubuMvcValidation>();
        }

        private JsonResponse theContinuation
        {
            get
            {
                var badInput = new UnexpectedInputWithoutName
                {
                    Email = "someone@nowhere.com"
                };
                var response = endpoints.PostJson(new ExpectedInputWithName(), badInput);

                try
                {
                    return response.ReadAsJson<JsonResponse>();
                }
                catch
                {
                    Debug.WriteLine(response.ReadAsText());
                    throw;
                }
            }
        }

        [Test]
        public void validation_fails_required_without_stackoverflow()
        {
            var response = theContinuation;
            response.success.ShouldBeFalse();
            response.errors.ShouldNotBeNull();
            response.errors[0].message.ShouldEqual(ValidationKeys.Required.DefaultValue);
        }
    }


    public class MissingValueEndpoint
    {
        public ExpectedInputWithName post_missing_value(ExpectedInputWithName input)
        {
            return input;
        }
    }


    public class ExpectedInputWithName
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UnexpectedInputWithoutName
    {
        public string Email { get; set; }
    }
}