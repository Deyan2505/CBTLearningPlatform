# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (STEP-1.5 — централизирана обработка на грешки):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → само секция STEP-1.5.
- `03_DECISION_LOG.md` → приложими ADR за rendering стратегия (ADR-007), ако релевантно.
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` → само за избор на приложим skill; `26_SKILL_USAGE_LOG.md` → само за кратък запис след реално използване.

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15), PRD/IA/User Flows/Content Model (17–22), Clinical Safety Boundaries (23), пълен Risk Register, пълен Session Log. Отварят се само когато задачата реално засяга тяхната област (виж таблицата в `01_MASTER_PLAN.md` → "Постоянно правило — Context Control").

## Текуща фаза

Фаза 0 — завършена. **Фаза 1 — STARTED.** STEP-1.1 `COMPLETE`. STEP-1.2 `COMPLETE`. STEP-1.3 `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING`. STEP-1.4 `COMPLETE — REMOTE CI RUN PENDING`.

## Текуща стъпка

STEP-1.4 завършена локално. Следваща (не автоматично изпълнена): STEP-1.5 — централизирана обработка на грешки.

## Последна завършена задача

STEP-1.4: създаден xUnit test project `CbtLearningPlatform.Tests` (`net10.0`, официален template, добавен към solution, project reference към host `CbtLearningPlatform`), 2 реални инфраструктурни теста (`RuntimeBaselineTests.cs` — target framework на компилирания test assembly, host assembly loadable), и двата минаващи. CI workflow (`.github/workflows/ci.yml`) разширен с `Test` стъпка след `Build`, без промяна на triggers/permissions/runner. Локален Release build + test: 0 Warning(s)/0 Error(s), 2/2 passed. **Реален GitHub Actions run е невъзможен — repository няма remote.**

## Repository — статус (2026-07-30)

| Параметър | Стойност |
|---|---|
| Repository root | project root (съдържа `00_PROJECT_OS/`, `code_artifact.html`, `CbtLearningPlatform/`) — потвърдено чрез `git rev-parse --show-toplevel` |
| Branch | `main` |
| `.gitignore` | официален `dotnet new gitignore` темплейт, `.vscode/` селективно (не изцяло игнорирана) |
| Git identity | зададена **локално** (`--local`, само това repository); global конфигурация непроменена |
| Baseline commit | **CREATED** — hash виж `10_SESSION_LOG.md` (не се записва email в Project OS) |
| Remote | не съществува (не е част от обхвата) |
| CI workflow | `.github/workflows/ci.yml` — restore + build + **test**, конфигуриран локално, никога не е изпълняван на GitHub (няма remote) |
| Test project | `CbtLearningPlatform.Tests` (xUnit, `net10.0`) — 2 теста, и двата passing |

## Environment — актуален статус (2026-07-30)

| Компонент | Статус |
|---|---|
| .NET SDK | `10.0.302` инсталиран и потвърден (`C:\Program Files\dotnet\sdk\10.0.302`) |
| .NET Runtimes | 6.0.35, 8.0.17 (запазени непроменени), 10.0.10 (нови) |
| VS Code / Claude Code / Git | Работят |
| Visual Studio (пълен IDE) | Все още не е инсталирана — `OPTIONAL`, не блокира |
| Blazor Web App solution | Съществува — `CbtLearningPlatform/` (2 проекта + `.sln` + `global.json`), build чист след Git init (0/0) |
| Git repository в проекта | **Съществува** (project root), 2 commits (baseline + CI workflow), няма remote |

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

STEP-1.5 (централизирана обработка на грешки) — **не е изпълнена автоматично**, изисква ново извикване. Реален GitHub Actions run (STEP-1.3 + STEP-1.4 заедно) остава `PENDING` до създаване на remote (отделно собственическо решение, не част от текущия roadmap обхват).

## Последна актуализация

2026-07-30 — Сесия 9, STEP-1.4 (xUnit test project + CI test step, локално завършена).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~58% (3.5 от 6 STEP-а — STEP-1.4 локално завършена, реален CI run pending). Общ проект (Фази 0–9): ~19%.
