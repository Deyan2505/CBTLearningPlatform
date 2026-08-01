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

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE`; STEP-1.6 `DEFERRED`. Фаза 2 — STEP-2.1/2.2 `COMPLETE`. **Фаза 3 — STARTED.** STEP-3.1 `COMPLETE`. STEP-3.2 (Втори реален урок от Модул 2) `COMPLETE`.

## Текуща стъпка

STEP-3.2 завършена. Следваща (не автоматично изпълнена): трети урок/следващ модул съдържание (изисква `07_CONTENT_GOVERNANCE.md` цикъл), Google Fonts CDN решение, Markdown/JSON pipeline решение (сега с 2 реални урока за сравнение), или STEP-1.6 — собственическо решение, изисква ново извикване.

## Последна завършена задача

STEP-3.2 (Фаза 3, Сесия 14): втори реален урок в Модул 2 — `/programa/modul-2/avtomatichni-misli` ("Автоматични мисли", SRC-041 Гл.9, тема изтеглена от одобрения content map запис "Модул 3", но реализирана като Урок 2 на флагманския Модул 2 по REQ-CONT-002 "2-4 lessons in flagship module"). Нов пример (готвене, различен от Урок 1), сравнение мисъл-срещу-чувство, "Забележете" callout, 3 рефлективни въпроса. `Modul2.razor` overview актуализиран с втора `ModuleCard`; Урок 1 вече линква реално към Урок 2 (вместо предишното честно "предстои"). 4 нови теста (общо 25/25 passing) — вкл. регресионен тест, че Урок 1 вече няма dead "предстои" следваща стъпка.

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
| Public pages | `/`, `/programa`, `/kpt`, `/programa/modul-2`, + 2 урока (`situacia-misal-emocia-povedenie`, `avtomatichni-misli`) — реални, честни, с `ModuleCard`/`LearningObjectives`/`SourceReferences` компоненти; nav active state (`NavLink`); Google Fonts CDN — временно решение (Variant B), не окончателно production |
| Learning content | 2 реални урока в Модул 2 — `REQUIRES PROFESSIONAL REVIEW`, не публикувани за реални потребители (RISK-010, няма щатен клиничен рецензент) |

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

Трети урок в Модул 2 или Модул 1 съдържание — изисква нов `07_CONTENT_GOVERNANCE.md` цикъл. **Markdown/JSON pipeline решение:** с 2 реални урока структурното повторение вече личи (учебни цели → обяснение → пример → сравнение/callout → проверка → обобщение → navigation → източници), но първоначалната препоръка беше изчакване на 3–4 урока — **все още недостатъчно данни за окончателно решение**, препоръка: 1 още урок преди решаване. Отделно: Google Fonts CDN решение (deferred), STEP-1.6 (`DEFERRED`). Всички — собственическо решение, изискват ново извикване. `PROVISIONAL — SCREENSHOT REVIEW PENDING` остава в сила. Реален GitHub Actions run остава `PENDING` до създаване на remote.

## Последна актуализация

2026-08-01 — Сесия 14, STEP-3.2 (втори реален урок в Модул 2).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а). Фаза 2: ~40% (design system + 2 публични страници). Фаза 3: ~15% (5 страници, 2 реални урока в Модул 2). Общ проект (Фази 0–9): ~30%.
