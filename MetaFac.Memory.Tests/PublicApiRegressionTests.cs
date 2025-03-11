using System.Threading.Tasks;
using Xunit;
using PublicApiGenerator;
using VerifyXunit;
using Shouldly;

namespace MetaFac.Memory.Tests
{
    public class PublicApiRegressionTests
    {
        [Fact]
        public void VersionCheck()
        {
            ThisAssembly.AssemblyVersion.ShouldBe("2.0.0.0");
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