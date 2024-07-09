/*

-c stands for context
//Add-Migration -c SchoolContext InitialMigration 
Update-Database
Script-Migration  - generates the SQL script
Add-Migration AddressAdded - Add Address property
//We can remove the applyed migration, but only if the DB is not updated
Add-Migration EmailAdded
Remove-Migration
Add-Migration EmailAddedRequired
Update-Database
Get-Migration --> returns list of previous migrations

***
For deleting all migrations:
1. In MS SQL TRUNCATE TABLE [__EFMigrationHistory]
2. Delete Migrations folder in VS Studio Project
3. Add-Migration InitialMigration --> includes all the changes, that are made by now
This is called Squash
4.Comment the content of protected override void Up(MigrationBuilder migrationBuilder) method --> if we don't do this, EF will try to create another table
5.Update-Database
***
Rename FullName to Name

Go to protected override void OnConfiguring and add new OptionBuilder for PostgreSql
Set:  $env:DATABASE_PROVIDER="PostgreSql" --> Environment variable
Check if the variable is created: echo $env:DATABASE_PROVIDER
Add-Migration InitialPostgreMigration -o Migrations/PostgreSqlMigrations

Create new DbContext SchoolPostgreContext
Add-Migration InitialPostgreMigration -c SchoolPostGreContext
*/