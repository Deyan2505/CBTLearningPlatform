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

### STEP-3.3 — Трети урок и Content Pipeline Decision Gate
- **Цел:** трети реален урок (горна практическа граница на флагманския модул за MVP) + окончателен, недвусмислен анализ дали е нужен Markdown/JSON content pipeline.
- **Обхват:** `/programa/modul-2/emocii-i-telesni-reaktsii` ("Емоции и телесни реакции"); актуализиран `Modul2.razor` (трета `ModuleCard`); актуализиран Урок 2 (реален линк към Урок 3).
- **Зависимости:** STEP-3.2.
- **Архитектурна реконсилиация (ADR-009, `03_DECISION_LOG.md`):** формализирано решение Вариант C — Модул 2 официално поема базовото ниво на темите от бъдещите Модул 3 ("Автоматични мисли") и Модул 5 ("Емоции и телесни реакции"); тези бъдещи модули се стесняват (Модул 3 → задълбочено записване на мисли/мост към thought record; Модул 5 → задълбочена емоционална регулация), за да не дублират вече наученото. `18_INFORMATION_ARCHITECTURE.md` актуализиран с анотации на засегнатите модули.
- **Content governance:** SRC-041 Гл. 10 ("Идентифициране на емоции"), REQ-CONT-001/002. Нов пример (закъснение в трафик), различен от Урок 1 (съобщение) и Урок 2 (готвене).
- **Acceptance criteria:** build 0/0; 30/30 теста; всички routes 200; Урок 2 вече линква реално към Урок 3; Урок 3 честно маркира липсващ Урок 4 като "предстои"; disclaimer навсякъде; неподкрепената категоризация отсъства.
- **Статус (2026-08-01, Сесия 15): `COMPLETE`.**
- **Content Pipeline Decision Analysis (сравнение на 3 реални урока, без имплементация):**
  - **Повтарящи се елементи:** route metadata, `REQUIRES PROFESSIONAL REVIEW` коментар, `PageTitle`/`h1`, `LearningObjectives` (3 цели всеки), връзка с предходния урок, "Основно обяснение" (2 абзаца), "Пример от ежедневието" (структурирана `.card`/`.stack` markup), опционална сравнителна секция, `callout--info` "Забележете", "Проверка на разбирането" (3–4 въпроса), "Обобщение" (4–5 извода), `DisclaimerCallout`, навигация, `SourceReferences`.
  - **Реални разлики:** Урок 1 има 2 паралелни примера (`.stack` с 2 `.card`), Уроци 2–3 имат по 1; изричен "Сравнение" heading само в Урок 2; брой въпроси/изводи варира леко (3–4/4–5).
  - **Оценка по критерии:** Сложност (Razor: ниска, вече доказана; Markdown: средна — изисква custom block синтаксис за `.card`/`.callout`/`.stack`, за да не се загуби design system интеграцията). Тестируемост (Razor: 30/30 работещи source-базирани теста без триене). Авторска редакция (полза от Markdown е теоретична — единственият автор в момента е през Claude Code сесии, не отделен non-technical редактор). Source traceability/clinical review workflow: идентични и при двата подхода. Performance: Razor е предварително компилиран, без runtime parsing. Migration cost: нисък сега (3 урока), но нараства с всеки следващ. Риск от overengineering: **висок** точно сега — REQ-CONT-002 таванира флагманския модул на 2–4 урока и той вече е при горната граница; pipeline за максимум 1 бъдещ урок в тази конкретна граница не носи пропорционална стойност.
  - **Решение:** `KEEP RAZOR FOR MVP`. Тясната връзка на съдържанието с споделените компоненти (`LearningObjectives`/`SourceReferences`/`DisclaimerCallout`/`ModuleCard`) и вградената structural markup (карти/callouts) прави Markdown миграцията в момента нетна загуба на простота без реална полза — 3 урока е малък обем, а REQ-CONT-002 ограничава по-нататъшния растеж точно на този модул. Решението подлежи на преразглеждане, когато стартира съдържание за Модул 1 или друг бъдещ модул извън капацитета на флагманския Модул 2.
- **Следваща стъпка (не изпълнена автоматично):** Google Fonts CDN решение, Модул 1 съдържание, или STEP-1.6 — собственическо решение, изисква ново извикване.

### STEP-3.4 — Модул 1 overview и първи реален урок
- **Цел:** реализация на Модул 1 ("Какво представлява КПТ") — уводния модул преди флагманския Модул 2.
- **Обхват:** `/programa/modul-1` (overview); `/programa/modul-1/kakvo-e-kpt` (единствения одобрен урок — IA описва Модул 1 като едно цяло, не многоурочна структура); актуализирани `Programa.razor`/`Home.razor` (реален линк вместо disabled state).
- **Зависимости:** STEP-2.2 (`Programa.razor` съществува), независим от STEP-3.1–3.3 (различен модул).
- **Архитектурна проверка:** без конфликт между IA и Requirements Traceability — REQ-CLIN-009 (evidence base) позволява общо, добре подкрепено твърдение, докато специфичната 2006 статистика изрично се нуждае от актуализирана проверка (същото ограничение, приложено вече в STEP-3.1 за `/kpt`). Модул 1 остава честно едноурочен — не е измислена нова подструктура.
- **Content governance:** SRC-041 Гл. 1, REQ-CONT-001, REQ-CLIN-009 (само общата claim). Нов пример (приятел споделя тревога), различен по тема от S-T-E-B примерите в Модул 2 — илюстрира границата образование/терапия, не самия модел (Модул 1 не преподава модела, това е Модул 2).
- **Acceptance criteria:** build 0/0; 38/38 теста; всички routes 200 (вкл. Модул 1 overview + урок); `Programa.razor`/`Home.razor` вече без disabled Модул 1 карта; Урок 1 линква реално напред към Модул 2 (Модул 1 няма урок 2); disclaimer навсякъде; неподкрепената категоризация отсъства.
- **Статус (2026-08-01, Сесия 16): `COMPLETE`.** `REQUIRES PROFESSIONAL REVIEW` маркирано. Visual QA: `PROVISIONAL — SCREENSHOT REVIEW PENDING`. Content Pipeline решението `KEEP RAZOR FOR MVP` — непроменено, потвърдено отново приложимо (Модул 1 съдържанието се вписва в същия established Razor+components модел без триене).
- **Следваща стъпка (не изпълнена автоматично):** Google Fonts CDN решение, STEP-1.6, или бъдещо съдържание извън Модул 1/2 (напр. Модул 14, речник, ЧЗВ) — собственическо решение, изисква ново извикване.

## Visual Direction Correction Checkpoint — dark-first UI + интерактивни учебни визуализации

- **Цел:** собственикът направи първи реален визуален преглед след STEP-3.4 и отхвърли текущата визуална посока (STEP-2.1/2.2 палитра/компоненти) като не-финална — твърде статична, недостатъчна визуална йерархия, недостатъчна интерактивност. Тази checkpoint стъпка коригира посоката, без да променя клинично съдържание или да добавя нови модули/уроци.
- **Обхват:** dark-first, multi-level, не-чисто-черен design system (запазва light темата през session-only toggle, без cookies/localStorage/server storage); reusable интерактивна CBT model диаграма (Interactive WebAssembly, на `/kpt` + Модул 2 Урок 1); малък интерактивен interpretation пример (без scoring/storage) на `/kpt`; learning-path визуализация, заменяща плоския `ModuleCard` списък на Home/Programa; целенасочени page-level refinements (Kpt, Modul2Lesson1, Home, Programa).
- **Зависимости:** Фаза 2 (STEP-2.1/2.2), Фаза 3 (STEP-3.1–3.4) — коригира визуалния слой върху вече съществуващото реално съдържание, не добавя ново съдържание.
- **Skill:** `ui-ux-pro-max` (SKILL-046) — целенасочени dark-theme/accessibility заявки; 2 предложени "dark" стила (pure-black/neon OLED; glassmorphism/blur/glow cinema) изрично отхвърлени като несъвместими с not-pure-black/no-decorative-effects ограниченията — виж `26_SKILL_USAGE_LOG.md`.
- **Acceptance criteria:** build 0/0; 56/56 теста; реален HTTP smoke test на fresh инстанция потвърждава `data-theme="dark"` на initial HTML, всички нови компонентни маркери (theme-toggle/cbt-diagram/interpretation-example/learning-path), prerendered WASM markup, light override в CSS, WCAG AA contrast (реално изчислен, не на око), 0 disabled карти, disclaimer/forbidden-term проверки непроменени.
- **Статус (2026-08-01, Сесия 17): `TECHNICAL QA COMPLETE — OWNER VISUAL APPROVAL PENDING`.** Работата е функционално завършена и технически проверена, но **умишлено не е commit-ната** — собственическата инструкция изисква реален визуален преглед (не само HTTP 200) преди какъвто и да е commit. Собственикът трябва да прегледа `http://localhost:5055` (сървърът в момента е спрян — изисква рестарт за преглед) по чеклист (home/programa/kpt/диаграма/пример/desktop/mobile/dark/light/навигация/четивност на урок), да даде конкретна обратна връзка, след което Claude Code коригира и едва тогава прави един-единствен commit.
- **Следваща стъпка (не изпълнена автоматично):** собственически визуален преглед → конкретна обратна връзка → корекции → commit. Не се стартира thought record упражнение и не се прави commit преди тази обратна връзка.

## Weekly Course Hub + Simulator Foundation Checkpoint — dark-first redesign продължение (ADR-010)

- **Цел:** нов, дългосрочен организационен слой (`/kurs`) над съществуващото Модул 1/2 съдържание — 15-седмичен curriculum reference (`kpt_syllabus.pdf`), progressive disclosure, симулатор-богато обучение като постоянни принципи, реализирани върху вече некомитнатата Visual Direction Correction работа (Сесия 17), не вместо нея.
- **Обхват:** curriculum metadata модел (`Curriculum/CourseCatalog.cs`); curriculum safety classification за всичките 15 седмици; sidebar+top-bar application shell (структурно вдъхновен от собственически reference, без брандинг/gamification); `ProgressiveExplanation` (native `<details>`, 3 употреби); публичен `/kurs` hub (15-елементен списък, честни статус етикети, 0 dead links); представителна `/kurs/sedmica-8` (пълен 8-секционен template, композира върху трите реални Модул 2 урока); нов `CategorizationCheck` interactive knowledge-check (reveal-style, без scoring). Останалите 14 седмици и целият останал simulator каталог — `Deferred`.
- **Зависимости:** Фаза 3 (Модул 1/2 реално съдържание), Visual Direction Correction checkpoint (dark-first design system, `CbtModelDiagram`, `InterpretationExample`).
- **Skill:** `ui-ux-pro-max` — targeted заявки за weekly hub/sidebar/progressive disclosure; двата "dark" стилови резултата (OLED, Cinema Mobile glassmorphism) отхвърлени отново, идентично на Сесия 17 — виж `26_SKILL_USAGE_LOG.md`.
- **Acceptance criteria:** build 0/0; 81/81 теста; всички routes 200 (вкл. `/kurs`, `/kurs/sedmica-8`); несъществуваща седмица → 404; 0 dead links в week list; 0 ECTS/институционални claims; всички интерактивни компонентни маркери реално prerendered; disclaimer/source references никога скрити зад progressive disclosure.
- **Статус (2026-08-01, Сесия 18): `WEEKLY HUB AND SIMULATOR FOUNDATION READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, натрупано върху некомитнатата Сесия 17 работа. Приложението е оставено стартирано на `http://localhost:5055` за собственически преглед на двата checkpoint-а заедно.
- **Следваща стъпка (не изпълнена автоматично):** собственически визуален преглед → обратна връзка → корекции → един commit, покриващ и двата checkpoint-а. Не се строи допълнителна седмица/симулатор без ново извикване.

### Simulator Opportunity Matrix

Пълен каталог на разгледаните interactive/simulator възможности за 15-те седмици (Section 13/14 от Сесия 18 инструкцията). Категории: `SIMULATOR` / `INTERACTIVE MODEL` / `COMPARISON` / `GUIDED DEMONSTRATION` / `KNOWLEDGE CHECK` / `STATIC VISUALIZATION` / `ACADEMIC ONLY`. Реализирани в тази стъпка: **само** редовете, маркирани `IMPLEMENTED` (Седмица 8 × 3). Всички останали — `CATALOGUED, NOT BUILT`, чакат бъдещо, отделно извикване и собствен content governance цикъл преди имплементация.

| Седмица | Възможност | Тип | Safety level | Изисква проф. преглед? | Лични данни? | Изцяло локална? | MVP приоритет | Статус |
|---|---|---|---|---|---|---|---|---|
| 1 | Интерактивна времева линия (психоанализа→емпиричен модел) | INTERACTIVE MODEL | PUBLIC CORE | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 2 | Бек срещу Елис сравнение | COMPARISON | PUBLIC CORE | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 3 | Йерархия (автоматични мисли→междинни→основни) + бърза/рефлексивна обработка | INTERACTIVE MODEL | PUBLIC CORE | Не | Не | Да | P1 | CATALOGUED, NOT BUILT |
| 4 | Демонстрация как професионалист организира информация (без лична история) | ACADEMIC ONLY | ACADEMIC CONTEXT ONLY | Да (за какъвто и да е бъдещ разширен вариант) | Не (по дизайн — забранено въвеждане) | Да | P3 | CATALOGUED, NOT BUILT |
| 5 | Как изглежда сътрудничеството в КПТ | GUIDED DEMONSTRATION | PUBLIC WITH ADAPTATION | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 6 | Walkthrough на структура на сесия (не self-therapy) | GUIDED DEMONSTRATION | PUBLIC WITH ADAPTATION | Не | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 7 | Activity–mood visualizer / basic planning demo | SIMULATOR | PUBLIC WITH ADAPTATION (проф. преглед за самия planner) | Да | Потенциално (ако въвежда лична активност) | Да | P2 | CATALOGUED, NOT BUILT |
| 8 | CBT model simulator | INTERACTIVE MODEL | PUBLIC CORE | Не | Не | Да | P0 | **IMPLEMENTED** (`CbtModelDiagram`, вече от Сесия 17) |
| 8 | Interpretation branching | SIMULATOR | PUBLIC CORE | Не | Не | Да | P0 | **IMPLEMENTED** (`InterpretationExample`, вече от Сесия 17) |
| 8 | Thought-versus-emotion sorter | KNOWLEDGE CHECK | PUBLIC CORE | Не | Не | Да | P0 | **IMPLEMENTED** (`CategorizationCheck`, ново тази стъпка) |
| 9 | Simplified thought record | SIMULATOR | PUBLIC WITH ADAPTATION | Да (отделен content/safety review) | Да (по дизайн, ако някога се въведе) | Да | P0 (бъдещо, EPIC-004) | CATALOGUED, NOT BUILT — изрично забранено за тази стъпка |
| 9 | Distortion explorer | SIMULATOR | PUBLIC WITH ADAPTATION | Да | Не | Да | P1 | CATALOGUED, NOT BUILT |
| 9 | Fact-versus-interpretation activity | SIMULATOR | PUBLIC WITH ADAPTATION | Да | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 10 | Socratic question explorer / alternative-perspective demo | GUIDED DEMONSTRATION | PUBLIC CORE | Не | Не | Да | P1 | CATALOGUED, NOT BUILT |
| 11 | Rules/attitudes/assumptions explorer (без downward-arrow tool) | INTERACTIVE MODEL | PROFESSIONAL REVIEW REQUIRED | Да | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 12 | Schema hierarchy visualization + criteria continuum demo | ACADEMIC ONLY | ACADEMIC CONTEXT ONLY | Да (за какъвто и да е self-guided вариант) | Не | Да | P2 | CATALOGUED, NOT BUILT |
| 13 | Decision balance simulator | SIMULATOR | PUBLIC WITH ADAPTATION | Не | Потенциално | Да | P2 | CATALOGUED, NOT BUILT |
| 13 | Responsibility pie / credit list | SIMULATOR | PUBLIC WITH ADAPTATION | Не | Потенциално | Да | P2 | CATALOGUED, NOT BUILT |
| 13 | Behavioral exposure planner | SIMULATOR | NOT ELIGIBLE FOR SELF-GUIDED SIMULATOR | Да, задължително | Да | Да | Не в MVP | CATALOGUED, NOT BUILT — блокирано без отделен проф. преглед |
| 14 | Maintenance-curve visualization / general planning demo | STATIC VISUALIZATION / GUIDED DEMONSTRATION | PROFESSIONAL REVIEW REQUIRED | Да | Не | Да | P2 | CATALOGUED, NOT BUILT — без relapse-risk predictor |
| 15 | CBT waves timeline / comparison на подходи | ACADEMIC ONLY | ACADEMIC CONTEXT ONLY | Не | Не | Да | P3 | CATALOGUED, NOT BUILT — без CT-R simulator |

**Всички редове по-горе са изцяло локални (без сървърна заявка), без сървърно съхранение на потребителски данни, съгласно ADR-002.** Дизайн договорът за всеки бъдещ симулатор (ясна учебна цел, keyboard/touch support, text fallback, reset, no hidden scoring/diagnosis/AI analysis, mobile layout, unit-testable state, собственически визуален преглед) е формализиран в Сесия 18 инструкцията и се прилага буквално за всяко бъдещо изграждане от този каталог.

### Redesign Round 2 — Content-Rich, Sectioned and Simulator-Led (Сесия 19, 2026-08-03)

- **Повод:** собственически визуален отказ на Сесия 18 резултата — недостатъчна информация, липса на разпознаваем симулатор, еднакви generic карти без визуален anchor.
- **Обхват:** Седмица 8 (`Сряд 8` реда в таблицата по-горе) реализацията надградена на място — `CbtChainSimulator` (control panel + live diagram + reset) замества по-простия `CbtModelDiagram` **само на тази страница** (диаграмата остава непроменена на `/kpt`/Модул 2 Урок 1); `InterpretationExample`/`CategorizationCheck` пренаписани визуално (branch diagram/workspace), не заменени с нови компоненти. `/kurs` получи course map + week timeline вместо плосък списък. Останалите 14 реда в таблицата по-горе остават `CATALOGUED, NOT BUILT`, непроменени.
- **Acceptance criteria:** build 0/0; 107/107 теста; 8 anchor-навигируеми секции на Седмица 8; симулаторът реално prerender-ва всички опции; branch diagram показва и двата пътя едновременно; comparison matrix и concept map присъстват; `.workspace` контейнер дава реална допълнителна ширина за диаграмите спрямо тесния `.content` четивен стълб.
- **Статус (2026-08-03, Сесия 19): `CONTENT-RICH SIMULATOR REDESIGN READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, трети натрупан checkpoint. Пълни детайли в `10_SESSION_LOG.md` (Сесия 19), вкл. точния pre-redesign визуален одит.
- **Следваща стъпка:** собственически визуален преглед на трите натрупани checkpoint-а заедно → обратна връзка → корекции → един commit.

### Redesign Round 3 — Two-Column Workspace and Semantic Color (Сесия 20, 2026-08-03)

- **Повод:** трети собственически визуален отказ — работната зона зад sidebar-а остава на практика едноколонна, недостатъчна цветова диференциация между типовете съдържание.
- **Обхват:** формализиран `.learning-grid` layout foundation (4 съотношения, 1 breakpoint) и 6-ролева семантична цветова система в `app.css` — и двете реюзируеми за всяка бъдеща седмица/страница, не еднократни за Седмица 8. Седмица 8 и `/kurs` преструктурирани върху тази основа; `CbtChainSimulator` вече показва controls и live output едновременно на desktop. `--container-max` увеличен от 1120px на 1400px.
- **Acceptance criteria:** build 0/0; 120/120 теста; ≥4 `.learning-grid` реда на Седмица 8; controls-преди-output ред в симулатора запазен в DOM; Пълно обяснение + Академичен контекст в общ ред без нов anchor; `/kurs` timeline+sidebar split; всичките 6 accent роли реално сервирани и в двете теми; single-column под 900px потвърдено чрез действителния CSS media block, не низово съвпадение.
- **Статус (2026-08-03, Сесия 20): `TWO-COLUMN COLOR-RICH WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, четвърти натрупан checkpoint. Пълни детайли, вкл. точните WCAG изчисления за виолет/синьо/индиго, в `10_SESSION_LOG.md` (Сесия 20).
- **Следваща стъпка:** собственически визуален преглед на четирите натрупани checkpoint-а заедно → обратна връзка → корекции → един commit.

### Redesign Round 4 — Global Two-Column Redesign Across All Existing Routes (Сесия 21, 2026-08-04)

- **Повод:** четвърти собственически визуален отказ, с реален screenshot — двуколонният pattern от Redesign Round 3 беше приложен само локално (Седмица 8 + Kurs); открит и коригиран и реален root-cause CSS bug (`.content` без `margin: auto`, залепено вляво в разширения 1400px контейнер).
- **Обхват:** `.learning-grid` breakpoint мина от viewport `@media` на `@container` (sidebar-aware ширина); нов `LearningSection.razor` reusable wrapper; 2 нови бутонни роли (`.btn-blue`/`.btn-neutral`, вече 5 общо); двуколонният workspace pattern приложен ГЛОБАЛНО на всичките 11 реални routes — Home (пълна преработка), Programa, Kpt (пренаписана въпреки предишен `GOOD — KEEP` одит), Модул 1/Модул 2 overview (нов module-overview шаблон), и четирите реални урока (всеки с конкретен нов visual anchor, калибрирано до 3 реда вместо пълните 4 от инструкцията заради по-краткото им реално съдържание — без изкуствено раздуване/нови клинични твърдения).
- **Acceptance criteria:** build 0/0; 159/159 теста; всичките 11 routes показват реален `.learning-grid` usage (не само CSS class presence — преброен реален count на всеки route); `.content` центриран (margin-inline:auto потвърден); container query потвърдена в served CSS; ≥4 бутонни роли реално използвани (5 постигнати); всеки route запазва disclaimer видим; h1=1 навсякъде.
- **Статус (2026-08-04, Сесия 21): `GLOBAL TWO-COLUMN LEARNING WORKSPACE READY — OWNER VISUAL REVIEW REQUIRED`.** Некомитнато, пети натрупан checkpoint. Пълни детайли, вкл. route-by-route breakdown и root-cause bug анализ, в `10_SESSION_LOG.md` (Сесия 21).
- **Следваща стъпка:** собственически визуален преглед на петте натрупани checkpoint-а заедно → обратна връзка → корекции → един commit.

### Final Visual Polish (Сесия 22, 2026-08-04)

- **Повод:** собственикът потвърди глобалната архитектура от Redesign Round 4 като правилна посока; поискан само финален polish — sidebar active-nav bug, Home CTA/learning-path visual, per-lesson progressive disclosure, подобрени visual anchors, workspace gutters, по-хладна dark палитра.
- **Обхват:** реален sidebar bug fix (`NavLinkMatch.All` на всичките 6 линка — предишните 5 разчитаха на подразбиращия се `Prefix` matching, което правеше родителя и текущата цел active едновременно на вложени routes); Home секции; всичките 4 реални урока получиха progressive disclosure + подобрени visual anchors; `.page-container` responsive gutter; navy-charcoal dark палитра (реално WCAG преизчислена); top bar subtitle.
- **Acceptance criteria:** build 0/0; 185/185 теста; точно 1 active sidebar елемент на всеки основен route (не 0, не 2+); Home CTA-та визуално и структурно различими (violet fill vs. blue outline); learning path е реална диаграма с connectors, не текстови кутии; и четирите урока показват progressive disclosure с основната дефиниция извън disclosure; disclaimer никога вътре в disclosure; dark background тон реално различен от предходния (`#0E1420` vs `#1E1D1B`), light темата непроменена.
- **Статус (2026-08-04, Сесия 22): `FINAL VISUAL POLISH READY — OWNER APPROVAL REQUIRED`.** Некомитнато, шести натрупан checkpoint. Пълни детайли в `10_SESSION_LOG.md` (Сесия 22).
- **Следваща стъпка:** собственически финален визуален преглед на шестте натрупани checkpoint-а заедно → одобрение (един commit) или последна конкретна обратна връзка.

### Final Layout Defect Correction (Сесия 23, 2026-08-04)

- **Повод:** реален собственически screenshot преглед — 12 конкретни проблема (2 от тях blocking root-cause bugs: hoризонтален overflow, `<h1>` focus рамка), останалите polish-ниво (stretched columns, nested card chrome, слаб module learning path, error-styled disclaimer, 18 duplicate role-label/heading двойки, слаб status text, sidebar wrapping, hero density, празен top bar).
- **Обхват:** `.learning-grid > * { min-width:0; align-self:start }` формализиран като задължителна част от grid контракта; heading focus CSS отделено от control focus CSS; `ModuleCard`/`DisclaimerCallout` премахнаха "card in card"/"warning in warning" chrome; Модул 1/Модул 2 получиха реален `.module-path` + concept map; систематичен duplicate-label audit и корекция в цялото приложение.
- **Acceptance criteria:** build 0/0; 227/227 теста; без page-level horizontal scrollbar; `<h1>` без control-style focus рамка при нормално зареждане; grid columns никога не се разтягат с празно пространство; educational disclaimer вече не изглежда като error; Модул 1/2 имат реален visual learning path (не само зелен правоъгълник); 0 duplicate role-label/heading двойки; всичко потвърдено в реално сервирания HTML/CSS, не само в кода.
- **Статус (2026-08-04, Сесия 23): `FINAL LAYOUT DEFECTS FIXED — OWNER APPROVAL REQUIRED`.** Некомитнато, седми натрупан checkpoint. Пълни детайли, вкл. точния root-cause анализ за overflow/focus бъговете, в `10_SESSION_LOG.md` (Сесия 23).
- **Следваща стъпка:** собственически финален визуален преглед на седемте натрупани checkpoint-а заедно → одобрение (един commit) или последна конкретна обратна връзка.

### Checkpoint chain resolved — Сесия 24 (2026-08-05)

Всичките седем checkpoint-а по-горе (Сесии 17–23) одобрени от собственика и committed в един foundation commit (`feat: add interactive CBT learning portal foundation`) след финален pre-flight (build/test/route smoke test). Виж `02_CURRENT_STATUS.md` за текущото шестточково разграничение на обхвата и `10_SESSION_LOG.md` (Сесия 24) за пълни детайли. Следваща фаза: `CONTENT-DRIVEN TEMPLATE VALIDATION` (не започната автоматично).

### CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 1: Седмица 1 (Сесия 25, 2026-08-05)

- **Повод:** след одобрения foundation commit, собственикът стартира новата фаза с изричен обхват — само Седмица 1, без redesign, без промяна на application shell освен при реален блокиращ проблем, без commit в тази стъпка.
- **Цел:** валидира втори representative-week архетип — "Theory and History" (историческо/академично съдържание, времева линия, causal-diagram интерактивност), различен от Седмица 8-ия "Simulator Workspace" архетип — доказвайки, че Weekly Course Hub архитектурата (ADR-010) реално мащабира, преди да се строят допълнителни седмици.
- **Обхват:** нов route `/kurs/sedmica-1`, 9 anchor-navigable секции (Накратко/Историческа линия/Научен обрат/Промяна във фокуса/Автоматични мисли preview/Протоколи и наръчници/1979 milestone/Проверка/Обобщение+академичен контекст+източници); два нови reusable компонента с **нула нови CSS класове** (`HistoricalTimeline.razor` — reuse на `.week-timeline`; `ResearchTurnStepper.razor` — reuse на `.cbt-diagram`, адаптиран директно от `CbtModelDiagram`); `comparison-matrix--dual` (вече дефиниран, но неизползван преди тази сесия) — приложен за таблицата "по-ранен срещу нов изследователски въпрос"; knowledge check чрез native `<details>/<summary>` (нула нов quiz engine); `DisclaimerCallout` разширен с optional `Text` параметър (backward-compatible, default текст непроменен, всички съществуващи извиквания без параметър работят идентично).
- **Source governance:** `kpt_syllabus.pdf` използван само като curriculum map (тема/последователност), не като цитируем източник; институционално оформление (кредити/катедра/акредитация) изрично изключено. Основен източник — SRC-041 (Judith Beck). Двете дати (1964, 1979) проверени срещу `10_SESSION_LOG.md` GAP-012 (Closed, потвърдени directно от SRC-041) — само 1979 показана като точна година на страницата, съгласно изричната инструкция да не се добавя точност отвъд дадената.
- **Source reconciliation (2 конфликта, документирани, не мълчаливо решени):** (1) syllabus твърдението, че автоматичните мисли са "лишени от обективна валидност" — заменено с вече заключената формулировка от платформата ("могат да бъдат точни, неточни или частично подкрепени от фактите"); (2) психоанализата не е представена като "провалена" школа — историческият преход е описан като промяна на изследователския въпрос/метод, не като "победа" на една школа над друга (единственото появяване на "победена" в страницата е вътре в изрична отрицателна конструкция, потвърдено при content QA).
- **Cross-linking:** Седмица 1 → `/kpt`, Модул 1, Седмица 8, `/kurs`; Модул 1 → Седмица 1 (като разширен, незадължителен исторически урок — не променя ролята на съществуващия `Modul1Lesson1`); `Kurs.razor` start panel вече сочи към двете налични седмици.
- **Acceptance criteria:** build 0/0; 248/248 теста (227 baseline + 21 нови/актуализирани); 12/12 реални routes 200, несъществуващ route 404; exactly 1 interactive island на страницата; 0 ECTS/институционален бранд/диагностично съдържание; 0 нови CSS класа; disclaimer/source references никога скрити зад disclosure.
- **Статус (2026-08-05, Сесия 25): `WEEK 1 CONTENT-DRIVEN VERTICAL SLICE READY — OWNER CONTENT AND VISUAL REVIEW REQUIRED`.** Некомитнато. Пълни детайли в `10_SESSION_LOG.md` (Сесия 25).
- **Следваща стъпка:** собственически content + visual + responsive преглед → одобрение (commit) или конкретна обратна връзка. Седмица 2/3/10 не са започнати.

### CONTENT-DRIVEN TEMPLATE VALIDATION — Owner review correction (Сесия 26, 2026-08-06)

- **Повод:** собственически визуален преглед на `/kurs/sedmica-1` — общият "Theory and History" архетип потвърден като успешен и запазен; поискани само точкови content-driven корекции, без redesign, без нова седмица, без промяна на application shell/цветова система/grid архитектура/Course Hub структура.
- **Ключов резултат от валидацията (изрично поискан за запис):** споделените Course Hub timeline и CBT diagram patterns бяха преизползваеми принципно, но реалното историческо съдържание изискваше компактна timeline плътност и отделен, responsive research-stepper layout. Това е валиден резултат от content-driven фазата, не дизайнерска грешка — точно затова се строи и validate-ва един реален vertical slice, преди да се мащабира към други седмици.
- **Public-content cleanup:** премахнати всички вътрешни development термини от рендерирания HTML (`11_SOURCE_REGISTER.md`, `kpt_syllabus.pdf`, `citation-grade`, uppercase `ACADEMIC/CLINICAL REVIEW PENDING`) — заменени с learner-facing академичен текст и обикновен български review статус изречение. Файловите имена остават само във вътрешния `@* ... *@` Razor коментар (никога не достига браузъра).
- **ResearchTurnStepper layout:** нов локален CSS Grid layout variant (`.research-turn-stepper`, app.css), scoped само за този компонент — `CbtModelDiagram`-овият споделен `.cbt-diagram__steps` flex-wrap остава напълно непроменен. 3 breakpoint-а (mobile-first): база = 1 колона; 640–899px = 3-колонен zigzag (ред1: 1→2, ред2: connector надолу, ред3: 3→4); 900px+ = единичен ред от 4 равноправни grid tracks. DOM/tab ред остава 1,2,3,4 на всяка ширина — само CSS визуалното разположение се променя.
- **Compact timeline:** нов `Compact` bool параметър на `HistoricalTimeline.razor` + `.week-timeline--compact` CSS модификатор (по-малък padding/gap/marker размер). Course Hub-овата собствена timeline (`Kurs.razor`) остава напълно непроменена — не подава `Compact`.
- **Sidebar contextual state:** нов "weak parent context" слой в `MainLayout.razor` (`IsSectionContext(prefix)` helper + `NavigationManager`) — `/kurs/*` и `/programa/*` вече показват слаб marker (border + приглушен текст, без background fill, без bold) на родителския sidebar item, докато точният route пази силния `.active` (NavLink, вграден `aria-current="page"`). Тествано: exact routes продължават да имат точно 1 `.active`, 0 `.is-context`; sub-routes вече имат точно 1 `.is-context`, 0 `.active` — старият "двоен active" бъг (Сесия 22) не е върнат.
- **Title/badge:** `CourseCatalog.cs` Week 1 Title скъсен на "Как се ражда когнитивната терапия" (премахнато повторението на "преход" с подзаглавието). Собствен `Налично` статус pill на страницата заменен с content-format badge "Теория и история" (`week-list__status--format`, нов CSS модификатор, academic accent tokens) — Course Hub-ът продължава да показва availability чрез собствената си, непроменена metadata система.
- **Knowledge-check wording + Section 09:** инструкцията пренаписана на по-директен, все още non-scored текст. Section 09 разбита на три ясно различими `<h3>` подблока (Какво да запомните / Академичен контекст / Източници и следващи стъпки) — disclaimer остава отделен визуален callout между тях, educational variant.
- **Section 01 density:** премахнат дублиращ параграф (повтарял hero въведението) — сведен до 1 основна теза + 4 idea chips + 1 заключително изречение.
- **Тестове:** 10 нови факта в `Week1ContentSliceTests.cs` (title/badge, internal-language absence, public review text, stepper layout markers, compact timeline markers, sidebar context markers, knowledge-check wording, Section 09 subblocks) + нови helper методи (`ReadLayoutComponent`, `ReadHostFile`, `ReadCss`, `ReadPublicMarkup` — последният съзнателно маха водещия Razor коментар преди forbidden-term проверки, за да не блъска легитимни вътрешни бележки). 258/258 общо (248 + 10 нови).
- **Проверено:** build 0/0; `git diff --check` чист; 12/12 routes 200 на прясна инстанция; decoded HTML (Python `html.unescape`, Blazor сервира кирилица като numeric entity references) потвърждава — 0 внтрешни термини, публичен review текст присъства, нов badge текст присъства, трите h3 subheadings присъстват, стар дълъг title отсъства; sidebar context/active комбинации потвърдени на `/kurs`, `/kurs/sedmica-1`, `/programa/modul-1`, lesson route.
- **Резултат:** `WEEK 1 CONTENT AND TEMPLATE CORRECTIONS READY — OWNER APPROVAL REQUIRED`. Некомитнато. Пълни детайли в `10_SESSION_LOG.md` (Сесия 26).
- **Следваща стъпка:** собственически финален преглед на Седмица 1 → одобрение (commit) или последна конкретна обратна връзка.

### Week 1 slice committed — Сесия 27 (2026-08-06)

Собственикът одобри съдържанието и визуалната реализация на Седмица 1. Финален pre-flight (build/test/route smoke test/content pre-flight) премина чисто и е създаден единствен commit `feat: add week 1 CBT history learning slice`. "Theory and History" архетипът е `VALIDATED` — вторият representative-week архетип на Weekly Course Hub-а (ADR-010) реално мащабира, включително валидния находка от Сесия 26 (споделен layout pattern недостатъчен за реално съдържание → локален, scoped variant). Виж `02_CURRENT_STATUS.md` за текущото разграничение на обхвата и `10_SESSION_LOG.md` (Сесия 27) за пълен commit hash и diffstat. Следваща фаза: `CONTENT-DRIVEN TEMPLATE VALIDATION — WEEK 3` (не започната автоматично).

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
