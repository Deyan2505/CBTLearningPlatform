# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (Модул 1 съдържание, Google Fonts CDN решение, или STEP-1.6):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → секция Фаза 3.
- `07_CONTENT_GOVERNANCE.md` → задължителен преди всяко ново реално учебно съдържание.
- `18_INFORMATION_ARCHITECTURE.md` (Модул 1 запис) / `13_REQUIREMENTS_TRACEABILITY.md`.
- `03_DECISION_LOG.md` → ADR-009 (флагмански Модул 2 архитектура) — вече не е активен въпрос, но релевантен контекст.
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` / `26_SKILL_USAGE_LOG.md` → `ui-ux-pro-max` (SKILL-046).

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15) освен конкретните SRC записи за новото съдържание, PRD (17), пълен Risk Register, пълен Session Log.

## Текуща фаза

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE`; STEP-1.6 `DEFERRED`. Фаза 2 — STEP-2.1/2.2 `COMPLETE`. **Фаза 3 — STARTED.** STEP-3.1/3.2 `COMPLETE`. STEP-3.3 (Трети урок + Content Pipeline Decision Gate) `COMPLETE`.

## Текуща стъпка

STEP-3.3 завършена. Флагманският Модул 2 достигна горната граница на REQ-CONT-002 (2–4 урока — сега 3). Следваща (не автоматично изпълнена): Модул 1 съдържание ("Какво представлява КПТ", стеснен обхват след ADR-009), Google Fonts CDN решение, или STEP-1.6 — собственическо решение, изисква ново извикване.

## Последна завършена задача

STEP-3.3 (Фаза 3, Сесия 15): трети реален урок в Модул 2 — `/programa/modul-2/emocii-i-telesni-reaktsii` ("Емоции и телесни реакции", SRC-041 Гл.10). **Архитектурна реконсилиация формализирана в ADR-009** (`03_DECISION_LOG.md`) — Модул 2 официално поема базовото ниво на темите от бъдещите Модул 3/Модул 5; тези модули се стесняват в `18_INFORMATION_ARCHITECTURE.md`, за да не дублират вече наученото. Нов пример (закъснение в трафик), Урок 2 вече линква реално към Урок 3. 5 нови теста (общо 30/30 passing). **Content Pipeline Decision Analysis:** `KEEP RAZOR FOR MVP` — вижте пълния анализ в `24_IMPLEMENTATION_ROADMAP.md` (STEP-3.3).

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
| Design system | `app.css` — tokens (color/typography/spacing/shape/layout) + компоненти (бутони/карти/callout/nav/форми); `DisclaimerCallout` shared component; `MainLayout`/`Home`/`NotFound` реализирани с реално съдържание |
| Public pages | `/`, `/programa`, `/kpt`, `/programa/modul-2`, + 3 урока (`situacia-misal-emocia-povedenie`, `avtomatichni-misli`, `emocii-i-telesni-reaktsii`) — реални, честни, с `ModuleCard`/`LearningObjectives`/`SourceReferences` компоненти; nav active state (`NavLink`); Google Fonts CDN — временно решение (Variant B), не окончателно production |
| Learning content | 3 реални урока в Модул 2 (горна граница на REQ-CONT-002) — `REQUIRES PROFESSIONAL REVIEW`, не публикувани за реални потребители (RISK-010, няма щатен клиничен рецензент) |
| Content architecture | `KEEP RAZOR FOR MVP` (Content Pipeline Decision, STEP-3.3) — Markdown/JSON pipeline не се изгражда; преразглежда се при съдържание извън капацитета на флагманския Модул 2 |

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

Модул 1 съдържание ("Какво представлява КПТ", стеснен обхват след ADR-009) — изисква нов `07_CONTENT_GOVERNANCE.md` цикъл. **Markdown/JSON pipeline: решено — `KEEP RAZOR FOR MVP`** (STEP-3.3), не се преразглежда без ново съдържание извън капацитета на флагманския Модул 2. Отделно: Google Fonts CDN решение (deferred), STEP-1.6 (`DEFERRED`). Всички — собственическо решение, изискват ново извикване. `PROVISIONAL — SCREENSHOT REVIEW PENDING` остава в сила. Реален GitHub Actions run остава `PENDING` до създаване на remote.

## Последна актуализация

2026-08-01 — Сесия 15, STEP-3.3 (трети реален урок, ADR-009 реконсилиация, Content Pipeline Decision: `KEEP RAZOR FOR MVP`).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а). Фаза 2: ~40% (design system + 2 публични страници). Фаза 3: ~20% (5 страници, 3 реални урока — флагманският Модул 2 съдържателно завършен за MVP). Общ проект (Фази 0–9): ~32%.
