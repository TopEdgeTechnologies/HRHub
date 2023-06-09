# HRHub




Steps need to follow
=================
1, Create .NET Core application.


2, Install the below nugget packages
     1,  Microsoft.EntityFrameworkCore
     2,  Microsoft.EntityFrameworkCore.Sqlserver 
     3,  Microsoft.EntityFrameworkCore.Tools
     4, Microsoft.AspNetCore.Authentication.JwtBearer 6.0.16
     

3,  Set Json 

	"DevConnect": "Data Source=WebServer;Initial Catalog=HrHub;User ID=team;Password=dynamixsolpassword; TrustServerCertificate=True;"

4, Generate DbContext using scaffold command

For Webserver ------>
Scaffold-DbContext "Data Source=WebServer;Initial Catalog=HRHub;User ID=team;Password=dynamixsolpassword;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f -DataAnnotations     
	
For Local ---------> 
Scaffold-DbContext "Data Source=DESKTOP-A2BNQOF;Initial Catalog=HRHub; Trusted_Connection=true; TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f -DataAnnotations

5, Set Program Class

	builder.Services.AddDbContext<MtbSchoolSystemContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnect")));



Data Source=DESKTOP-A2BNQOF;Initial Catalog=MTB_SchoolSystem;Integrated Security=True


app.UseCors(Options => Options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());

{

  "userName": "Super Admin",
  "password": "Super Admin"
 
}

