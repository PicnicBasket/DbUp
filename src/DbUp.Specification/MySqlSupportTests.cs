using System.Data.SqlServerCe;
using System.IO;
using NUnit.Framework;

namespace DbUp.Specification
{
    [TestFixture]
    public class MySqlSupportTests
    {
        [Test]
        public void CanUseMySql()
        {
            const string connectionString = "Server=localhost;Database=daedalus-integration;Uid=DaedalusDbUser;Pwd=password;";

            //Verify supports scripts which specify schema (To Support MySql)
            var upgrader = DeployChanges.To
                .MySqlDatabase(connectionString)
                .WithScript("Script0001", "create table $schema$.Foo (Id int)")
                .Build();

            var result = upgrader.PerformUpgrade();

            Assert.IsTrue(result.Successful);
        } 
    }
}