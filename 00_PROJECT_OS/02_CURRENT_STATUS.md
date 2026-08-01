# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (Google Fonts CDN решение, STEP-1.6, или бъдещо съдържание извън Модул 1/2):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → секция Фаза 3.
- `07_CONTENT_GOVERNANCE.md` → задължителен преди всяко ново реално учебно съдържание.
- `18_INFORMATION_ARCHITECTURE.md` / `13_REQUIREMENTS_TRACEABILITY.md` → само модула за следващото съдържание.
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` / `26_SKILL_USAGE_LOG.md` → `ui-ux-pro-max` (SKILL-046).

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15) освен конкретните SRC записи за новото съдържание, PRD (17), пълен Risk Register, пълен Session Log.

## Текуща фаза

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE`; STEP-1.6 `DEFERRED`. Фаза 2 — STEP-2.1/2.2 `COMPLETE`. **Фаза 3 — STARTED.** STEP-3.1–3.3 `COMPLETE`. STEP-3.4 (Модул 1 overview и първи реален урок) `COMPLETE`.

## Текуща стъпка

STEP-3.4 завършена. И Модул 1, и флагманският Модул 2 (горна граница на REQ-CONT-002) вече имат реално съдържание. Следваща (не автоматично изпълнена): Google Fonts CDN решение, STEP-1.6, или бъдещо съдържание извън капацитета на Модул 1/2 — собственическо решение, изисква ново извикване.

## Последна завършена задача

STEP-3.4 (Фаза 3, Сесия 16): Модул 1 overview (`/programa/modul-1`) + първи реален урок (`/programa/modul-1/kakvo-e-kpt`, SRC-041 Гл.1, REQ-CLIN-009 — само общото твърдение, без остарялата 2006 статистика). Нов пример (приятел споделя тревога), илюстриращ границата образование/терапия. `Programa.razor` и `Home.razor` — Модул 1 картите вече сочат към реалното съдържание (вместо disabled state). Урок 1 линква напред към реалния Модул 2 (Модул 1 има само 1 урок, честно). 8 нови теста (общо 38/38 passing).

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
| Public pages | `/`, `/programa`, `/kpt`, `/programa/modul-1` + 1 урок, `/programa/modul-2` + 3 урока — реални, честни, с `ModuleCard`/`LearningObjectives`/`SourceReferences` компоненти; nav active state (`NavLink`); Google Fonts CDN — временно решение (Variant B), не окончателно production |
| Learning content | Модул 1 (1 урок) + Модул 2 (3 урока, горна граница на REQ-CONT-002) — `REQUIRES PROFESSIONAL REVIEW`, не публикувани за реални потребители (RISK-010, няма щатен клиничен рецензент) |
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

Модул 1 и Модул 2 вече имат реално съдържание в границите на одобрения MVP обхват (`19_MVP_SCOPE.md` MUST HAVE #3). Следваща логична стъпка е извън content-slice работата: Google Fonts CDN решение (deferred), STEP-1.6 (`DEFERRED`), или планиране на следваща MVP функционалност (напр. Модул 14 "Кога е нужна професионална помощ", речник, ЧЗВ — вижте `19_MVP_SCOPE.md`). `KEEP RAZOR FOR MVP` остава в сила. Всички — собственическо решение, изискват ново извикване. `PROVISIONAL — SCREENSHOT REVIEW PENDING` остава в сила. Реален GitHub Actions run остава `PENDING` до създаване на remote.

## Последна актуализация

2026-08-01 — Сесия 16, STEP-3.4 (Модул 1 overview + първи реален урок).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а). Фаза 2: ~40% (design system + 2 публични страници). Фаза 3: ~30% (7 страници, 4 реални урока — Модул 1 + флагманският Модул 2 съдържателно завършени за MVP). Общ проект (Фази 0–9): ~34%.
