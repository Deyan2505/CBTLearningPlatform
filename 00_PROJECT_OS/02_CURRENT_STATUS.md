# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (STEP-1.6 — linting/formatting конфигурация):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → само секция STEP-1.6.
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` → само за избор на приложим skill; `26_SKILL_USAGE_LOG.md` → само за кратък запис след реално използване.

**Забележка:** преди STEP-1.6 собственикът поиска Phase 1 Remaining Steps Review (виж handoff в `10_SESSION_LOG.md`, Сесия 10) — препоръка за преминаване към UI/UX вместо доизчерпване на цялата инфраструктурна фаза.

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15), PRD/IA/User Flows/Content Model (17–22), Clinical Safety Boundaries (23), пълен Risk Register, пълен Session Log. Отварят се само когато задачата реално засяга тяхната област (виж таблицата в `01_MASTER_PLAN.md` → "Постоянно правило — Context Control").

## Текуща фаза

Фаза 0 — завършена. **Фаза 1 — STARTED.** STEP-1.1 `COMPLETE`. STEP-1.2 `COMPLETE`. STEP-1.3 `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING`. STEP-1.4 `COMPLETE — REMOTE CI RUN PENDING`. STEP-1.5 `COMPLETE — REMOTE CI RUN PENDING`.

## Текуща стъпка

STEP-1.5 завършена локално. Следваща (не автоматично изпълнена, вижте Phase 1 Remaining Steps Review в `10_SESSION_LOG.md`): STEP-1.6 или директно преход към Фаза 2 (собственическо решение).

## Последна завършена задача

STEP-1.5: `<ErrorBoundary>` добавен около `<RouteView>` в `Routes.razor` (минимален български fallback за static SSR слоя); `Error.razor` пренаписана — спокойно българско съобщение, без технически детайли, безопасен `RequestId` (без PII); `#blazor-error-ui` банер преведен (id/класове непроменени — ползвани от `blazor.web.js`). Сървърната инфраструктура (`UseExceptionHandler`/`UseHsts`/`UseStatusCodePagesWithReExecute`) вече беше налична от темплейта — не дублирана. 2 нови reflection-базирани теста (общо 4/4 passing). **Реално симулирано изключение** (временен diagnostic endpoint, премахнат преди commit) в истинска Production среда потвърди: HTTP 500, приятелско съобщение, 0 изтекли технически детайли. **Реален GitHub Actions run е невъзможен — repository няма remote.**

## Repository — статус (2026-07-30)

| Параметър | Стойност |
|---|---|
| Repository root | project root (съдържа `00_PROJECT_OS/`, `code_artifact.html`, `CbtLearningPlatform/`) — потвърдено чрез `git rev-parse --show-toplevel` |
| Branch | `main` |
| `.gitignore` | официален `dotnet new gitignore` темплейт, `.vscode/` селективно (не изцяло игнорирана) |
| Git identity | зададена **локално** (`--local`, само това repository); global конфигурация непроменена |
| Baseline commit | **CREATED** — hash виж `10_SESSION_LOG.md` (не се записва email в Project OS) |
| Remote | не съществува (не е част от обхвата) |
| CI workflow | `.github/workflows/ci.yml` — restore + build + test, конфигуриран локално, никога не е изпълняван на GitHub (няма remote) |
| Test project | `CbtLearningPlatform.Tests` (xUnit, `net10.0`) — 4 теста, всички passing |
| Error handling | `ErrorBoundary` (Routes.razor) + българска `Error.razor` + преведен `#blazor-error-ui` банер; server-side `UseExceptionHandler` от темплейта, проверен реално в Production среда |

## Environment — актуален статус (2026-07-30)

| Компонент | Статус |
|---|---|
| .NET SDK | `10.0.302` инсталиран и потвърден (`C:\Program Files\dotnet\sdk\10.0.302`) |
| .NET Runtimes | 6.0.35, 8.0.17 (запазени непроменени), 10.0.10 (нови) |
| VS Code / Claude Code / Git | Работят |
| Visual Studio (пълен IDE) | Все още не е инсталирана — `OPTIONAL`, не блокира |
| Blazor Web App solution | Съществува — `CbtLearningPlatform/` (2 проекта + `.sln` + `global.json`), build чист след Git init (0/0) |
| Git repository в проекта | **Съществува** (project root), 4 commits (baseline + CI workflow + test foundation + error handling), няма remote |

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

`ui-ux-pro-max` вече `ACTIVE` (project-scoped, `.claude/skills/ui-ux-pro-max/`) — единствената пречка пред Фаза 2 е разрешена. Собственическо решение между STEP-1.6 (linting/formatting, `SAFE TO DEFER`) и директен преход към Фаза 2 (UI/UX) — вижте Phase 1 Remaining Steps Review в `10_SESSION_LOG.md` (Сесия 10). Реален GitHub Actions run остава `PENDING` до създаване на remote (отделно собственическо решение).

## Последна актуализация

2026-07-30 — Сесия 10, STEP-1.5 (error handling, локално завършена) + Phase 1 Remaining Steps Review.

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~75% (4.5 от 6 STEP-а — STEP-1.5 локално завършена, реален CI run pending). Общ проект (Фази 0–9): ~20%.
