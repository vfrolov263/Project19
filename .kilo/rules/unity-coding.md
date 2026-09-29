# Unity Coding Standards

Code style and conventions for this Unity project.

## Namespaces
- Use `Assets._Project.Scripts.<Module>` namespace structure.
- Match the namespace to the folder path exactly.
- No `using` global namespaces at the top level.

## Naming Conventions
- Public fields: `PascalCase` (e.g., `InteractionDistance`).
- Private fields: `_camelCase` with leading underscore.
- Local variables: `camelCase`.
- Interfaces: `I` prefix (e.g., `IInteractable`, `IInteractor`).
- Static classes: `PascalCase` (e.g., `Settings`).

## Serialization
- Use `[Serializable]` for classes that need Inspector serialization.
- Prefer `ScriptableObject` for data that is not tied to a GameObject lifecycle.

## GameObject and Component Access
- Avoid `GameObject.Find` in runtime code — use Inspector injection or service locators.
- Use `GetComponent`/`TryGetComponent` instead of `GetComponent<T>()` for better performance.
- Cache component references in `Awake` or constructor.
- Use VContainer for dependency injection instead of direct references where possible.

## Dependency Injection (VContainer)
- Use `LifetimeScope` to define the root container for a scene or GameObject hierarchy.
- Register services with `LifetimeScope.Register` or via builder pattern: `LifetimeBuilder`.
- Use `IContainer`/`IObjectResolver` for resolving dependencies.
- Prefer constructor injection: declare dependencies as constructor parameters.
- Use `[Inject]` attribute only when constructor injection is not possible.
- Register singletons with `Lifetime.Scoped` or `Lifetime.Singleton` — avoid `Lifetime.Transient` for shared services.
- Use `VContainer.Settings` for configuration.
- For GameObjects, use `LifetimeScope Prefab` or instantiate via container to ensure dependencies are injected.
- Dispose containers manually when needed: `container.Dispose()`.

## Physics
- Use `Physics.Raycast` with a LayerMask to filter interactions.
- Always check `hit.collider` before accessing components.
- Use `TryGetComponent` to avoid null references.

## Async and UniRx
- Use `Awaitable` (Unity 6 async) for all async operations — never use `Task` or `UniTask` unless interop is required.
- Use `UniRx` (`IObservable<T>`) for reactive streams.
- Convert observable streams to awaitable using `UniRxAwaitableExtensions.ToAwaitable<T>(IObservable<T>, CancellationToken)`.
- For waiting on a condition, use `Awaitable.WaitForSecondsAsync` with a `CancellationToken` for cancellation.
- Always pass a `CancellationToken` to async methods so they can be cancelled on `Dispose()`.
- Always dispose subscriptions — store as `IDisposable` and call `Dispose()` on cleanup.
- Avoid `async void` — use `async Awaitable` or handle errors explicitly.

## Logging
- Use `Debug.Log` for information.
- Use `Debug.LogWarning` for recoverable issues.
- Use `Debug.LogError` for critical failures.

## Disposal
- Implement `IDisposable` for classes managing unmanaged resources.
- Cancel `CancellationTokenSource` and dispose it in `Dispose()`.
- Set references to `null` after disposal to prevent reuse.

## Code Structure
- Keep methods small and focused — single responsibility.
- Use private methods for internal logic, public for API.
- Group related fields together at the top of the class.