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
        
        

        [Fact]
        public void LocatorShiftUp_0() => MaidenheadLocator.LocatorShiftUp("IO91").Should().Be("IO92");
        
        [Fact]
        public void LocatorShiftUp_1() => MaidenheadLocator.LocatorShiftUp("IO91LK").Should().Be("IO91LL");
        
        [Fact]
        public void LocatorShiftUp_2() => MaidenheadLocator.LocatorShiftUp("IO91LK54").Should().Be("IO91LK55");
        
        [Fact]
        public void LocatorShiftUp_3() => MaidenheadLocator.LocatorShiftUp("IO91LK54PK").Should().Be("IO91LK54PL");
        
        [Fact]
        public void LocatorShiftUp_4() => MaidenheadLocator.LocatorShiftUp("IO91LK54AX").Should().Be("IO91LK55AA");
        
        /*[Fact]
        public void LocatorShiftUp_5() => MaidenheadLocator.LocatorShiftUp("IR04AX09AX").Should().Be("??????????");*/ //This test truly sucks
        
        
        
        [Fact]
        public void LocatorShiftDown_0() => MaidenheadLocator.LocatorShiftDown("IO92").Should().Be("IO91");
        
        [Fact]
        public void LocatorShiftDown_1() => MaidenheadLocator.LocatorShiftDown("IO91LL").Should().Be("IO91LK");
        
        [Fact]
        public void LocatorShiftDown_2() => MaidenheadLocator.LocatorShiftDown("IO91LK55").Should().Be("IO91LK54");
        
        [Fact]
        public void LocatorShiftDown_3() => MaidenheadLocator.LocatorShiftDown("IO91LK54PL").Should().Be("IO91LK54PK");
        
        [Fact]
        public void LocatorShiftDown_4() => MaidenheadLocator.LocatorShiftDown("IO91LK55AA").Should().Be("IO91LK54AX");
        
        
        
        [Fact]
        public void LocatorShiftRight_0() => MaidenheadLocator.LocatorShiftRight("IO92").Should().Be("JO02");
        
        [Fact]
        public void LocatorShiftRight_1() => MaidenheadLocator.LocatorShiftRight("IO91LL").Should().Be("IO91ML");
        
        [Fact]
        public void LocatorShiftRight_2() => MaidenheadLocator.LocatorShiftRight("IO91LK55").Should().Be("IO91LK65");
        
        [Fact]
        public void LocatorShiftRight_3() => MaidenheadLocator.LocatorShiftRight("IO91LK54PL").Should().Be("IO91LK54QL");
        
        [Fact]
        public void LocatorShiftRight_4() => MaidenheadLocator.LocatorShiftRight("IO91LK54XL").Should().Be("IO91LK64AL");
        
        
        
        [Fact]
        public void LocatorShiftLeft_0() => MaidenheadLocator.LocatorShiftLeft("JO02").Should().Be("IO92");
        
        [Fact]
        public void LocatorShiftLeft_1() => MaidenheadLocator.LocatorShiftLeft("IO91ML").Should().Be("IO91LL");
        
        [Fact]
        public void LocatorShiftLeft_2() => MaidenheadLocator.LocatorShiftLeft("IO91LK65").Should().Be("IO91LK55");
        
        [Fact]
        public void LocatorShiftLeft_3() => MaidenheadLocator.LocatorShiftLeft("IO91LK54QL").Should().Be("IO91LK54PL");
        
        [Fact]
        public void LocatorShiftLeft_4() => MaidenheadLocator.LocatorShiftLeft("IO91LK64AL").Should().Be("IO91LK54XL");
    }
}
