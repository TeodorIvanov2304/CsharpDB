/*

First install:
Install-Package Microsoft.EntityFrameworkCore -v 6.0.1 ? 
Install-Package Microsoft.EntityFrameworkCore.Tools –v 6.0.1
Install-Package Microsoft.EntityFrameworkCore.SqlServer –v 6.0.1
Install-Package Microsoft.EntityFrameworkCore.Design -v 6.0.1
Create DbContext class
Then create connection string
In StudentSystemContext type override OnConfiguring method with:
- optionsBuilder.UseSqlServer(ConnectionString);  -- Sql could be replaced with Postgre or other
Then build student class and set attributes
Add DbContext<Student> Students {get; set;}
Build Course class
Build Mapping class StudentCourse and we make the navigation properties "virtual"
Create public virtual ICollection<StudentCourse> StudentsCourses { get; set; } in Student class
Copy-paste the same property in Courses
Create override void OnModelCreating(ModelBuilder modelBuilder) in StudentSystemContext then implement the composite PK configuration
Add Course and StudentCourse DBSets
Create Resource class
Create enumeration in Resource, ResourceType
Add collection of resources public virtual ICollection<Resource> Resources { get; set; } in Course class (second part from the relation). The first part is creating the FK in Course
Create Homework class, create the relation. After that, the second part of the relation - public virtual ICollection<Homework> Homeworks { get; set; } in Course class and Student class
Add Resource and Homework DbSets in StudentSystemContext
Create constructor, that accepts DbContextOptions in StudentSystemContext


After creating and configuring all models write in PMC:
Add-Migration InitialMigration
Update-Database










 
 */