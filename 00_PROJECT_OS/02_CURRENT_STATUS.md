# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (следващ урок/модул съдържание, Google Fonts CDN решение, или STEP-1.6):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → секция Фаза 3.
- `07_CONTENT_GOVERNANCE.md` → задължителен преди всяко ново реално учебно съдържание.
- `18_INFORMATION_ARCHITECTURE.md` / `13_REQUIREMENTS_TRACEABILITY.md` → само модула/REQ записите за следващия урок.
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` / `26_SKILL_USAGE_LOG.md` → `ui-ux-pro-max` (SKILL-046).

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15) освен конкретните SRC записи за новото съдържание, PRD (17), пълен Risk Register, пълен Session Log.

## Текуща фаза

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE`; STEP-1.6 `DEFERRED`. Фаза 2 — STEP-2.1/2.2 `COMPLETE`. **Фаза 3 — STARTED.** STEP-3.1 (Първи реален обучителен content slice) `COMPLETE`.

## Текуща стъпка

STEP-3.1 завършена. Следваща (не автоматично изпълнена): следващ урок/модул съдържание (изисква `07_CONTENT_GOVERNANCE.md` цикъл), Google Fonts CDN решение, или STEP-1.6 — собственическо решение, изисква ново извикване.

## Последна завършена задача

STEP-3.1 (Фаза 3, Сесия 13): първи реален обучителен content slice — `/kpt` (публична страница "Какво е КПТ"), `/programa/modul-2` (Модул 2 overview), `/programa/modul-2/situacia-misal-emocia-povedenie` (първи реален урок — модел Ситуация→Мисъл→Емоция→Поведение, REQ-CLIN-003/SRC-041 Гл.3, оригинален неутрален пример, 2-3 рефлективни въпроса, обобщение). Нови reusable `LearningObjectives.razor` + `SourceReferences.razor` (реално използвани 2×). Модул 2 картите на Home/Programa вече сочат към реалното съдържание (вместо disabled state от STEP-2.2). **Реален бъг открит и коригиран по време на изпълнение:** `/kpt` и урокът първоначално нямаха `DisclaimerCallout` въпреки правилото за видим disclaimer на всяка психологическа страница — открито чрез HTTP smoke test, коригирано, добавен регресионен тест. Съдържанието маркирано `REQUIRES PROFESSIONAL REVIEW` (Razor коментари + `13_REQUIREMENTS_TRACEABILITY.md`) — не публикувано за реални потребители без клиничен рецензент (RISK-010). 12 нови теста (общо 21/21 passing).

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
| Public pages | `/`, `/programa`, `/kpt`, `/programa/modul-2`, `/programa/modul-2/situacia-misal-emocia-povedenie` — реални, честни, с `ModuleCard`/`LearningObjectives`/`SourceReferences` компоненти; nav active state (`NavLink`); Google Fonts CDN — временно решение (Variant B), не окончателно production |
| Learning content | Първи реален урок (Модул 2) — `REQUIRES PROFESSIONAL REVIEW`, не публикуван за реални потребители (RISK-010, няма щатен клиничен рецензент) |

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

Следващ урок/модул съдържание (Модул 1 "Какво представлява КПТ", или урок 2 в Модул 2) — изисква нов `07_CONTENT_GOVERNANCE.md` цикъл. Отделно: Google Fonts CDN решение (deferred), STEP-1.6 (`DEFERRED`), файлов Markdown/JSON parsing pipeline (отворен архитектурен въпрос — виж STEP-3.1 в `24_IMPLEMENTATION_ROADMAP.md`). Всички — собственическо решение, изискват ново извикване. `PROVISIONAL — SCREENSHOT REVIEW PENDING` остава в сила. Реален GitHub Actions run остава `PENDING` до създаване на remote.

## Последна актуализация

2026-08-01 — Сесия 13, STEP-3.1 (първи реален обучителен content slice, Фаза 3 стартирана).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а). Фаза 2: ~40% (design system + 2 публични страници). Фаза 3: старт (1 content slice — 3 страници, 1 реален урок). Общ проект (Фази 0–9): ~28%.
