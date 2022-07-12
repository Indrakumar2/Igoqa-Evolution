using Evolution.DbRepository.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.UnitTest.Mocks.Contexts
{
    public static class MockedEvolutionContext
    {
        public static Mock<Evolution2Context> GetMockContext()
        {
            return new Mock<Evolution2Context>();
        }
    }
}
