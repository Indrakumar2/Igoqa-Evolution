using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models;

namespace Evolution.UnitTests.Mocks.Data.Assignment.Db
{
    public static class MockAssignment
    {
        public static IQueryable<DbModel.Assignment> GetAssignmentMockData()
        {
            return new List<DbModel.Assignment>
            {
               new DbModel.Assignment{Id=1,ProjectId=1,Project=new DbModel.Project{Id=1,ProjectNumber=1},AssignmentNumber=1,IsAssignmentComplete=false,AssignmentStatus="C",AssignmentType="R",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0}

            }.AsQueryable();
        }
    }
}
