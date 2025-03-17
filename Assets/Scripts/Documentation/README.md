# Documentação do Platformer 2D

## Índice
1. [Arquitetura do Projeto](#1-arquitetura-do-projeto)
2. [Sistemas Core](#2-sistemas-core)
3. [Sistema do Player](#3-sistema-do-player)
4. [Sistema de Items](#4-sistema-de-items)
5. [Sistema de UI](#5-sistema-de-ui)
6. [Inimigos](#6-inimigos)
7. [Guia de Implementação](#7-guia-de-implementação)

## 1. Arquitetura do Projeto

### Estrutura de Pastas
```
Assets/Scripts/
├── Core/               # Sistemas fundamentais
├── Player/             # Componentes do jogador
├── Items/             # Sistema de coletáveis
├── Enemies/           # Sistema de inimigos
├── UI/                # Interface do usuário
└── ScriptableObjects/ # Dados configuráveis
```

## 2. Sistemas Core

### GameManager
Responsável pelo controle do estado do jogo e reinicialização.

**Implementação:**
1. Crie um objeto vazio "GameManager" na primeira cena
2. Adicione o componente `GameManager.cs`
3. Este objeto será mantido entre cenas (DontDestroyOnLoad)

**Eventos:**
- `OnPlayerDeath` -> Reinicia o jogo após 1.5 segundos

### GameEvents
Sistema de eventos global para comunicação entre componentes.

**Implementação:**
1. Criado automaticamente pelo GameManager
2. Gerencia eventos do tipo:
   - Simples (void)
   - Int (pontuação, vida)
   - Float (temporizadores)

**Eventos Principais:**
```csharp
OnScoreChanged(int)
OnHealthChanged(int)
OnPlayerDeath()
OnInvincibilityStarted(float)
```

### ObjectPool
Sistema de pooling para otimização de objetos frequentemente criados/destruídos.

**Uso:**
```csharp
// Obter objeto
ObjectPool.Instance.SpawnFromPool(prefab, position, rotation);

// Retornar objeto
ObjectPool.Instance.ReturnToPool(gameObject);
```

## 3. Sistema do Player

### PlayerController
Componente principal que integra todos os sistemas do jogador.

**Configuração:**
1. Crie um objeto para o player
2. Adicione componentes:
   - Rigidbody2D (Constraints: Freeze Rotation Z)
   - BoxCollider2D
   - PlayerController
3. Configure ScriptableObjects:
   - PlayerMovementData
   - PlayerHealthData

### PlayerMovementData (ScriptableObject)
```csharp
walkSpeed: 5f        // Velocidade base
runSpeed: 8f         // Velocidade correndo
acceleration: 20f    // Aceleração
deceleration: 30f    // Desaceleração
airControlFactor: 0.5f
groundCheckDistance: 0.88f
coyoteTime: 0.2f
jumpBufferTime: 0.2f
jumpForce: 10f
maxJumps: 2
```

### PlayerHealthData (ScriptableObject)
```csharp
maxHealth: 3
invincibilityDuration: 2f
knockbackForce: 10f
knockbackDuration: 0.2f
```

## 4. Sistema de Items

### Item Base
Classe abstrata para todos os coletáveis.

**Para criar novo item:**
1. Crie script herdando de `Item`
2. Implemente `OnCollect()`
3. Configure prefab:
   - Adicione BoxCollider2D (IsTrigger = true)
   - Adicione script do item
   - Configure ItemData

### Tipos de Items
- **HealthItem**: Restaura vida
- **PointItem**: Adiciona pontos
- **InvincibilityItem**: Temporariamente invencível
- **PowerUpItem**: Boost de velocidade

## 5. Sistema de UI

### UIManager
Gerencia todos os elementos da interface.

**Configuração:**
1. Crie Canvas (UI -> Canvas)
2. Adicione elementos:
   - Score Text (TextMeshProUGUI)
   - Health Text (TextMeshProUGUI)
3. Adicione UIManager ao Canvas
4. Configure referências no inspector

## 6. Inimigos

### Enemy Base
Classe base para todos os inimigos.

**Configuração de novo inimigo:**
1. Crie script herdando de `Enemy`
2. Configure colliders:
   - mainCollider (lateral)
   - topCollider (stomping)
3. Configure EnemyData ScriptableObject

### PatrolEnemy
Exemplo de inimigo que patrulha área.

**Configuração:**
```csharp
moveSpeed: 2f
patrolDistance: 3f
groundLayer: (LayerMask)
```

## 7. Guia de Implementação

### Configuração Inicial
1. Crie cena nova
2. Adicione GameManager
3. Configure Layers:
   - Ground
   - Player
   - Enemy

### Player Setup
1. Configure player prefab
2. Crie ScriptableObjects:
   ```
   Create -> Game/Player/Movement Data
   Create -> Game/Player/Health Data
   ```
3. Configure valores nos ScriptableObjects
4. Atribua referências no inspector

### Sistema de Items
1. Configure prefabs para cada tipo
2. Crie ItemData para cada tipo:
   ```
   Create -> Game/Item Data/Health Item
   Create -> Game/Item Data/Point Item
   Create -> Game/Item Data/Invincibility Item
   ```

### UI Setup
1. Configure Canvas
2. Adicione TextMeshPro (se necessário)
3. Configure UIManager
4. Teste atualizações de UI

### Checklist Final
- [ ] GameManager na cena
- [ ] Player configurado
- [ ] Layers configuradas
- [ ] ScriptableObjects criados
- [ ] UI funcionando
- [ ] Sistema de items testado
- [ ] Inimigos posicionados
- [ ] Cena adicionada ao Build Settings

### Debug Tools
- Gizmos para visualização de:
  - Área de patrulha (inimigos)
  - Ground check (player)
  - Colliders (debug mode)

### Dicas de Otimização
1. Use Object Pooling para:
   - Efeitos de partículas
   - Projéteis
   - Items coletáveis
2. Configure corretamente as collision layers
3. Use eventos ao invés de GetComponent frequente
4. Mantenha ScriptableObjects organizados
