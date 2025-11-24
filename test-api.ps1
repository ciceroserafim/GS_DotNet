# Script de Teste Rápido - Aprenda+ API
# PowerShell Script para testar a API

$baseUrl = "https://localhost:5001"
$apiUrl = "$baseUrl/api/v1"

Write-Host "=== Aprenda+ API - Testes Rápidos ===" -ForegroundColor Cyan
Write-Host ""

# Ignorar erros de certificado SSL
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }

# Teste 1: Health Check
Write-Host "1. Testando Health Check..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/health" -Method Get -SkipCertificateCheck
    Write-Host "   ✅ Health Check: $response" -ForegroundColor Green
}
catch {
    Write-Host "   ❌ Erro: $_" -ForegroundColor Red
}
Write-Host ""

# Teste 2: Registrar Usuário
Write-Host "2. Registrando novo usuário..." -ForegroundColor Yellow
$userData = @{
    nome  = "Teste User"
    email = "teste@example.com"
    senha = "senha123"
} | ConvertTo-Json

try {
    $user = Invoke-RestMethod -Uri "$apiUrl/user/register" -Method Post -Body $userData -ContentType "application/json" -SkipCertificateCheck
    Write-Host "   ✅ Usuário criado: $($user.nome) (ID: $($user.id))" -ForegroundColor Green
    $userId = $user.id
}
catch {
    Write-Host "   ❌ Erro ao criar usuário: $_" -ForegroundColor Red
    exit
}
Write-Host ""

# Teste 3: Login
Write-Host "3. Fazendo login..." -ForegroundColor Yellow
$loginData = @{
    email = "teste@example.com"
    senha = "senha123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/user/login" -Method Post -Body $loginData -ContentType "application/json" -SkipCertificateCheck
    $token = $loginResponse.token
    Write-Host "   ✅ Login realizado! Token obtido." -ForegroundColor Green
    Write-Host "   Token: $($token.Substring(0, [Math]::Min(50, $token.Length)))..." -ForegroundColor Gray
}
catch {
    Write-Host "   ❌ Erro no login: $_" -ForegroundColor Red
    exit
}
Write-Host ""

# Teste 4: Listar Usuários (Paginado)
Write-Host "4. Listando usuários (paginado)..." -ForegroundColor Yellow
try {
    $users = Invoke-RestMethod -Uri "$apiUrl/user?pageNumber=1&pageSize=5" -Method Get -SkipCertificateCheck
    Write-Host "   ✅ Usuários listados: $($users.totalItems) total, página $($users.pageNumber) de $($users.totalPages)" -ForegroundColor Green
    Write-Host "   Links HATEOAS presentes: $($users.links.Count)" -ForegroundColor Gray
}
catch {
    Write-Host "   ❌ Erro ao listar usuários: $_" -ForegroundColor Red
}
Write-Host ""

# Teste 5: Obter Usuário por ID
Write-Host "5. Obtendo usuário por ID..." -ForegroundColor Yellow
try {
    $user = Invoke-RestMethod -Uri "$apiUrl/user/$userId" -Method Get -SkipCertificateCheck
    Write-Host "   ✅ Usuário encontrado: $($user.nome)" -ForegroundColor Green
    Write-Host "   Links HATEOAS: $($user.links.Count)" -ForegroundColor Gray
}
catch {
    Write-Host "   ❌ Erro ao obter usuário: $_" -ForegroundColor Red
}
Write-Host ""

# Teste 6: Criar Curso (Autenticado)
Write-Host "6. Criando curso (autenticado)..." -ForegroundColor Yellow
$courseData = @{
    nome      = "Curso de Teste"
    descricao = "Descrição do curso de teste"
} | ConvertTo-Json

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type"  = "application/json"
}

try {
    $course = Invoke-RestMethod -Uri "$apiUrl/courses" -Method Post -Body $courseData -Headers $headers -SkipCertificateCheck
    Write-Host "   ✅ Curso criado: $($course.nome) (ID: $($course.id))" -ForegroundColor Green
    $courseId = $course.id
}
catch {
    Write-Host "   ❌ Erro ao criar curso: $_" -ForegroundColor Red
}
Write-Host ""

# Teste 7: Listar Cursos
Write-Host "7. Listando cursos..." -ForegroundColor Yellow
try {
    $courses = Invoke-RestMethod -Uri "$apiUrl/courses?pageNumber=1&pageSize=5" -Method Get -SkipCertificateCheck
    Write-Host "   ✅ Cursos listados: $($courses.totalItems) total" -ForegroundColor Green
}
catch {
    Write-Host "   ❌ Erro ao listar cursos: $_" -ForegroundColor Red
}
Write-Host ""

# Teste 8: Criar Trilha
Write-Host "8. Criando trilha..." -ForegroundColor Yellow
$trailData = @{
    nome      = "Trilha de Teste"
    descricao = "Descrição da trilha de teste"
} | ConvertTo-Json

try {
    $trail = Invoke-RestMethod -Uri "$apiUrl/trail" -Method Post -Body $trailData -Headers $headers -SkipCertificateCheck
    Write-Host "   ✅ Trilha criada: $($trail.nome) (ID: $($trail.id))" -ForegroundColor Green
}
catch {
    Write-Host "   ❌ Erro ao criar trilha: $_" -ForegroundColor Red
}
Write-Host ""

Write-Host "=== Testes Concluídos ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Para mais detalhes, consulte o GUIA_DE_TESTES.md" -ForegroundColor Gray

