# Documentação do Projeto Platformer 2D

## 1. Estrutura do Projeto

### 1.1 Organização de Pastas
- `Core/`: Sistemas fundamentais (GameEvents, ObjectPool)
- `Player/`: Componentes do jogador
- `Items/`: Sistema de itens coletáveis
- `Enemies/`: Sistema de inimigos
- `UI/`: Interface do usuário
- `ScriptableObjects/`: Dados configuráveis

## 2. Sistema do Jogador

### 2.1 Sistema de Input
O sistema de input é gerenciado pela classe `PlayerInput`:

```csharp
// Configuração de Input
MoveInput    -> Horizontal Axis (A/D ou ←/→)
IsRunning    -> Left Shift
JumpPressed  -> Space (GetButtonDown)
JumpReleased -> Space (GetButtonUp)
JumpHeld     -> Space (GetButton)
```

### 2.2 Sistema de Movimento
Características atualizadas:
- Multi-raycast para detecção de chão
- Sistema de buffer de pulo aprimorado
- Coyote time implementado
- Controle de velocidade variável

#### Debug Visual:
- Raios vermelhos mostram a detecção de chão
- Três pontos de verificação (esquerda, centro, direita)

#### Configuração do MovementData:
```csharp
walkSpeed = 5f           // Velocidade base
runSpeed = 8f           // Velocidade de corrida
acceleration = 10f      // Aceleração
deceleration = 10f      // Desaceleração
airControlFactor = 0.5f // Controle no ar
groundCheckDistance = 0.2f
coyoteTime = 0.2f
jumpBufferTime = 0.2f
jumpForce = 10f
maxJumps = 2
```

### 2.3 Sistema de Vida
Funcionalidades atualizadas:
- Sistema de dano com knockback
- Invencibilidade configurável
- Sistema de cura
- Integração com UI

#### Eventos de Vida:
```csharp
OnHealthChanged      // Quando a vida é alterada
OnPlayerDeath       // Quando o jogador morre
OnInvincibilityStarted  // Ao iniciar invencibilidade
```

## 3. Sistema de UI

### 3.1 Configuração do UIManager
1. Adicione o componente UIManager a um objeto da UI
2. Configure as referências:
   - Score Text (TextMeshProUGUI)
   - Health Text (TextMeshProUGUI)

### 3.2 Eventos da UI
O UIManager escuta os seguintes eventos:
```csharp
OnScoreChanged  -> Atualiza pontuação
OnHealthChanged -> Atualiza barra de vida
```

### 3.3 Integração com Itens
Cada item atualiza a UI através de:
1. Chamada direta via UIManager
2. Sistema de eventos (GameEvents)

## 4. Sistema de Itens

### 4.1 Tipos de Itens e Efeitos
1. HealthItem
   - Restaura vida
   - Atualiza UI de vida
   - Dispara evento OnHealthChanged

2. PointItem
   - Adiciona pontos
   - Atualiza UI de score
   - Dispara evento OnScoreChanged

3. PowerUpItem
   - Aumenta velocidade temporariamente
   - Afeta walkSpeed e runSpeed
   - Restaura valores originais após duração

### 4.2 Implementação de Novos Itens
```csharp
public class NewItem : Item
{
    public override void OnCollect(PlayerController collector)
    {
        // Efeito do item
        // Atualização da UI
        // Disparo de eventos
    }
}
```

## 5. Passo a Passo de Implementação

### 5.1 Configuração do Player
1. Crie um GameObject para o player
2. Adicione componentes:
   - Rigidbody2D
   - BoxCollider2D
   - PlayerController
3. Configure LayerMask para ground detection
4. Ajuste valores no PlayerMovementData
5. Configure referência do UIManager

### 5.2 Configuração da UI
1. Crie canvas com UI elements
2. Adicione TextMeshProUGUI para score e vida
3. Configure UIManager
4. Teste eventos de atualização

### 5.3 Configuração de Itens
1. Crie prefabs para cada tipo de item
2. Configure colliders como triggers
3. Adicione scripts específicos
4. Teste coleta e atualizações na UI

## 6. Debug e Otimização

### 6.1 Debug Visual
- Raycast ground detection (vermelho)
- Gizmos para áreas de patrulha
- Debug.Log para eventos importantes

### 6.2 Checklist de Implementação
- [ ] Player movement configurado
- [ ] Ground detection funcionando
- [ ] Sistema de pulo responsivo
- [ ] UI atualiza corretamente
- [ ] Itens funcionando
- [ ] Eventos disparando apropriadamente

## 7. Sistema de Eventos (GameEvents)

### 7.1 Principais Eventos
- OnScoreChanged
- OnHealthChanged
- OnPlayerDeath
- OnInvincibilityStarted

### 7.2 Uso:
```csharp
// Registrar listener
GameEvents.Instance.AddListener("EventName", () => { });

// Disparar evento
GameEvents.Instance.TriggerEvent("EventName");
```

## 8. Object Pooling

### 8.1 Implementação:
```csharp
// Obter objeto
ObjectPool.Instance.SpawnFromPool(prefab, position, rotation);

// Retornar objeto
ObjectPool.Instance.ReturnToPool(gameObject);
```

## 9. Dicas de Otimização

1. Use Object Pooling para objetos frequentes
2. Mantenha os ScriptableObjects organizados
3. Use eventos ao invés de referências diretas
4. Implemente CullingGroup para inimigos
5. Configure corretamente as layers de colisão

## 10. Suporte e Manutenção

### 10.1 Adicionando Novos Recursos
1. Planeje a implementação
2. Crie ScriptableObjects se necessário
3. Implemente seguindo os padrões existentes
4. Teste extensivamente
5. Documente as mudanças

### 10.2 Debug
- Use Debug.Log para desenvolvimento
- Implemente Gizmos para visualização
- Configure corretamente tags e layers
