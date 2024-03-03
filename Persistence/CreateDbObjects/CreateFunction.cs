using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.CreateDbObjects
{
    public static class CreateFunction
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

                return 0;
            }
        }
        public static void Create(AppDbContext dbContext)
        {

            int exitsFunction = -1;
            string rawSqlQuery = "";
            try
            {

                //Return Table 
                #region fn_Example
                try
                {

                    string fn_Example = "fn_Example";
                    exitsFunction = checkDbObject(dbContext, fn_Example);
                    if (exitsFunction == 0)
                    {
                        rawSqlQuery = @"
        CREATE FUNCTION [dbo].[" + fn_Example + @"]
		(

)
RETURNS @Result TABLE (ID INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(500))
AS
BEGIN
 INSERT INTO @RESULT(Name)
SELECT 'TEST'

    RETURN;
END
		";


                        dbContext.Database.ExecuteSqlRaw(rawSqlQuery);

                    }

                }
                catch (Exception)
                {


                }
                #endregion

                //Return Scaler
                #region fn_Base64StringToImage
                try
                {

                    string fn_Base64StringToImage = "fn_Base64StringToImage";
                    exitsFunction = checkDbObject(dbContext, fn_Base64StringToImage);
                    if (exitsFunction == 0)
                    {
                        rawSqlQuery = @"
        CREATE FUNCTION [dbo].[" + fn_Base64StringToImage + @"]
		(
    @base64String NVARCHAR(MAX)
)
RETURNS VARBINARY(MAX)
AS
BEGIN
    RETURN CAST('' AS XML).value('xs:base64Binary(sql:variable(""@base64String""))', 'VARBINARY(MAX)');
END
		";


                        dbContext.Database.ExecuteSqlRaw(rawSqlQuery);

                    }

                }
                catch (Exception)
                {


                }
                #endregion
                #region fn_ImageToBase64String
                try
                {

                    string fn_ImageToBase64String = "fn_ImageToBase64String";
                    exitsFunction = checkDbObject(dbContext, fn_ImageToBase64String);
                    if (exitsFunction == 0)
                    {
                        rawSqlQuery = @"
        CREATE FUNCTION [dbo].[" + fn_ImageToBase64String + @"]
		(
    @image VARBINARY(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (SELECT CAST('' AS XML).value('xs:base64Binary(sql:variable(""@image""))', 'NVARCHAR(MAX)'));
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
