# cosmos-db-demo

A bunch of .NET code samples demonstrating best parts of Cosmos DB.

The code uses identity-based authentication when connecting to Cosmos DB, so you will need to explicitly give yourself permissions via e.g. Azure Powershell:
```
	$accountName = "my-cosmos-db-account-name"
	$resourceGroupName = "my-resource-group-name"
	$principalId = "my-entra-id-user-object-id"

	New-AzCosmosDBSqlRoleAssignment `
		-AccountName $accountName `
		-ResourceGroupName $resourceGroupName `
		-RoleDefinitionId "00000000-0000-0000-0000-000000000002" `
		-Scope "/" `
		-PrincipalId $principalId
```