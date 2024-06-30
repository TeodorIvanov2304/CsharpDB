/*
First install:
Install-Package Microsoft.EntityFrameworkCore -v 6.0.1 ? 
Install-Package Microsoft.EntityFrameworkCore.Tools –v 6.0.1
Install-Package Microsoft.EntityFrameworkCore.SqlServer –v 6.0.1
Install-Package Microsoft.EntityFrameworkCore.Design -v 6.0.1

The program will consist from different layers(projects):

Create new Project P02_FootballBetting.Data, type Class library, which will store the DbContext
Create new Project P02_FootballBetting.Models, type Class library, for storing models
Create new Project P02_FootballBetting.Common, type Class library.The project will be accessible from all other P02 Projects.It will store constants and shared things

Create DBContext class in P02_FootballBetting.Data folder
Install the packages for this project too
Create connection string
Create override OnConfiguring method
Create constructor and pass DbContextOptions options
Create Country class in P02_FootballBetting.Models
Create ValidationConstants class in FootballBetting.Models. This is for storing the app constants
Add reference from FootballBetting.Models to FootballBetting.Common
Add Country DbSet in FootballBettingContext
Add reference from FootballBettingData to .Models
Create Town class in P02_FootballBetting.Models
Create the relation between Town and Country, part 1, part 2
Create constructor in Country, initializing hashset of towns
Add Town in DbContext
Create User class in .Models
Add User in DbContext
Create Position class in .Models
Add Position class in DbContext
Create Team class in .Models 
Create 2 FK PrimaryColorId and SecondaryColorId
Create the second part of the relation ->
Create public ICollection<Team> PrimaryKitTeams { get; set; } and SecondaryKitTeams in Color class 
Initialize new hashsets of Team in Color constructor -> PrimaryKitTeams = new HashSet<Team>();
Add Team class in DbContext
*/