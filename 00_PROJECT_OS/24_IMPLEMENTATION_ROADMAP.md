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
- **Статус (2026-08-01, Сесия 11): `DEFERRED — REQUIRED BEFORE FIRST MAJOR FEATURE MERGE OR REMOTE PR`.** Собственическо решение да се премине директно към Фаза 2. Не блокира UI работа; задължителна преди първи голям feature merge, remote pull request или deployment readiness review.

## Преход към Фаза 2 (дизайн система) — предварителни условия

Фаза 2 не започва, докато Фаза 1 STEP-1.1…1.6 не са `Done` и потвърдени в `02_CURRENT_STATUS.md`. **Изключение (2026-08-01, Сесия 11):** собственикът изрично реши STEP-1.6 да остане `DEFERRED` (не блокира UI работа) и Фаза 2 да започне без нея — виж статус на STEP-1.6 по-горе.

## Фаза 2 — Дизайн система и UX основа: стъпки

### STEP-2.1 — Design system foundation (tokens + базови компоненти)
- **Цел:** последователна, достъпна визуална основа преди писане на съдържание.
- **Обхват:** CSS custom properties (color roles, typography, spacing, shape, layout); базови компоненти — бутони, връзки, карти, callout/disclaimer banner, navigation container (header + footer + skip link), form field foundation; `MainLayout.razor` пренаписан с реална навигация по одобрената IA; `Home.razor` пренаписана с честно, минимално съдържание (не темплейтен placeholder).
- **Зависимости:** Фаза 1 (STEP-1.1–1.5; STEP-1.6 съзнателно отложена).
- **Skill:** `ui-ux-pro-max` (SKILL-046) — `--design-system` + `--domain color/typography/ux` заявки за starting point, адаптирани ръчно (виж `26_SKILL_USAGE_LOG.md` за точните приложени/отхвърлени препоръки).
- **Acceptance criteria:** споделените компоненти реално се използват на `Home.razor` и `NotFound.razor`; build 0/0; тестове зелени; HTTP smoke test потвърждава структурата (nav/header/footer/disclaimer/skip-link/lang="bg") в реално сервирания HTML.
- **Definition of Done (тази стъпка):** design tokens + основни компоненти съществуват и се използват на поне 1 реална страница (`Home.razor`); пълният Phase 2 критерий ("поне 2 реални страници") се постига кумулативно със следващи стъпки/Фаза 3 съдържателни страници.
- **Статус (2026-08-01, Сесия 11): `COMPLETE`.** Виж `10_SESSION_LOG.md` за пълни детайли.

### STEP-2.2 — Real page wireframes and navigation
- **Цел:** поне 2 реални страници, използващи споделената компонентна библиотека (изпълнява Phase 2 критерия за завършване).
- **Обхват:** `Home.razor` пренаписана (hero/процес/модули preview/образователна граница); нова `/programa` страница (модулен каталог); нов `ModuleCard.razor` shared component; nav active state (`NavLink`).
- **Зависимости:** STEP-2.1.
- **Skill:** `ui-ux-pro-max` — landing/UX справки, адаптирани ръчно (отхвърлени testimonials/social-proof/sticky-nav/video-hero модели — виж `26_SKILL_USAGE_LOG.md`).
- **Решение за модулните карти:** Модул 1 и Модул 2 показани с честен статус "в процес на подготовка", **без** линкове към несъществуващи lesson страници — за разлика от header/footer nav (приета практика от STEP-2.1 да сочи към бъдещи routes), модулна карта директно на страница представлява по-силно обещание за клик "сега", затова не се линква преждевременно.
- **Acceptance criteria:** build 0/0; 9/9 теста; `/` и `/programa` връщат HTTP 200; nav active state коректен (`aria-current="page"`); няма dead links; disclaimer видим на двете страници.
- **Статус (2026-08-01, Сесия 12): `COMPLETE`.** Phase 2 критерий за завършване ("споделена библиотека, използвана на поне 2 реални страници") изпълнен. Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING` (HTTP/структурна проверка извършена; няма достъпен browser/screenshot инструмент в тази сесия).
- **Следваща стъпка в Фаза 2/3 (не изпълнена автоматично):** реално съдържание за `/kpt` и модулните lesson страници — изисква `07_CONTENT_GOVERNANCE.md` цикъл, ново извикване.

## Фаза 3 — Система за обучително съдържание: стъпки

### STEP-3.1 — Първи реален обучителен content slice
- **Цел:** малък, напълно използваем обучителен поток: Начало → Програма → Какво е КПТ → Модул → урок, който установява стандарта за всички следващи уроци.
- **Обхват:** `/kpt` (публична страница); `/programa/modul-2` (module overview); `/programa/modul-2/situacia-misal-emocia-povedenie` (първи реален урок); `LearningObjectives.razor` + `SourceReferences.razor` (reusable, реално използвани 2×).
- **Зависимости:** Фаза 2 (STEP-2.1, STEP-2.2).
- **Content governance:** REQ-CLIN-003 (SRC-041 Гл. 3, "Бек 1964; Елис 1962" — не изисква source review, добре установен модел) + REQ-CONT-002/005 (урочен шаблон, флагмански модул като първи урок). REQ-CLIN-008 (10 принципа) и REQ-CLIN-009 (evidence-base статистика от 2006) — **съзнателно изключени** от тази стъпка (Deferred/нужда от актуализирана проверка). Пример в урока — оригинален, неутрален, не адаптиран от `code_artifact.html`.
- **Архитектурна забележка (отворен въпрос, не решен в тази стъпка):** съдържанието е директно в Razor markup, не през Markdown/JSON parsing pipeline, буквално предвиден в Master Plan Фаза 3 ("зареждане и парсване на съдържание от файлове"). ADR-003 (без база данни) не е нарушено, но файлов parsing pipeline остава отворено решение — препоръка: реши се след 2–4 реални урока, когато шаблонът е доказан.
- **Решение за именуване:** инструкцията за тази стъпка илюстративно посочи route `/programa/modul-1` за примерния урок, но описаното съдържание (S→M→E→P модел) е точно одобреният Модул 2 (флагмански) по `18_INFORMATION_ARCHITECTURE.md`/REQ-CONT-005 — реализирано с коректната одобрена номерация вместо примерния route.
- **Реален бъг, открит и коригиран по време на изпълнение:** `/kpt` и урок страницата първоначално нямаха `DisclaimerCallout` (само списък "какво не е"), въпреки изричното правило в `23_CLINICAL_SAFETY_BOUNDARIES.md` за видим disclaimer на **всяка** страница с психологическо съдържание, не само началната. Открито чрез реален HTTP smoke test, коригирано, добавен регресионен тест (`PsychologicalContentPage_IncludesDisclaimerCallout`).
- **Acceptance criteria:** build 0/0; 21/21 теста; всички нови routes връщат HTTP 200; несъществуващ lesson route → friendly NotFound; module overview сочи към реалния първи урок (без dead link); неподкрепената категоризация отсъства; disclaimer видим на всички съдържателни страници.
- **Статус (2026-08-01, Сесия 13): `COMPLETE`.** `REQUIRES PROFESSIONAL REVIEW` маркирано (Razor коментари + `13_REQUIREMENTS_TRACEABILITY.md`) — не публикувано за реални потребители без клиничен рецензент (RISK-010). Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING`.
- **Следваща стъпка (не изпълнена автоматично):** Google Fonts CDN решение (deferred), STEP-1.6 (deferred), или следващ урок/модул — собственическо решение, изисква ново извикване.

### STEP-3.2 — Втори реален урок от Модул 2
- **Цел:** втори реален урок, надграждащ модела от Урок 1, потвърждаващ повторяемостта на урочния шаблон.
- **Обхват:** `/programa/modul-2/avtomatichni-misli` ("Автоматични мисли"); актуализиран `Modul2.razor` overview (втора `ModuleCard`); актуализиран Урок 1 (реален линк към Урок 2 вместо "предстои").
- **Зависимости:** STEP-3.1.
- **Content governance:** тема — SRC-041 Гл. 9 ("Идентифициране на автоматични мисли"), REQ-CONT-001/002. Изтеглена от одобрения content map запис "Модул 3 — Автоматични мисли" (`18_INFORMATION_ARCHITECTURE.md`), реализирана като Урок 2 на флагманския Модул 2 по REQ-CONT-002 ("2–4 lessons in the flagship module"), не като отделна Модул 3 страница — консистентно с решението от STEP-3.1.
- **Нов пример:** различна тема от Урок 1 (готвене по нова рецепта, вместо съобщение без отговор) — демонстрира разграничение мисъл/чувство.
- **Acceptance criteria:** build 0/0; 25/25 теста; всички routes 200; Урок 1 вече линква реално към Урок 2 (без dead "предстои"); Урок 2 честно маркира Урок 3 като "предстои"; disclaimer на всички съдържателни страници; неподкрепената категоризация отсъства.
- **Статус (2026-08-01, Сесия 14): `COMPLETE`.** `REQUIRES PROFESSIONAL REVIEW` маркирано. Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING`.
- **Markdown/JSON pipeline препоръка:** с 2 реални урока структурното повторение личи ясно, но първоначалната препоръка (3–4 урока преди решение) все още не е достигната — препоръка: 1 още урок преди окончателно решение.
- **Следваща стъпка (не изпълнена автоматично):** трети урок, Google Fonts CDN решение, Markdown/JSON pipeline решение, или STEP-1.6 — собственическо решение, изисква ново извикване.

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
