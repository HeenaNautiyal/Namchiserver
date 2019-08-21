USE [MVCDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_Login]    Script Date: 19-07-2019 10:26:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_Login]
@UserName varchar(100),
@Password varchar(100),
@Msg varchar(Max) out

As
DECLARE 
@Outputparam VARCHAR(200);

set @Outputparam='';

Begin tran
IF NOT EXISTS(SELECT *FROM Tbl_UserMaster WHERE UserName =@UserName and Password=@Password)
begin  
SET @Msg='Invalid Login/Password'
END 
Else
Begin
Set @Msg='Login Successfully'
End
IF @@ERROR <> 0

     BEGIN

            ROLLBACK TRAN

      END

ELSE

      BEGIN

            COMMIT TRAN

      END

Select @Msg

--Exec SP_Login    UserName,					   Password,       Message
Exec SP_Login	   'heena.nautiyal@caritaseco.in', 'admin' ,        ''



