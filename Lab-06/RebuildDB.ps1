$serverName = "TRONGKHANG-IT"
$databaseName = "BookstoreDB"

# Read SQL from file with explicit encoding to avoid double-encoding issues
$sqlFile = "e:\Lab-06\Lab-06\RebuildDatabasev2.sql"
$fullSql = Get-Content -Path $sqlFile -Encoding UTF8 -Raw

# Execute via .NET
$connStr = "Server=$serverName;Database=master;Integrated Security=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
try {
    $conn.Open()
    
    # Split by GO is tricky in simple execution, but RebuildDatabasev2.sql has GO.
    # We need to handle GO or run the script as one batch if possible, or use SMO.
    # Simpler: Modify SQL to not use GO or split it here.
    # The RebuildDatabasev2.sql uses GO. Let's split it.
    
    $batches = $fullSql -split "(?m)^\s*GO\s*$"
    
    foreach ($batch in $batches) {
        if ($batch.Trim().Length -gt 0) {
            $cmd = $conn.CreateCommand()
            $cmd.CommandText = $batch
            $cmd.ExecuteNonQuery()
        }
    }
    Write-Host "Database Rebuilt Successfully."

    # Verify
    $cmd.CommandText = "SELECT TOP 1 CategoryName FROM BookstoreDB.dbo.Categories"
    $res = $cmd.ExecuteScalar()
    
    # Outputting to file prevents console font issues
    $res | Out-File "e:\Lab-06\Lab-06\verify_result.txt" -Encoding UTF8
    Write-Host "Verification result saved to verify_result.txt"
}
catch {
    Write-Error $_.Exception.Message
}
finally {
    $conn.Close()
}
