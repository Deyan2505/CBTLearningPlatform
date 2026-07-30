# 24 — Implementation Roadmap

**Собственическо одобрение:** получено на 2026-07-30 за всичките 5 предпоставки (MVP, стек — с .NET 10 LTS корекция, IA, data model, prototype решение) — виж ADR-007/ADR-008 в `03_DECISION_LOG.md`.

**STEP-1.1 статус: `COMPLETE`** (2026-07-30, Сесия 6). .NET 10 SDK инсталиран автономно; Blazor Web App solution `CbtLearningPlatform` създаден, build-проверен (0 предупреждения/грешки), `dotnet run` реално проверен (HTTP 200). Пълни доказателства в `02_CURRENT_STATUS.md`, `10_SESSION_LOG.md`, `26_SKILL_USAGE_LOG.md`.

**STEP-1.2 статус: `COMPLETE`** (2026-07-30, Сесия 7). Git repository инициализиран в правилния project root, официален `.NET .gitignore` темплейт създаден и валидиран, branch `main`, Git identity зададена локално (собственикът потвърди email), baseline commit създаден с цялата одобрена baseline основа.

**STEP-1.3 статус: `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING`** (2026-07-30, Сесия 8). `.github/workflows/ci.yml` създаден — restore + Release build, `permissions: contents: read`, SDK от `global.json`, без caching. Security review и структурен YAML преглед чисти; локален Release build 0/0. Реален GitHub Actions run **не е възможен** — repository няма remote (извън обхвата на тази стъпка).

**STEP-1.4 статус: `COMPLETE — REMOTE CI RUN PENDING`** (2026-07-30, Сесия 9). xUnit test project `CbtLearningPlatform.Tests` създаден (официален template, `net10.0`), добавен към solution, project reference към host; 2 реални инфраструктурни теста, и двата passing. CI workflow разширен с `Test` стъпка. Локален build+test: 0/0, 2/2 passed.

**STEP-1.5 статус: `COMPLETE — REMOTE CI RUN PENDING`** (2026-07-30, Сесия 10). `ErrorBoundary` + българска `Error.razor` + преведен `#blazor-error-ui` банер. Server-side exception handling вече беше в темплейта (`UseExceptionHandler`) — не дублиран. Реално симулирано изключение в истинска Production среда потвърди безопасно поведение (HTTP 500, без изтекли детайли). 2 нови теста, общо 4/4 passing. Вижте Phase 1 Remaining Steps Review (`10_SESSION_LOG.md`, Сесия 10) за препоръка относно STEP-1.6 vs. директен преход към Фаза 2.

Детайлните стъпки по-долу покриват Фаза 1 (техническа основа) — непосредствено следващата фаза. Фази 2–9 остават на нивото на детайл в `01_MASTER_PLAN.md`, до момента на реалното им започване (тогава ще получат същото ниво на детайл).

## Фаза 1 — Техническа основа: стъпки

### STEP-1.1 — Инициализация на Blazor Web App solution
- **Цел:** работещ, празен, стартиращ проект.
- **Обхват:** `dotnet new blazor` solution (target framework **`net10.0`**, изрично проверено, не по подразбиране на инсталирания SDK) с основна папкова структура (Components/Pages, Components/Layout, Content, wwwroot).
- **Зависимости:** одобрен технологичен стек (`20_TECHNOLOGY_DECISION.md`, .NET 10 LTS) — **и наличен .NET 10 SDK + Visual Studio на машината, разработваща проекта.**
- **Засегнати файлове:** нов solution (~10-15 нови файла от шаблона).
- **Acceptance criteria:** `dotnet run` показва началната страница локално; `.csproj` сочи `<TargetFramework>net10.0</TargetFramework>`.
- **Тестове:** N/A (скеле).
- **Рискове:** грешен .NET SDK версия инсталирана локално — проверка преди старт (виж `02_CURRENT_STATUS.md` Environment Check — **на 2026-07-30 SDK липсва напълно, стъпката е `BLOCKED`**).
- **Документация:** `10_SESSION_LOG.md` запис.
- **Definition of Done:** проектът се компилира и стартира с една команда от чист clone.

### STEP-1.2 — Git repository и .gitignore
- **Цел:** версионен контрол от самото начало.
- **Обхват:** `git init`, `.gitignore` (стандартен .NET темплейт + IDE файлове), първи commit.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** `git status` чист след build; няма `bin/`/`obj/` в проследяването.
- **Definition of Done:** repo съществува, история започва чисто.
- **Статус (2026-07-30, Сесия 7): `COMPLETE`.** `git init`/`.gitignore` изпълнени и валидирани; acceptance criteria за проследяването изпълнено (`bin/`/`obj/` игнорирани, потвърдено с `git check-ignore`). Git identity зададена локално (собственикът потвърди email при изрична заявка); baseline commit създаден. Definition of Done постигнато — repo съществува, история започва чисто.

### STEP-1.3 — Основен CI build
- **Цел:** автоматична проверка при всеки push.
- **Обхват:** GitHub Actions (или сравним) workflow — restore + build + test.
- **Зависимости:** STEP-1.1, STEP-1.2.
- **Acceptance criteria:** push към main тригерва успешен build.
- **Definition of Done:** CI зелен badge, реално тестван с push.
- **Статус (2026-07-30, Сесия 8): `CI CONFIGURATION COMPLETE — REMOTE RUN PENDING`.** `test` частта на обхвата съзнателно отложена — няма test project (той е предмет на STEP-1.4); workflow-ът ще бъде разширен с `dotnet test`, когато проектът реално съществува, вместо фиктивна команда сега. Acceptance criteria/Definition of Done, изискващи push и зелен badge, остават `PENDING` — repository няма GitHub remote (създаването му е извън обхвата на тази стъпка).

### STEP-1.4 — Тестов проект (xUnit)
- **Цел:** основа за automated tests.
- **Обхват:** нов xUnit проект, референция към основния; един тривиален тест за проверка на настройката.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** `dotnet test` минава.
- **Definition of Done:** CI изпълнява тестовия проект (интегрира се в STEP-1.3).
- **Статус (2026-07-30, Сесия 9): `COMPLETE — REMOTE CI RUN PENDING`.** Вместо един тривиален тест — 2 честни инфраструктурни теста (target framework verification чрез `TargetFrameworkAttribute` на компилирания test assembly — не `AppContext.TargetFrameworkName`, който отразява entry assembly на VSTest testhost, не самия test проект; host assembly loadability). CI интеграцията изпълнена (`.github/workflows/ci.yml` разширен с `Test` стъпка). Реален GitHub run остава `PENDING` — няма remote.

### STEP-1.5 — Централизирана обработка на грешки
- **Цел:** потребителят никога не вижда суров stack trace.
- **Обхват:** глобален error boundary (Blazor) + custom error страница.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** симулирано изключение показва приятелска страница, не suров trace.
- **Тестове:** unit тест за error boundary поведение.
- **Definition of Done:** проверено ръчно + автоматизиран тест.
- **Статус (2026-07-30, Сесия 10): `COMPLETE — REMOTE CI RUN PENDING`.** Acceptance criteria проверено буквално ръчно: временен diagnostic endpoint (премахнат преди commit) хвърлен в истинска Production среда (не Development — `launchSettings.json` override избегнат чрез директно стартиране на compiled DLL) → HTTP 500, приятелско българско съобщение, 0 изтекли технически детайли. Автоматизиран тест: 2 reflection-базирани теста (съществуване на Error компонента + архитектурна гаранция, че той няма property от тип `Exception`).

### STEP-1.6 — Linting/formatting конфигурация
- **Цел:** консистентен код стил.
- **Обхват:** `.editorconfig`, базови Roslyn analyzer правила.
- **Зависимости:** STEP-1.1.
- **Acceptance criteria:** `dotnet format --verify-no-changes` минава на чист проект.
- **Definition of Done:** интегрирано в CI (STEP-1.3) като проверка.

## Преход към Фаза 2 (дизайн система) — предварителни условия

Фаза 2 не започва, докато Фаза 1 STEP-1.1…1.6 не са `Done` и потвърдени в `02_CURRENT_STATUS.md`.

## Фази 2–9 — ниво на детайл

Запазват детайла, вече документиран в `01_MASTER_PLAN.md` (цел, задачи, зависимости, резултат, критерии за приемане, рискове за всяка фаза). Ще бъдат разбити на STEP-ниво детайл (както Фаза 1 по-горе) в началото на всяка съответна фаза, не предварително — за да не се планира в детайл работа, чиито предпоставки (напр. реален MVP UI за Фаза 2 дизайн решенията) все още не съществуват.

## Обобщена зависимост верига

```
Одобрение (MVP + стек + IA + data model + prototype decision)
  → Фаза 1 (STEP-1.1 … 1.6)
    → Фаза 2 (дизайн система)
      → Фаза 3 (съдържателна система, флагмански модул)
        → Фаза 4 (интерактивно упражнение)
          → Фаза 5 (проследяване на прогреса)
            → Фаза 7 (QA преди launch)
              → Фаза 8 (deployment)
```

Фаза 6 (администрация) и Фаза 9 (следващи версии) остават извън тази верига — Deferred, изпълняват се след успешен MVP launch.
