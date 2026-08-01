# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (Фаза 2/3 продължение — реално съдържание за `/kpt` и Модул 1/2, или STEP-1.6 при собственическо решение):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → секция Фаза 2.
- `18_INFORMATION_ARCHITECTURE.md` / `22_USER_FLOWS.md` → само страницата, която реално се строи.
- `07_CONTENT_GOVERNANCE.md` → задължителен преди писане на реално учебно съдържание (Модул 1/2 текст).
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` / `26_SKILL_USAGE_LOG.md` → `ui-ux-pro-max` (SKILL-046) е основният UI/UX skill за всяка следваща UI стъпка.

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15), PRD (17), пълен Risk Register, пълен Session Log.

## Текуща фаза

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE` (STEP-1.3/1.4/1.5 с `REMOTE CI RUN PENDING`); STEP-1.6 `DEFERRED — REQUIRED BEFORE FIRST MAJOR FEATURE MERGE OR REMOTE PR`. **Фаза 2 — STARTED.** STEP-2.1 (Design system foundation) `COMPLETE`. STEP-2.2 (Real page wireframes and navigation) `COMPLETE`.

## Текуща стъпка

STEP-2.2 завършена. Следваща (не автоматично изпълнена): реално съдържание за `/kpt` и/или модулните страници (изисква `07_CONTENT_GOVERNANCE.md` цикъл), или STEP-1.6 — собственическо решение, изисква ново извикване.

## Последна завършена задача

STEP-2.2 (Фаза 2, Сесия 12): 2 реални публични страници — `Home.razor` пренаписана (hero/как работи/какво ще научите/образователна граница) и нова `/programa` страница (модулен каталог). Нов reusable `ModuleCard.razor` (Title/Description/StatusLabel/DestinationUrl/CtaLabel) — Модул 1 и Модул 2 показани с честен статус "в процес на подготовка", **без dead links** към несъществуващи lesson страници (съзнателно решение). Nav активно състояние чрез Blazor `NavLink` (автоматичен `aria-current="page"` + CSS клас). `ui-ux-pro-max` използван за landing/UX справки, адаптиран ръчно (отхвърлени testimonials/social-proof/sticky-nav модели). 3 нови теста (общо 9/9 passing). Реален HTTP smoke test потвърди двете страници, active nav state, disabled module CTA state, липса на dead links.

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
| Public pages | `/` (Home) и `/programa` — реални, честни, с `ModuleCard` компонент; nav active state (`NavLink`); Google Fonts CDN — временно решение (Variant B), не окончателно production |

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

Пълният Phase 2 критерий "споделена библиотека, използвана на поне 2 реални страници" вече е изпълнен (`Home` + `Programa`). Следваща логична стъпка: реално съдържание за `/kpt` и модулните lesson страници (Фаза 3, изисква `07_CONTENT_GOVERNANCE.md` цикъл), или STEP-1.6 (`DEFERRED`) — собственическо решение, изисква ново извикване. `PROVISIONAL — SCREENSHOT REVIEW PENDING`: реален browser/screenshot QA все още не е извършван (няма достъпен инструмент в тази сесия). Реален GitHub Actions run остава `PENDING` до създаване на remote.

## Последна актуализация

2026-08-01 — Сесия 12, STEP-2.2 (реални публични страници Home + Programa, Фаза 2 критерий изпълнен).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а — само STEP-1.6 съзнателно отложена). Фаза 2: ~40% (2 стъпки — design system + 2 реални страници; предстоят допълнителни wireframes/компоненти при реална нужда). Общ проект (Фази 0–9): ~25%.
