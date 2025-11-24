#!/bin/bash
# Script de Teste Rápido - Aprenda+ API
# Bash Script para testar a API

BASE_URL="https://localhost:5001"
API_URL="$BASE_URL/api/v1"

echo "=== Aprenda+ API - Testes Rápidos ==="
echo ""

# Teste 1: Health Check
echo "1. Testando Health Check..."
HEALTH=$(curl -s -k "$BASE_URL/health")
if [ $? -eq 0 ]; then
    echo "   ✅ Health Check: $HEALTH"
else
    echo "   ❌ Erro no Health Check"
fi
echo ""

# Teste 2: Registrar Usuário
echo "2. Registrando novo usuário..."
USER_RESPONSE=$(curl -s -k -X POST "$API_URL/user/register" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Teste User",
    "email": "teste@example.com",
    "senha": "senha123"
  }')

USER_ID=$(echo $USER_RESPONSE | grep -o '"id":"[^"]*' | cut -d'"' -f4)
if [ ! -z "$USER_ID" ]; then
    echo "   ✅ Usuário criado (ID: $USER_ID)"
else
    echo "   ❌ Erro ao criar usuário"
    exit 1
fi
echo ""

# Teste 3: Login
echo "3. Fazendo login..."
LOGIN_RESPONSE=$(curl -s -k -X POST "$API_URL/user/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "teste@example.com",
    "senha": "senha123"
  }')

TOKEN=$(echo $LOGIN_RESPONSE | grep -o '"token":"[^"]*' | cut -d'"' -f4)
if [ ! -z "$TOKEN" ]; then
    echo "   ✅ Login realizado! Token obtido."
    echo "   Token: ${TOKEN:0:50}..."
else
    echo "   ❌ Erro no login"
    exit 1
fi
echo ""

# Teste 4: Listar Usuários
echo "4. Listando usuários (paginado)..."
USERS=$(curl -s -k "$API_URL/user?pageNumber=1&pageSize=5")
if [ $? -eq 0 ]; then
    echo "   ✅ Usuários listados com sucesso"
else
    echo "   ❌ Erro ao listar usuários"
fi
echo ""

# Teste 5: Obter Usuário por ID
echo "5. Obtendo usuário por ID..."
USER=$(curl -s -k "$API_URL/user/$USER_ID")
if [ $? -eq 0 ]; then
    echo "   ✅ Usuário encontrado"
else
    echo "   ❌ Erro ao obter usuário"
fi
echo ""

# Teste 6: Criar Curso
echo "6. Criando curso (autenticado)..."
COURSE_RESPONSE=$(curl -s -k -X POST "$API_URL/courses" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "nome": "Curso de Teste",
    "descricao": "Descrição do curso de teste"
  }')

COURSE_ID=$(echo $COURSE_RESPONSE | grep -o '"id":"[^"]*' | cut -d'"' -f4)
if [ ! -z "$COURSE_ID" ]; then
    echo "   ✅ Curso criado (ID: $COURSE_ID)"
else
    echo "   ❌ Erro ao criar curso"
fi
echo ""

# Teste 7: Listar Cursos
echo "7. Listando cursos..."
COURSES=$(curl -s -k "$API_URL/courses?pageNumber=1&pageSize=5")
if [ $? -eq 0 ]; then
    echo "   ✅ Cursos listados com sucesso"
else
    echo "   ❌ Erro ao listar cursos"
fi
echo ""

# Teste 8: Criar Trilha
echo "8. Criando trilha..."
TRAIL_RESPONSE=$(curl -s -k -X POST "$API_URL/trail" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "nome": "Trilha de Teste",
    "descricao": "Descrição da trilha de teste"
  }')

TRAIL_ID=$(echo $TRAIL_RESPONSE | grep -o '"id":"[^"]*' | cut -d'"' -f4)
if [ ! -z "$TRAIL_ID" ]; then
    echo "   ✅ Trilha criada (ID: $TRAIL_ID)"
else
    echo "   ❌ Erro ao criar trilha"
fi
echo ""

echo "=== Testes Concluídos ==="
echo ""
echo "Para mais detalhes, consulte o GUIA_DE_TESTES.md"

