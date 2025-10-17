dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0-preview.3.*

dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0-preview.3.*

dotnet tool install --global dotnet-ef --version 9.0.0-preview.3.*

dotnet ef migrations add InitialCreate

dotnet ef database update
