param(
    [string]$Port = "5000"
)

$root = "C:\Users\Hugo\OneDrive\Documentos\projetos\GestaoEstoque"
$url = "http://localhost:$Port"
$log = Join-Path $root "run.log"

Set-Location $root

# Encerrar instâncias anteriores para evitar lock em bin
Get-Process dotnet -ErrorAction SilentlyContinue | ForEach-Object { $_.Kill() } | Out-Null

# Build
$build = dotnet build
if ($LASTEXITCODE -ne 0) {
    Out-File -FilePath $log -Append -InputObject "$(Get-Date -Format s) Build failed."
    exit 1
}

# Start the app hidden (keeps running after this script)
Start-Process dotnet -ArgumentList "run --project `"$root\EstoqueLocal.Web`" --urls $url --no-build" -WorkingDirectory $root -WindowStyle Hidden

# Give it a moment to boot then open Chrome on the main page
Start-Sleep -Seconds 10
$chromePaths = @(
  'C:\Program Files\Google\Chrome\Application\chrome.exe',
  'C:\Program Files (x86)\Google\Chrome\Application\chrome.exe'
)
$chrome = $chromePaths | Where-Object { Test-Path $_ } | Select-Object -First 1
if ($chrome) {
    Start-Process $chrome "$url/Home/Index"
} else {
    Out-File -FilePath $log -Append -InputObject "$(Get-Date -Format s) Chrome não encontrado."
}
