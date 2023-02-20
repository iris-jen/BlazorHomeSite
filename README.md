# BlazorHomeSite

Repo for my personal website,
--Down for some changes!!--

To run:
(Apply all commands in the BlazorHomeSite directory)

-> Migrations need to be applied 
	-use the command "dotnet ef database update" 

-> User Secrets need to be added for the Admin account / Email stuff to work. You will need a send grid API key 
    - use the commands
		- dotnet user-secrets set SendGridKey <key>
		- dotnet user-secrets set FromEmailAddress <value>
		- dotnet user-secrets set AdminEmailAddress <value>
		- dotnet user-secrets set ShowInitAdminButton <value>