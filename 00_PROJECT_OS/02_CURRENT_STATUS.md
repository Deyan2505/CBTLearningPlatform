# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (STEP-1.3 — основен CI build):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → само секция STEP-1.3.
- `03_DECISION_LOG.md` → само ADR-001/ADR-007 (стек/рендиране), ако STEP-1.3 ги засяга.
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` → само за избор на приложим skill; `26_SKILL_USAGE_LOG.md` → само за кратък запис след реално използване.

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15), PRD/IA/User Flows/Content Model (17–22), Clinical Safety Boundaries (23), пълен Risk Register, пълен Session Log. Отварят се само когато задачата реално засяга тяхната област (виж таблицата в `01_MASTER_PLAN.md` → "Постоянно правило — Context Control").

## Текуща фаза

Фаза 0 — завършена. **Фаза 1 — STARTED.** STEP-1.1 `COMPLETE`. STEP-1.2 `COMPLETE`.

## Текуща стъпка

STEP-1.2 `COMPLETE`. Следваща (не автоматично изпълнена): STEP-1.3 — основен CI build.

## Последна завършена задача

STEP-1.2 изцяло завършена: Git repository инициализиран в **project root** (не в `CbtLearningPlatform/`), официален `.NET .gitignore` темплейт създаден и валидиран (`git check-ignore`), branch `main`, Git identity зададена **локално** (само за това repository, не global), baseline commit създаден с цялата одобрена baseline основа (Project OS + Blazor solution + `code_artifact.html` + `.gitignore`).

## Repository — статус (2026-07-30)

| Параметър | Стойност |
|---|---|
| Repository root | project root (съдържа `00_PROJECT_OS/`, `code_artifact.html`, `CbtLearningPlatform/`) — потвърдено чрез `git rev-parse --show-toplevel` |
| Branch | `main` |
| `.gitignore` | официален `dotnet new gitignore` темплейт, `.vscode/` селективно (не изцяло игнорирана) |
| Git identity | зададена **локално** (`--local`, само това repository); global конфигурация непроменена |
| Baseline commit | **CREATED** — hash виж `10_SESSION_LOG.md` (не се записва email в Project OS) |
| Remote | не съществува (не е част от обхвата) |

## Environment — актуален статус (2026-07-30)

| Компонент | Статус |
|---|---|
| .NET SDK | `10.0.302` инсталиран и потвърден (`C:\Program Files\dotnet\sdk\10.0.302`) |
| .NET Runtimes | 6.0.35, 8.0.17 (запазени непроменени), 10.0.10 (нови) |
| VS Code / Claude Code / Git | Работят |
| Visual Studio (пълен IDE) | Все още не е инсталирана — `OPTIONAL`, не блокира |
| Blazor Web App solution | Съществува — `CbtLearningPlatform/` (2 проекта + `.sln` + `global.json`), build чист след Git init (0/0) |
| Git repository в проекта | **Съществува** (project root), без commits |

## Какво е проверено

- `git rev-parse --show-toplevel` → сочи към project root, не към `CbtLearningPlatform/`.
- `git check-ignore -v` → `bin/`/`obj/` пътища игнорирани; `02_CURRENT_STATUS.md`, `code_artifact.html`, `global.json`, `.sln` — НЕ игнорирани.
- `git status --short` след `git add .` → точно 48 файла (A), без `bin/`/`obj/`/`.vs/`.
- `git diff --cached --stat` → `48 files changed, 4538 insertions(+)`.
- Secret scan (`.env`, `*.pfx`/`*.p12`/`*.pem`/`*.key`, `appsettings*` съдържание) → без открити тайни.
- `dotnet restore` + `dotnet build` след Git init → 0 Warning(s), 0 Error(s) (без регресия).
- `code_artifact.html` — непроменен (122142 bytes, timestamp непроменен).

## Активни проблеми

- Visual Studio (пълен IDE) все още не е инсталирана — `RECOMMENDED`, не блокира.
- Клиничен рецензент все още липсва — не блокира техническа работа.

## Блокиращи проблеми

Няма.

## Следваща препоръчана задача

STEP-1.3 (основен CI build) — **не е изпълнена автоматично**, изисква ново извикване.

## Последна актуализация

2026-07-30 — Сесия 7, STEP-1.2 завършена (Git repository + `.gitignore` + baseline commit).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~33% (2 от 6 STEP-а). Общ проект (Фази 0–9): ~17%.
