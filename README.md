# Ecommerce API

Projekt Web API do zarządzania produktami i zamówieniami w systemie e-commerce.

## CI/CD z GitHub Actions

Workflow `.github/workflows/dotnet.yml` automatycznie buduje projekt po każdym `push` do gałęzi `main`.

### Co robi workflow:
- Odpala się po `push` na `main`
- Przygotowuje środowisko .NET
- Przywraca zależności
- Buduje projekt (`dotnet build`)
- (Opcjonalnie) uruchamia testy (`dotnet test`)
