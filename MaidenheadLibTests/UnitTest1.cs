using FluentAssertions;
using MaidenheadLib;
using Xunit;

namespace MaidenheadLibTests
{
    public class UnitTest1
    {
        [Fact]
        public void LocatorToLatLng_0()
        {
            var (lat, lon) = MaidenheadLocator.LocatorToLatLng("IO91lk");
            lat.Should().Be(51.4375);
            lon.Should().Be(-1.0416666666666572);
        }

        [Fact]
        public void LocatorToLatLng_1()
        {
            var (lat, lon) = MaidenheadLocator.LocatorToLatLng("IO91lk45");
            lat.Should().Be(51.43958333333333);
            lon.Should().Be(-1.0458333333333485);
        }

        [Fact]
        public void LocatorToLatLng_2()
        {
            var (lat, lon) = MaidenheadLocator.LocatorToLatLng("IO91lk45xa");
            lat.Should().Be(51.437586805555554);
            lon.Should().Be(-1.0418402777777942);
        }

        [Fact]
        public void LatLngToLocator_0() => MaidenheadLocator.LatLngToLocator(51.4375, -1.0417).Should().Be("IO91lk");

        [Fact]
        public void LatLngToLocator_1() => MaidenheadLocator.LatLngToLocator(51.4375, -1.0417, 1).Should().Be("IO91lk45");

        [Fact]
        public void LatLngToLocator_2() => MaidenheadLocator.LatLngToLocator(51.4375, -1.0417, 2).Should().Be("IO91lk45xa");
    }
}
