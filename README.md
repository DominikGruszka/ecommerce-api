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

# Ecommerce API

## Wykorzystane usługi Azure:
- Azure App Service (do hostingu API)
- GitHub Actions (do automatycznego wdrażania)

##  Konfiguracja
Nie wymaga dodatkowej konfiguracji po wdrożeniu.  
Po `git push` na gałąź `main`, kod jest automatycznie publikowany do Azure.
