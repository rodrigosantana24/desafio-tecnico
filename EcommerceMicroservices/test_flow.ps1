# Test flow script for EcommerceMicroservices
# Run this from the solution root: .\test_flow.ps1

$gateway = 'http://localhost:5000'
$estoque = 'http://localhost:5001'
$vendas = 'http://localhost:5002'

Write-Host "Starting test flow..."

# Create a product via gateway (estoque)
$prod = @{ Nome = 'Caneca'; Preco = 25.5; Estoque = 10 } | ConvertTo-Json
Write-Host "Creating product via gateway..."
try {
    $resp = Invoke-RestMethod -Method Post -Uri "$gateway/estoque/api/products" -Body $prod -ContentType 'application/json' -ErrorAction Stop
    $resp | ConvertTo-Json -Depth 5 | Out-File product_created.json -Encoding UTF8
    Write-Host "Product created -> saved to product_created.json"
} catch {
    Write-Error "Failed to create product: $_"
}

Start-Sleep -Seconds 1

Write-Host "Listing products (before order)..."
try {
    $list = Invoke-RestMethod -Method Get -Uri "$gateway/estoque/api/products" -ErrorAction Stop
    $list | ConvertTo-Json -Depth 5 | Out-File products_before.json -Encoding UTF8
    Write-Host "Products list saved to products_before.json"
} catch { Write-Error "Failed to list products: $_" }

Start-Sleep -Seconds 1

# Create an order via gateway (vendas)
$order = @{ ProductId = 1; Quantity = 2 } | ConvertTo-Json
Write-Host "Creating order via gateway..."
try {
    $ord = Invoke-RestMethod -Method Post -Uri "$gateway/vendas/api/orders" -Body $order -ContentType 'application/json' -ErrorAction Stop
    $ord | ConvertTo-Json -Depth 5 | Out-File order_created.json -Encoding UTF8
    Write-Host "Order created -> saved to order_created.json"
} catch { Write-Error "Failed to create order: $_" }

Start-Sleep -Seconds 2

Write-Host "Listing orders..."
try {
    $orders = Invoke-RestMethod -Method Get -Uri "$gateway/vendas/api/orders" -ErrorAction Stop
    $orders | ConvertTo-Json -Depth 5 | Out-File orders_list.json -Encoding UTF8
    Write-Host "Orders list saved to orders_list.json"
} catch { Write-Error "Failed to list orders: $_" }

Start-Sleep -Seconds 1

Write-Host "Listing products (after order)..."
try {
    $list2 = Invoke-RestMethod -Method Get -Uri "$gateway/estoque/api/products" -ErrorAction Stop
    $list2 | ConvertTo-Json -Depth 5 | Out-File products_after.json -Encoding UTF8
    Write-Host "Products list saved to products_after.json"
} catch { Write-Error "Failed to list products after order: $_" }

Write-Host "Test flow finished. Review the generated JSON files in the current folder and take screenshots as needed."
