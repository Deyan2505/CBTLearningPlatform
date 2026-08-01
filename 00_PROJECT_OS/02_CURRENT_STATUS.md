# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (Фаза 2 — допълнителни wireframes/страници по `22_USER_FLOWS.md`, или STEP-1.6 при собственическо решение):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- `24_IMPLEMENTATION_ROADMAP.md` → секция Фаза 2.
- `18_INFORMATION_ARCHITECTURE.md` / `22_USER_FLOWS.md` → само страницата, която реално се строи.
- `25_CLAUDE_CODE_SKILLS_REGISTRY.md` / `26_SKILL_USAGE_LOG.md` → `ui-ux-pro-max` (SKILL-046) е основният UI/UX skill за всяка следваща UI стъпка.

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15), PRD/Content Model (17, 21), Clinical Safety Boundaries (23) — освен когато страницата реално съдържа психологическо съдържание, пълен Risk Register, пълен Session Log.

## Текуща фаза

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE` (STEP-1.3/1.4/1.5 с `REMOTE CI RUN PENDING`); STEP-1.6 `DEFERRED — REQUIRED BEFORE FIRST MAJOR FEATURE MERGE OR REMOTE PR`. **Фаза 2 — STARTED.** STEP-2.1 (Design system foundation) `COMPLETE`.

## Текуща стъпка

STEP-2.1 завършена. Следваща (не автоматично изпълнена): допълнителни wireframes/страници по `22_USER_FLOWS.md` или разширяване на компонентната библиотека — изисква ново извикване.

## Последна завършена задача

STEP-2.1 (Фаза 2, Сесия 11): реална design system основа — CSS custom properties (color roles/typography/spacing/shape/layout) в `app.css`; базови компоненти (бутони, връзки, карти, `DisclaimerCallout` shared component, navigation container, form field foundation); `MainLayout.razor` пренаписан с реална header/nav/footer/skip-link структура по одобрената IA; `Home.razor` и `NotFound.razor` пренаписани с честно, минимално съдържание (не placeholder). `ui-ux-pro-max` използван за starting point (стил "Accessible & Ethical", шрифт Atkinson Hyperlegible), адаптиран ръчно (напр. заменена предложената cyan/healthcare палитра с по-топла, по-малко "клинична" — виж `26_SKILL_USAGE_LOG.md`). 2 нови теста (общо 6/6 passing). Реален HTTP smoke test потвърди структурата в сервирания HTML.

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

Допълнителни Фаза 2 wireframes/страници по `22_USER_FLOWS.md` (за да се изпълни пълния Phase 2 критерий "поне 2 реални страници"), или STEP-1.6 (`DEFERRED`, задължителна преди първи голям merge/remote PR) — собственическо решение, изисква ново извикване. Реален GitHub Actions run остава `PENDING` до създаване на remote.

## Последна актуализация

2026-08-01 — Сесия 11, STEP-2.1 (Design system foundation, Фаза 2 стартирана).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а — само STEP-1.6 съзнателно отложена). Фаза 2: старт (1 стъпка от няколко). Общ проект (Фази 0–9): ~23%.
