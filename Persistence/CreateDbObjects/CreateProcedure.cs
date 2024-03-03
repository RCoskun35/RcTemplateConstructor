using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.CreateDbObjects
{
    public static class CreateProcedure
    {
        public static int checkDbObject(AppDbContext dbContext, string dbObjectName)
        {
            int result = 0;
            try
            {
                result = dbContext.DbObjectId.FromSqlRaw($"SELECT ISNULL(OBJECT_ID('dbo.{dbObjectName}'),0) Id").FirstOrDefault().Id;
                return result;
            }
            catch (Exception)
            {

                return result;
            }



        }
        public static void Create(AppDbContext dbContext)
        {
            try
            {
                int exitsProcedure = 0;
                string rawSqlQuery = "";

                #region test
                try
                {
                    string test = "test";
                    exitsProcedure = checkDbObject(dbContext, test);
                    if (exitsProcedure == 0)
                    {
                        rawSqlQuery = @"
        CREATE PROCEDURE [dbo].[" + test + @"]
			(
		@LoginId int=null,
		@ParamInt int = null,
		@FirmIds nvarchar(100)=null,
		@Name nvarchar(100)=null,
		@SurName nvarchar(100)=null,
		@PersonnelNo nvarchar(100)=null,
		@StartDate datetime=null,
		@EndDate datetime=null,
		@ParamBool bit=null,
		@ParamBool2 bit=null
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	select top 100 * from aspnetusers 
	WHERE 1 = IIF(@ParamInt IS NULL, 1, CASE WHEN ID > @ParamInt THEN 1 ELSE 0 END)
END
		
		";


                        dbContext.Database.ExecuteSqlRaw(rawSqlQuery);

                    }
                }
                catch (Exception)
                {


                }
                #endregion

            }
            catch (Exception)
            {


            }

        }
    }
}
