using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using PublicApiGenerator;
using VerifyXunit;

namespace MetaFac.Memory.Tests
{
    public class PublicApiRegressionTests
    {
        [Fact]
        public void VersionCheck()
        {
            ThisAssembly.AssemblyVersion.Should().Be("1.6.0.0");
        }

        [Fact]
        public async Task CheckPublicApi()
        {
            // act
            var options = new ApiGeneratorOptions()
            {
                IncludeAssemblyAttributes = false
            };
            string currentApi = ApiGenerator.GeneratePublicApi(typeof(Octets).Assembly, options);

            // assert
            await Verifier.Verify(currentApi);
        }

    }
}